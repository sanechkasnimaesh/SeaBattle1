using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBattle
{
    /// <summary>
    /// Contains the behavior of an individual cell.
    /// </summary>
    public class Cell : INotifyPropertyChanged //ПОВЕДЕНИЕ ОТДЕЛЬНОЙ ЯЧЕЙКИ
    {
        /// <summary>
        /// If the cell ever was shooted?
        /// </summary>
        public bool IsShooted
        {
            get
            {
                return _isShooted;
            }
            set
            {
                _isShooted = value;
                OnPropertyChanged("IsShooted");
            }
        }
        private bool _isShooted = false;

        /// <summary>
        /// If this cell is the last shooted cell in this BattleField?
        /// </summary>
        public bool IsLastShooted
        {
            get
            {
                return _isLastShooted;
            }
            set
            {
                _isLastShooted = value;
                OnPropertyChanged("IsLastShooted");
            }
        }
        private bool _isLastShooted = false;

        /// <summary>
        /// If this cell touches any boat?
        /// </summary>
        public bool IsSafeZone = false;

        /// <summary>
        /// If this cellhas an advantage when placing ships?
        /// </summary>
        public bool IsBoatsFriendlyZone = false;

        /// <summary>
        /// If the state of the cell shouldn't be displayed?
        /// </summary>
        public bool IsInvisible
        {
            get
            {
                return _isInvisible;
            }
            set
            {
                _isInvisible = value;
                OnPropertyChanged("IsInvisible");
            }
        }
        bool _isInvisible;

        /// <summary>
        /// The boat which contains this cell.
        /// </summary>
        public Boat ParentBoat { get; set; }

        /// <summary>
        /// The BattleField which contains this cell.
        /// </summary>
        public BattleField ParentBattleField { get; set; }

        /// <summary>
        /// X-coordinate of the cell in the BattleField
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// Y-coordinate of the cell in the BattleField
        /// </summary>
        public int y { get; set; }

        /// <summary>
        /// Style of cell
        /// </summary>
        public CellStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                OnPropertyChanged("Style");
            }
        }
        private CellStyle _style;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_x">X-coordinate of the cell in the BattleField</param>
        /// <param name="p_y">Y-coordinate of the cell in the BattleField</param>
        /// <param name="p_BattleField">The BattleField which contains this cell.</param>
        public Cell(int p_x, int p_y, BattleField p_BattleField)
        {
            x = p_x;
            y = p_y;

            ParentBattleField = p_BattleField;

            Style = CellStyle.Empty;

            if (p_BattleField is EnemyBattleField)
            {
                IsInvisible = true;
            }
        }

        /// <summary>
        /// Method to be called when user draws a cell.
        /// </summary>
        public void DrawCell()
        {
            if (ParentBattleField.Game.Stage == GameStage.BoatsArrange)
            {
                if (ParentBattleField is UserBattleField)
                {
                    Style = CellStyle.HealthyCell;
                }
            }
        }

        /// <summary>
        /// Method to be called when user erases a cell.
        /// </summary>
        public void RemoveCell()
        {
            if (ParentBattleField.Game.Stage == GameStage.BoatsArrange
             && ParentBattleField is UserBattleField)
            {
                Style = CellStyle.Empty;
            }
        }

        /// <summary>
        /// Method to be called when user or computer shoot the cell.
        /// </summary>
        public void Shoot()
        {
            if (!(IsShooted || Style == CellStyle.Shooted)) // user didn't want to click here
            {
                //bool _isSuccess;

                for (int i = 0; i < BattleField.DefaultFieldSize; i++)
                {
                    for (int j = 0; j < BattleField.DefaultFieldSize; j++)
                    {
                        ParentBattleField.Cells[i, j].IsLastShooted = false;
                    }
                }

                IsLastShooted = true;

                if (ParentBoat == null)
                {
                    Style = CellStyle.Shooted;
                    //_isSuccess = false;
                }
                else
                {
                    ParentBoat.Shoot(this);
                    // _isSuccess = true;
                }

                IsShooted = true;
                IsInvisible = false;
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
    }

    public enum CellStyle
    {
        Empty, Shooted, HealthyCell, WoundedCell, DeadCell
    }
}
