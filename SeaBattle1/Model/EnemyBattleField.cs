using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    /// <summary>
    /// Сontains the specific logic of the playing field of enemy.
    /// </summary>
    public class EnemyBattleField : BattleField
    {
        public EnemyBattleField(Game p_Game)
        {
            Game = p_Game;

            FillCells();
        }

        /// <summary>
        /// Arranges boats automatically
        /// </summary>
        public void ArrangeAutomatically()
        {
            Boats = new List<Boat>();

            // 1. Create the set of Boats with default Coordinates

            for (int i = 0; i < 4; i++)
            {
                int boats_quantity = 4 - i;
                int cells_quantity = i + 1;

                for (int j = 0; j < boats_quantity; j++)
                {
                    List<Cell> curr_boat = new List<Cell>();
                    for (int k = 0; k < cells_quantity; k++)
                    {
                        Cell curr_cell = new Cell(0, 0, this); //default Coordinates
                        curr_boat.Add(curr_cell);
                    }

                    Boats.Add(new Boat(curr_boat));
                }
            }

            // 2. Arrange. we start from the biggest one

            Random r = Game.r;

            for (int k = Boats.Count - 1; k >= 0; k--)
            {
                Boat curr_boat = Boats[k];
                bool succes = false;

                //TODO: make a Timer to restart and avoid the possibility of infinite process
                //arrange or die trying.
                while (!succes)
                {
                    succes = ArrangeBoat(curr_boat, r);
                }
            }

            //ConnectBoatsToBF();
        }

        /// <summary>
        /// Helper method for arrangement of boats of enemy
        /// </summary>
        /// <param name="p_boat">The Boat to be arranged</param>
        /// <param name="r">Object of Random</param>
        /// <returns>returns true if the place for a boat was found.</returns>
        private bool ArrangeBoat(Boat p_boat, Random r)
        {
            bool IsArranged = false;

            //boat has random coordinates and orientation
            bool IsHorisontal = Convert.ToBoolean(r.Next(100) % 2); //0 is horisontal

            int start_x;
            int start_y;

            if (p_boat.Type == BoatType.Battleship)
            {
                //we start placing from one of the walls because it helps to maximize the quantyty of free Cells and to make the Game more difficult
                //select the wall
                int wall = r.Next(100) % 4;

                if (wall == 0)//nord
                {
                    if (IsHorisontal)
                    {
                        start_y = 0;
                        start_x = r.Next(6);
                    }
                    else
                    {
                        start_y = 0;
                        start_x = r.Next(9);
                    }
                }
                else if (wall == 1)//east
                {
                    if (IsHorisontal)
                    {
                        start_x = 0;
                        start_y = r.Next(9);
                    }
                    else
                    {
                        start_x = 0;
                        start_y = r.Next(6);
                    }
                }
                else if (wall == 2)//south
                {
                    if (IsHorisontal)
                    {
                        start_y = 9;
                        start_x = r.Next(6);
                    }
                    else
                    {
                        start_y = 6;
                        start_x = r.Next(9);
                    }
                }
                else//west
                {
                    if (IsHorisontal)
                    {
                        start_x = 6;
                        start_y = r.Next(9);
                    }
                    else
                    {
                        start_x = 9;
                        start_y = r.Next(6);
                    }
                }

                IsArranged = SetCoordinates(p_boat, start_x, start_y, IsHorisontal);
            }
            else if (p_boat.Type == BoatType.Torpedoboat)
            {
                List<Cell> AvailableCell = new List<Cell>();

                for (int i = 0; i < BattleField.DefaultFieldSize; i++)
                {
                    for (int j = 0; j < BattleField.DefaultFieldSize; j++)
                    {
                        if (Cells[i, j].Style == CellStyle.Empty && !Cells[i, j].IsSafeZone)
                        {
                            //Torpedoboats do not like (50/50) to be near other Boats

                            if (Cells[i, j].IsBoatsFriendlyZone)
                            {
                                bool IsTorpederFriendly = Convert.ToBoolean(r.Next(100) % 2);
                                if (IsTorpederFriendly)
                                {
                                    AvailableCell.Add(Cells[i, j]);
                                }
                            }
                            else
                            {
                                AvailableCell.Add(Cells[i, j]);
                            }

                        }
                    }
                }

                //pure random is on!

                if (AvailableCell.Count > 0)
                {
                    int chousenCellNumber = r.Next(AvailableCell.Count);

                    //TODO
                    IsArranged = SetCoordinates(p_boat, AvailableCell[chousenCellNumber].x, AvailableCell[chousenCellNumber].y, IsHorisontal);
                }
                else
                {
                    IsArranged = false;
                }

            }
            else  //multy-cells
            {
                //let's increase the entropy in the world ;)
                if (r.Next(10) == 0) //pure random
                {
                    List<Cell> AvailableCell = new List<Cell>();

                    for (int i = 0; i < BattleField.DefaultFieldSize; i++)
                    {
                        for (int j = 0; j < BattleField.DefaultFieldSize; j++)
                        {
                            if (Cells[i, j].Style == CellStyle.Empty && !Cells[i, j].IsSafeZone)
                            {
                                AvailableCell.Add(Cells[i, j]);
                            }
                        }
                    }

                    int chousenCellNumber = r.Next(AvailableCell.Count);

                    IsArranged = SetCoordinates(p_boat, AvailableCell[chousenCellNumber].x, AvailableCell[chousenCellNumber].y, IsHorisontal);
                }

                else //arrange as close as possible to the Battleship and others to maximaze the quantity of empty Cells
                {
                    List<Cell> AvailableCell = new List<Cell>();

                    for (int i = 0; i < BattleField.DefaultFieldSize; i++)
                    {
                        for (int j = 0; j < BattleField.DefaultFieldSize; j++)
                        {
                            if (Cells[i, j].Style == CellStyle.Empty && !Cells[i, j].IsSafeZone && Cells[i, j].IsBoatsFriendlyZone)
                            {
                                AvailableCell.Add(Cells[i, j]);
                            }
                        }
                    }

                    int chousenCellNumber = r.Next(AvailableCell.Count);

                    IsArranged = SetCoordinates(p_boat, AvailableCell[chousenCellNumber].x, AvailableCell[chousenCellNumber].y, IsHorisontal);
                }

            }

            return IsArranged;
        }
        /// <summary>
        /// Helper method for arrangement of boats of enemy
        /// </summary>
        /// <param name="p_boat">The Boat to be arranged</param>
        /// <param name="p_x">X-coordinate of the first cell of boat</param>
        /// <param name="p_y">Y-coordinate of the first cell of boat</param>
        /// <param name="p_IsHorisontal">Desired position of the boat</param>
        /// <returns>Returns true if the coordinates were set for each cell of the boat.</returns>
        private bool SetCoordinates(Boat p_boat, int p_x, int p_y, bool p_IsHorisontal)
        {
            bool IsSuccess = true;
            Cell _startCell;
            //Cell[,] Cells = Cells[0].ParentBattleField.Cells;


            //check the free space
            //TODO: improve the search of the free space!
            try
            {
                _startCell = Cells[p_x, p_y];

                if (p_IsHorisontal)
                {

                    for (int l = 0; l < p_boat.Cells.Count; l++)
                    {
                        if (!(Cells[_startCell.x + l, _startCell.y].Style == CellStyle.Empty) || Cells[_startCell.x + l, _startCell.y].IsSafeZone)
                        {
                            IsSuccess = false;
                        }
                    }
                }

                else//vertical
                {

                    for (int l = 0; l < p_boat.Cells.Count; l++)
                    {
                        if (!(Cells[_startCell.x, _startCell.y + l].Style == CellStyle.Empty) || Cells[_startCell.x, _startCell.y + l].IsSafeZone)
                        {
                            IsSuccess = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                IsSuccess = false;
            }

            //set the coordinates
            if (IsSuccess)
            {
                if (p_IsHorisontal)
                {
                    for (int l = 0; l < p_boat.Cells.Count; l++)
                    {
                        Cell _cell = Cells[p_x + l, p_y];

                        _cell.Style = CellStyle.HealthyCell;

                        _cell.ParentBoat = p_boat;

                        p_boat.Cells[l] = _cell;
                    }
                }
                else//vertical
                {
                    for (int l = 0; l < p_boat.Cells.Count; l++)
                    {
                        Cell _cell = Cells[p_x, p_y + l];

                        _cell.Style = CellStyle.HealthyCell;

                        _cell.ParentBoat = p_boat;

                        p_boat.Cells[l] = _cell;
                    }
                }

                this.ConnectBoatToBF(p_boat);
            }

            //surround the boat with a safe zone
            SurroundBoat(p_boat, CellStyle.Empty, true);

            return IsSuccess;
        }

        public void RemoveFog()
        {
            for (int i = 0; i < BattleField.DefaultFieldSize; i++)
            {
                for (int j = 0; j < BattleField.DefaultFieldSize; j++)
                {
                    Cells[i, j].IsInvisible = false;
                }
            }
        }

    }
}
