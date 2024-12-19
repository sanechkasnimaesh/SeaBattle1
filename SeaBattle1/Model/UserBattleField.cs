using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    /// <summary>
    /// Содержит логику игрового поля пользователя.
    /// </summary>
    public class UserBattleField : BattleField
    {
        public UserBattleField(Game p_Game) //конструктор класса, инициализирует игровое поле пользователя и заполняет его ячейками
        {
            Game = p_Game;

            FillCells();
        }
        /// <summary>
        /// Парсинг лодки по расположению, сделанному пользователем
        /// </summary>
        public void ParseArrangement() //парсит(?) расстановку кораблей, находя все заполненные ячейки и создавая из них корабли
        {                              //парсинг - анализ данных
            Boats = new List<Boat>();

            // 1.find all filled Cells

            List<Cell> _filledCells = new List<Cell>();

            for (int i = 0; i < BattleField.DefaultFieldSize; i++)
            {
                for (int j = 0; j < BattleField.DefaultFieldSize; j++)
                {
                    if (Cells[i, j].Style == CellStyle.HealthyCell)
                    {
                        _filledCells.Add(Cells[i, j]);
                    }
                }
            }

            //2. among filled Cells find  all neigbours. Create a Boat from them

            while (_filledCells.Count > 0)
            {
                Cell _cell = _filledCells[0];

                List<Cell> _neighbours = FindNeighbours(_cell, _filledCells);

                Boat _Boat = new Boat(_neighbours);
                Boats.Add(_Boat);
            }
        }

        //Achtung! Recursion.
        List<Cell> FindNeighbours(Cell p_Cell, List<Cell> p_SearchList) //рекурсивно находит соседние ячейки, образующие корабль
        {
            List<Cell> _result = new List<Cell>();
            _result.Add(p_Cell);
            p_SearchList.Remove(p_Cell);

            FindNeighbours_recursion(p_Cell, p_SearchList, _result);
            return _result;
        }

        void FindNeighbours_recursion(Cell p_Cell, List<Cell> p_SearchList, List<Cell> p_result)
        {
            if (p_SearchList.Count > 0)
            {
                for (int k = 0; k < p_SearchList.Count; k++)
                {
                    Cell _cell = p_SearchList[k];

                    if (
                        (_cell.x >= p_Cell.x - 1 && _cell.x <= p_Cell.x + 1)
                     && (_cell.y >= p_Cell.y - 1 && _cell.y <= p_Cell.y + 1)
                        )
                    {
                        p_result.Add(_cell);
                        p_SearchList.Remove(_cell);
                        k--;

                        if (p_SearchList.Count > 0)
                        {
                            FindNeighbours_recursion(_cell, p_SearchList, p_result);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if parsed boats are correct and connects them to the BattleField in the case of success
        /// </summary>
        /// <param name="p_message">Error string</param>
        /// <returns>returns true if arrangement was correct</returns>
        public bool CheckArrangement(out string p_message) //проверка правильности расстановки кораблей
        {
            // 3. Check if Boats are correct

            bool _result = true;

            //first check quantity

            if (Boats != null)
            {
                if (Boats.All(b => (b.Cells.Select(c => c.x).Distinct().Count() == 1)
                                 || (b.Cells.Select(c => c.y).Distinct().Count() == 1)
                             )
                   )
                {
                    if (Boats.Count == 10)
                    {
                        if (Boats.All(a => a.Cells.Count <= 4))
                        {
                            if (
                            Boats.Count(a => a.Cells.Count == 4) == 1
                         && Boats.Count(a => a.Cells.Count == 3) == 2
                         && Boats.Count(a => a.Cells.Count == 2) == 3
                         && Boats.Count(a => a.Cells.Count == 1) == 4
                            )
                            {
                                _result = true;
                                p_message = "А ты умен, начинай бой!";
                            }
                            else
                            {
                                _result = false;
                                p_message = "Неправильно корабли стоят ну посмотри ты на инструкцию";
                            }
                        }
                        else
                        {
                            _result = false;
                            p_message = "Некоторые корабли слишком длинные";
                        }

                    }
                    else
                    {
                        _result = false;
                        p_message = "Неверное количество кораблей";
                    }
                }
                else
                {
                    _result = false;
                    p_message = "Корабль не должен наклоняться или касаться другого коробля по диагонали!";
                }
            }
            else
            {
                throw new Exception("Before Check the Boats should be parsed");
            }


            if (_result)
            {
                foreach (var boat in Boats)
                {
                    this.ConnectBoatToBF(boat);
                }
            }
            return _result;
        }

    }
}
