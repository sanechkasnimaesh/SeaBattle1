using SeaBattle.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SeaBattle
{
    public class Game : INotifyPropertyChanged
    {
        public UserBattleField UserBattleField { get; set; }
        public EnemyBattleField EnemyBattleField { get; set; }

        public static List<GameStatsEntry> GameStats = new List<GameStatsEntry>();

        public GameStage Stage
        {
            get { return _stage; }
            set
            {
                _stage = value;
                OnPropertyChanged("Stage");
            }
        }
        GameStage _stage;

        public Actor CurrentActor;

        public GameResult Result { get; set; }

        public int UserScores
        {
            get { return _userScores; }
            set
            {
                _userScores = value;
                OnPropertyChanged("UserScores");
            }
        }
        int _userScores = 0;

        public int EnemyScores
        {
            get { return _enemyScores; }
            set
            {
                _enemyScores = value;
                OnPropertyChanged("EnemyScores");
            }
        }
        int _enemyScores = 0;

        public DateTime StartTime { get; private set; }

        Dictionary<BoatType, int> CostOfCell = new Dictionary<BoatType, int>
        {
            {BoatType.Battleship, 15},
            {BoatType.Cruiser, 30},
            {BoatType.Destroyer, 60},
            {BoatType.Torpedoboat, 150}
        };

        public string ResultMessage;

        public Random r;

        public Game()
        {
            UserBattleField = new UserBattleField(this);
            EnemyBattleField = new EnemyBattleField(this);

            Stage = GameStage.BoatsArrange;
            CurrentActor = Actor.User;
            Result = GameResult.InPlay;

            r = new Random();
            StartTime = DateTime.Now;
        }

        public void ShootToUserBFAutomatically()
        {
            int _targetX = 0;
            int _targetY = 0;

            List<Cell> _knownCells = new List<Cell>();

            for (int i = 0; i < UserBattleField.DefaultFieldSize; i++)
            {
                for (int j = 0; j < UserBattleField.DefaultFieldSize; j++)
                {
                    if (UserBattleField.Cells[i, j].Style == CellStyle.Shooted || UserBattleField.Cells[i, j].Style == CellStyle.WoundedCell || UserBattleField.Cells[i, j].Style == CellStyle.DeadCell)
                    {
                        _knownCells.Add(UserBattleField.Cells[i, j]);
                    }
                }
            }

            if (_knownCells.Count == 0)
            {
                int corner = r.Next(3);
                int padding_x = r.Next(100) % 2;
                int padding_y = r.Next(100) % 2;

                if (corner == 0)
                {
                    _targetX = 0 + padding_x;
                    _targetY = 0 + padding_y;
                }
                else if (corner == 1)
                {
                    _targetX = 9 - padding_x;
                    _targetY = 0 + padding_y;
                }
                else if (corner == 2)
                {
                    _targetX = 0 + padding_x;
                    _targetY = 9 - padding_y;
                }
                else if (corner == 3)
                {
                    _targetX = 9 - padding_x;
                    _targetY = 9 - padding_y;
                }
            }
            else
            {
                List<Cell> woundedCells = (from cell in _knownCells where cell.Style == CellStyle.WoundedCell select cell).ToList();

                if (woundedCells.Count() > 0)
                {
                    if (woundedCells.Count > 1)
                    {
                        if ((from cell in woundedCells select cell.x).Distinct().Count() == 1)
                        {
                            int searchDirection = 0;
                            bool IsNextShootFound = false;

                            while (searchDirection < 2 && !IsNextShootFound)
                            {
                                if (searchDirection == 0)
                                {
                                    _targetY = (from cell in woundedCells select cell.y).Max() + 1;
                                    _targetX = woundedCells[0].x;
                                }
                                else
                                {
                                    _targetY = (from cell in woundedCells select cell.y).Min() - 1;
                                    _targetX = woundedCells[0].x;
                                }

                                if (_targetX >= 0 &&
                               _targetX <= 9 &&
                               _targetY >= 0 &&
                               _targetY <= 9)
                                {
                                    if (UserBattleField.Cells[_targetX, _targetY].Style == CellStyle.HealthyCell ||
                                        UserBattleField.Cells[_targetX, _targetY].Style == CellStyle.Empty)
                                    {
                                        IsNextShootFound = true;
                                    }
                                }

                                searchDirection++;
                            }
                        }
                        else if ((from cell in woundedCells select cell.y).Distinct().Count() == 1)
                        {
                            int searchDirection = 0;
                            bool IsNextShootFound = false;

                            while (searchDirection < 2 && !IsNextShootFound)
                            {
                                if (searchDirection == 0)
                                {
                                    _targetX = (from cell in woundedCells select cell.x).Max() + 1;
                                    _targetY = woundedCells[0].y;
                                }
                                else
                                {
                                    _targetX = (from cell in woundedCells select cell.x).Min() - 1;
                                    _targetY = woundedCells[0].y;
                                }

                                if (_targetX >= 0 &&
                              _targetX <= 9 &&
                              _targetY >= 0 &&
                              _targetY <= 9)
                                {
                                    if (UserBattleField.Cells[_targetX, _targetY].Style == CellStyle.HealthyCell ||
                                        UserBattleField.Cells[_targetX, _targetY].Style == CellStyle.Empty)
                                    {
                                        IsNextShootFound = true;
                                    }
                                }

                                searchDirection++;
                            }
                        }
                    }
                    else
                    {
                        int searchDirection = 0;
                        bool IsNextShootFound = false;
                        Cell selectedCell = woundedCells[0];

                        while (searchDirection < 4 && !IsNextShootFound)
                        {
                            if (searchDirection == 0)
                            {
                                _targetX = selectedCell.x;
                                _targetY = selectedCell.y - 1;
                            }
                            else if (searchDirection == 1)
                            {
                                _targetX = selectedCell.x + 1;
                                _targetY = selectedCell.y;
                            }
                            else if (searchDirection == 2)
                            {
                                _targetX = selectedCell.x;
                                _targetY = selectedCell.y + 1;
                            }
                            else if (searchDirection == 3)
                            {
                                _targetX = selectedCell.x - 1;
                                _targetY = selectedCell.y;
                            }

                            if (_targetX >= 0 &&
                                 _targetX <= 9 &&
                                 _targetY >= 0 &&
                                 _targetY <= 9)
                            {
                                if (UserBattleField.Cells[_targetX, _targetY].Style == CellStyle.HealthyCell ||
                                    UserBattleField.Cells[_targetX, _targetY].Style == CellStyle.Empty)
                                {
                                    IsNextShootFound = true;
                                }
                            }

                            searchDirection++;
                        }
                    }
                }
                else
                {
                    int maxLenght =
                        (from boat in UserBattleField.Boats where boat.Cells.Any(c => c.Style == CellStyle.HealthyCell) select boat.Cells.Count).Max();

                    if (maxLenght > 1)
                    {
                        bool IsVerticalSearch = Convert.ToBoolean(r.Next(100) % 2);
                        bool IsReverseSearch = Convert.ToBoolean(r.Next(50) % 2);

                        List<Cell> LongLine = new List<Cell>();
                        bool IsLongLineFound = false;

                        while (!IsLongLineFound)
                        {
                            if (!IsVerticalSearch)
                            {
                                if (!IsReverseSearch)
                                {
                                    int i = 0;
                                    int j = 0;

                                    while (i < 10 && j < 10 && !IsLongLineFound)
                                    {
                                        if (UserBattleField.Cells[i, j].Style == CellStyle.HealthyCell || UserBattleField.Cells[i, j].Style == CellStyle.Empty)
                                        {
                                            LongLine.Add(UserBattleField.Cells[i, j]);
                                        }
                                        else
                                        {
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }

                                        i++;

                                        if (i > 9)
                                        {
                                            i = 0;
                                            j++;
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int i = 9;
                                    int j = 9;
                                    while (i >= 0 && j >= 0 && !IsLongLineFound)
                                    {
                                        if (UserBattleField.Cells[i, j].Style == CellStyle.HealthyCell || UserBattleField.Cells[i, j].Style == CellStyle.Empty)
                                        {
                                            LongLine.Add(UserBattleField.Cells[i, j]);
                                        }
                                        else
                                        {
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }

                                        i--;

                                        if (i < 0)
                                        {
                                            i = 9;
                                            j--;
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }
                                    }
                                }

                                IsVerticalSearch = !IsVerticalSearch;
                            }
                            else
                            {
                                if (!IsReverseSearch)
                                {
                                    int i = 0;
                                    int j = 0;

                                    while (i < 10 && j < 10 && !IsLongLineFound)
                                    {
                                        if (UserBattleField.Cells[i, j].Style == CellStyle.HealthyCell || UserBattleField.Cells[i, j].Style == CellStyle.Empty)
                                        {
                                            LongLine.Add(UserBattleField.Cells[i, j]);
                                        }
                                        else
                                        {
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }

                                        j++;

                                        if (j > 9)
                                        {
                                            j = 0;
                                            i++;
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int i = 9;
                                    int j = 9;
                                    while (i >= 0 && j >= 0 && !IsLongLineFound)
                                    {
                                        if (UserBattleField.Cells[i, j].Style == CellStyle.HealthyCell || UserBattleField.Cells[i, j].Style == CellStyle.Empty)
                                        {
                                            LongLine.Add(UserBattleField.Cells[i, j]);
                                        }
                                        else
                                        {
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }

                                        j--;

                                        if (j < 0)
                                        {
                                            j = 9;
                                            i--;
                                            if (LongLine.Count >= maxLenght)
                                            {
                                                IsLongLineFound = true;
                                            }
                                            else
                                            {
                                                LongLine = new List<Cell>();
                                            }
                                        }
                                    }
                                }
                            }

                            IsVerticalSearch = !IsVerticalSearch;
                        }
                        _targetX = LongLine[maxLenght - 1].x;
                        _targetY = LongLine[maxLenght - 1].y;
                    }
                    else
                    {
                        List<Cell> possibleVictim = new List<Cell>();
                        for (int i = 0; i < UserBattleField.DefaultFieldSize; i++)
                        {
                            for (int j = 0; j < UserBattleField.DefaultFieldSize; j++)
                            {
                                if (UserBattleField.Cells[i, j].Style == CellStyle.Empty || UserBattleField.Cells[i, j].Style == CellStyle.HealthyCell)
                                {
                                    possibleVictim.Add(UserBattleField.Cells[i, j]);
                                }
                            }
                        }
                        if (possibleVictim.Count > 0)
                        {
                            int victimNumber = r.Next(possibleVictim.Count - 1);
                            _targetX = possibleVictim[victimNumber].x;
                            _targetY = possibleVictim[victimNumber].y;
                        }
                    }
                }
            }

            Cell shootedCell = UserBattleField.Cells[_targetX, _targetY];
            shootedCell.Shoot();
        }

        public bool IsOver()
        {
            bool _isOver = false;

            if (CurrentActor == Actor.User)
            {
                if (EnemyBattleField.Boats.All(b => b.State == BoatState.Dead))
                {
                    Result = GameResult.Victory;
                    Stage = GameStage.Finished;
                    EnemyBattleField.RemoveFog();
                    _isOver = true;

                    // Сохранение статистики
                    SaveGameStats(UserScores, EnemyScores, StartTime);
                }
            }
            else
            {
                if (UserBattleField.Boats.All(b => b.State == BoatState.Dead))
                {
                    Result = GameResult.Defeat;
                    Stage = GameStage.Finished;
                    EnemyBattleField.RemoveFog();
                    _isOver = true;

                    // Сохранение статистики
                    SaveGameStats(UserScores, EnemyScores, StartTime);
                }
            }

            return _isOver;
        }

        private void SaveGameStats(int userScore, int enemyScore, DateTime startTime)
        {
            GameStatsEntry statsEntry = new GameStatsEntry
            {
                UserScore = userScore,
                EnemyScore = enemyScore,
                StartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss")
            };

            Game.GameStats.Add(statsEntry);
        }

        public void AssessShoot(Actor p_actor, Cell p_cell)
        {
            int _prise;

            if (p_cell.ParentBoat != null)
            {
                _prise = CostOfCell[p_cell.ParentBoat.Type];
            }
            else
            {
                _prise = -5;
            }

            if (p_actor == Actor.Computer)
            {
                EnemyScores += _prise;
            }
            else
            {
                UserScores += _prise;
            }
        }

        public bool IsTargetStageAvailable(GameStage p_TargetStage)
        {
            bool _canExecute = false;

            if (Stage == GameStage.BoatsArrange)
            {
                if (p_TargetStage == GameStage.Playing)
                {
                    _canExecute = true;
                }
            }
            else if (Stage == GameStage.Playing)
            {
                if (p_TargetStage == GameStage.Finished)
                {
                    _canExecute = true;
                }
            }

            return _canExecute;
        }

        public void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsShooted")
            {
                Cell _shootedCell = sender as Cell;

                if (_shootedCell != null)
                {
                    this.AssessShoot(this.CurrentActor, _shootedCell);

                    bool _isSuccess =
                        _shootedCell.Style == CellStyle.DeadCell || _shootedCell.Style == CellStyle.WoundedCell ?
                        true : false;

                    if (_isSuccess)
                    {
                        if (!this.IsOver())
                        {
                            if (this.CurrentActor == Actor.Computer)
                            {
                                this.ShootToUserBFAutomatically();
                            }
                        }
                    }
                    else
                    {
                        if (this.CurrentActor == Actor.User)
                        {
                            this.CurrentActor = Actor.Computer;
                            if (!this.IsOver())
                            {
                                this.ShootToUserBFAutomatically();
                            }
                        }
                        else
                        {
                            this.CurrentActor = Actor.User;
                        }
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //public void SaveGameStats()
        //{
        //    SQLiteHelper.AddGameStats(UserScores, EnemyScores, StartTime);
        //}
    }

    public enum GameStage
    {
        BoatsArrange, Playing, Finished
    }

    public enum Actor
    {
        User, Computer
    }

    public enum GameResult
    {
        InPlay, Victory, Defeat
    }
}