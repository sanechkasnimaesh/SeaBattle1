using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using SeaBattle;

namespace SeaBattle.ViewModel
{
    /// <summary>
    /// DataContext for Cell View
    /// </summary>
    public class CellViewModel
    {
        public Cell Cell { get; set; }

        public CellStyle Style { get; set; }

        public CellClickCommand LeftClick { get; set; }

        public CellClickCommand RightClick { get; set; }

        public CellViewModel(Cell p_Cell)
        {
            Cell = p_Cell;

            Style = Cell.Style;

            LeftClick = new CellClickCommand(Cell, MouseButton.Left);
            RightClick = new CellClickCommand(Cell, MouseButton.Right);
        }
    }

    public class CellClickCommand : ICommand
    {
        Cell _Cell;
        MouseButton _MB;

        public bool CanExecute(object parameter)
        {
            bool _result = false;

            if (_Cell.ParentBattleField.Game.Stage == GameStage.BoatsArrange)
            {
                if (_Cell.ParentBattleField is UserBattleField)
                {
                    _result = true;
                }
            }
            else if (_Cell.ParentBattleField.Game.Stage == GameStage.Playing)
            {
                if (_Cell.ParentBattleField is EnemyBattleField)
                {
                    if (_MB == MouseButton.Left)
                    {
                        _result = true;
                    }
                }
            }

            return _result;
        }

        public event EventHandler CanExecuteChanged;

        private void Style_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public void Execute(object parameter)
        {
            if (_Cell.ParentBattleField.Game.Stage == GameStage.BoatsArrange)
            {
                if (_Cell.ParentBattleField is UserBattleField)
                {
                    if (_MB == MouseButton.Left)
                    {
                        _Cell.DrawCell();
                    }
                    else if (_MB == MouseButton.Right)
                    {
                        _Cell.RemoveCell();
                    }
                }
            }
            else if (_Cell.ParentBattleField.Game.Stage == GameStage.Playing)
            {
                if (_Cell.ParentBattleField is EnemyBattleField)
                {
                    if (_MB == MouseButton.Left && _Cell.ParentBattleField.Game.CurrentActor == Actor.User)
                    {
                        _Cell.Shoot();
                    }
                }
            }
        }

        public CellClickCommand(Cell p_Cell, MouseButton p_MB)
        {
            _Cell = p_Cell;
            _MB = p_MB;

            _Cell.PropertyChanged += new PropertyChangedEventHandler(Style_PropertyChanged);
            _Cell.ParentBattleField.Game.PropertyChanged += new PropertyChangedEventHandler(Style_PropertyChanged);
        }
    }
}
