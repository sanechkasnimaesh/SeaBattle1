using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    /// <summary>
    /// Contains the behavior of boat
    /// </summary>
    public class Boat : INotifyPropertyChanged //ОПИСАНИЕ ПОВЕДЕНИЯ ЛОДКИ
    {
        /// <summary>
        /// Set of cells which belong to this boat.
        /// </summary>
        public List<Cell> Cells;

        /// <summary>
        /// Type of boat
        /// </summary>
        public BoatType Type;

        /// <summary>
        /// Holds the current state of this boat.
        /// </summary>
        public BoatState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }
        BoatState _state;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="p_Cells">List of cells to  be converted into a boat.</param>
        public Boat(List<Cell> p_Cells)
        {
            Cells = p_Cells;

            State = BoatState.Healthy;

            foreach (var cell in p_Cells)
            {
                cell.ParentBoat = this;
            }

            if (Cells.Count == 4)
            {
                Type = BoatType.Battleship;
            }
            else if (Cells.Count == 3)
            {
                Type = BoatType.Cruiser;
            }
            else if (Cells.Count == 2)
            {
                Type = BoatType.Destroyer;
            }
            else if (Cells.Count == 1)
            {
                Type = BoatType.Torpedoboat;
            }
            else
            {
                //TODO
                //AliensShip in our Fleet!
            }
        }

        /// <summary>
        /// Sets the state of the boat after shoot.
        /// </summary>
        /// <param name="p_Cell"></param>
        public void Shoot(Cell p_Cell)
        {
            p_Cell.Style = CellStyle.WoundedCell;

            State = BoatState.Wounded;

            if (Cells.All(c => c.Style == CellStyle.WoundedCell))
            {
                this.Die();
            }
        }
        /// <summary>
        /// Marks all the child cells as dead.
        /// </summary>
        void Die()
        {
            State = BoatState.Dead;

            foreach (var cell in Cells)
            {
                cell.Style = CellStyle.DeadCell;
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

    public enum BoatState
    {
        Healthy, Wounded, Dead
    }

    public enum BoatType
    {
        Battleship, Cruiser, Destroyer, Torpedoboat
    }
}
