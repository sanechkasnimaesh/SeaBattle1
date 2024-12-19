using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    /// <summary>
    /// Сontains the logic of the playing field.
    /// </summary>
    public class BattleField //ПОЛЕ БИТВЫ
    {
        public static readonly int DefaultFieldSize = 10;

        /// <summary>
        /// Set of cells
        /// </summary>
        public Cell[,] Cells;

        /// <summary>
        /// Set of boats connected to this BattleField
        /// </summary>
        public List<Boat> Boats;

        /// <summary>
        /// Parent Game
        /// </summary>
        public Game Game;

        /// <summary>
        /// Method for initial fill.
        /// </summary>
        protected void FillCells()
        {
            Cells = new Cell[DefaultFieldSize, DefaultFieldSize];

            for (int i = 0; i < BattleField.DefaultFieldSize; i++)
            {
                //rows

                for (int j = 0; j < DefaultFieldSize; j++)
                {
                    //columns
                    Cell _cell = new Cell(i, j, this);

                    Cells[i, j] = _cell;

                    _cell.PropertyChanged += Game.Cell_PropertyChanged;
                }
            }
        }
        /// <summary>
        /// Method MUST be performed after filling the Boats.
        /// </summary>
        protected void ConnectBoatToBF(Boat p_Boat)
        {
            p_Boat.PropertyChanged += boat_PropertyChanged;
        }

        /// <summary>
        /// Surrounds boat with the specified cells.
        /// </summary>
        /// <param name="p_boat">The Boat to be surrounded</param>
        /// <param name="p_TargetState">CellStyle to surrond the boat</param>
        /// <param name="p_IsArrangement">Indicates the Stage of Game and affects the safe zone</param>
        public void SurroundBoat(Boat p_boat, CellStyle p_TargetState, bool p_IsArrangement)
        {
            bool _isSafeZone = false;
            bool _isInvisible = false;

            if (p_IsArrangement)
            {
                _isSafeZone = true;
                _isInvisible = true;
            }

            foreach (var cell in p_boat.Cells)
            {
                //nord
                if (cell.y != 0)
                {
                    if (Cells[cell.x, cell.y - 1].Style == CellStyle.Empty)
                    {
                        Cells[cell.x, cell.y - 1].Style = p_TargetState;
                        Cells[cell.x, cell.y - 1].IsSafeZone = _isSafeZone;
                        Cells[cell.x, cell.y - 1].IsInvisible = _isInvisible;
                    }
                }

                //south
                if (cell.y != 9)
                {
                    if (Cells[cell.x, cell.y + 1].Style == CellStyle.Empty)
                    {
                        Cells[cell.x, cell.y + 1].Style = p_TargetState;
                        Cells[cell.x, cell.y + 1].IsSafeZone = _isSafeZone;
                        Cells[cell.x, cell.y + 1].IsInvisible = _isInvisible;
                    }
                }

                //west
                if (cell.x != 0)
                {
                    if (Cells[cell.x - 1, cell.y].Style == CellStyle.Empty)
                    {
                        Cells[cell.x - 1, cell.y].Style = p_TargetState;
                        Cells[cell.x - 1, cell.y].IsSafeZone = _isSafeZone;
                        Cells[cell.x - 1, cell.y].IsInvisible = _isInvisible;
                    }
                }

                //east
                if (cell.x != 9)
                {
                    if (Cells[cell.x + 1, cell.y].Style == CellStyle.Empty)
                    {
                        Cells[cell.x + 1, cell.y].Style = p_TargetState;
                        Cells[cell.x + 1, cell.y].IsSafeZone = _isSafeZone;
                        Cells[cell.x + 1, cell.y].IsInvisible = _isInvisible;
                    }
                }

                //NW corner
                if (cell.y != 0 && cell.x != 0)
                {
                    if (Cells[cell.x - 1, cell.y - 1].Style == CellStyle.Empty)
                    {
                        Cells[cell.x - 1, cell.y - 1].Style = p_TargetState;
                        Cells[cell.x - 1, cell.y - 1].IsSafeZone = _isSafeZone;
                        Cells[cell.x - 1, cell.y - 1].IsInvisible = _isInvisible;
                    }
                }

                //NE corner
                if (cell.y != 0 && cell.x != 9)
                {
                    if (Cells[cell.x + 1, cell.y - 1].Style == CellStyle.Empty)
                    {
                        Cells[cell.x + 1, cell.y - 1].Style = p_TargetState;
                        Cells[cell.x + 1, cell.y - 1].IsSafeZone = _isSafeZone;
                        Cells[cell.x + 1, cell.y - 1].IsInvisible = _isInvisible;
                    }
                }

                //SW corner
                if (cell.y != 9 && cell.x != 0)
                {
                    if (Cells[cell.x - 1, cell.y + 1].Style == CellStyle.Empty)
                    {
                        Cells[cell.x - 1, cell.y + 1].Style = p_TargetState;
                        Cells[cell.x - 1, cell.y + 1].IsSafeZone = _isSafeZone;
                        Cells[cell.x - 1, cell.y + 1].IsInvisible = _isInvisible;
                    }
                }
                //SE corner
                if (cell.y != 9 && cell.x != 9)
                {
                    if (Cells[cell.x + 1, cell.y + 1].Style == CellStyle.Empty)
                    {
                        Cells[cell.x + 1, cell.y + 1].Style = p_TargetState;
                        Cells[cell.x + 1, cell.y + 1].IsSafeZone = _isSafeZone;
                        Cells[cell.x + 1, cell.y + 1].IsInvisible = _isInvisible;
                    }
                }

                if (p_IsArrangement)
                {
                    //nord
                    if (cell.y > 1)
                    {
                        Cells[cell.x, cell.y - 2].IsBoatsFriendlyZone = true;
                    }
                    //south
                    if (cell.y < 8)
                    {
                        Cells[cell.x, cell.y + 2].IsBoatsFriendlyZone = true;
                    }
                    //west
                    if (cell.x > 1)
                    {
                        Cells[cell.x - 2, cell.y].IsBoatsFriendlyZone = true;
                    }
                    //east
                    if (cell.x < 8)
                    {
                        Cells[cell.x + 2, cell.y].IsBoatsFriendlyZone = true;
                    }
                    //NW corner
                    if (cell.y > 1 && cell.x > 1)
                    {
                        Cells[cell.x - 2, cell.y - 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x - 1, cell.y - 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x - 2, cell.y - 1].IsBoatsFriendlyZone = true;
                    }
                    //NE corner
                    if (cell.y > 1 && cell.x < 8)
                    {
                        Cells[cell.x + 2, cell.y - 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x + 1, cell.y - 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x + 2, cell.y - 1].IsBoatsFriendlyZone = true;
                    }
                    //SW corner
                    if (cell.y < 8 && cell.x > 1)
                    {
                        Cells[cell.x - 2, cell.y + 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x - 1, cell.y + 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x - 2, cell.y + 1].IsBoatsFriendlyZone = true;
                    }
                    //SE corner
                    if (cell.y < 8 && cell.x < 8)
                    {
                        Cells[cell.x + 2, cell.y + 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x + 1, cell.y + 2].IsBoatsFriendlyZone = true;
                        Cells[cell.x + 2, cell.y + 1].IsBoatsFriendlyZone = true;
                    }
                }
                else
                {

                }
            }
        }

        void boat_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                Boat _boat = sender as Boat;
                if (_boat != null)
                {
                    if (_boat.State == BoatState.Dead)
                    {
                        SurroundBoat(_boat, CellStyle.Shooted, false);
                    }
                }
            }
        }
    }
}
