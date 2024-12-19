using SeaBattle;
using SeaBattle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    class BattleFieldViewModel //VM ПОЛЯ БИТВЫ
    {
        BattleField _BattleField;

        public List<object> L1_DisplayedCells { get; set; }


        public BattleFieldViewModel(BattleField p_BattleField)
        {
            _BattleField = p_BattleField;
            FillDisplayedCells();
        }

        void FillDisplayedCells()
        {
            L1_DisplayedCells = new List<object>();

            for (int i = 0; i < BattleField.DefaultFieldSize; i++)
            {
                List<CellViewModel> L2_DisplayedCells = new List<CellViewModel>();

                for (int j = 0; j < BattleField.DefaultFieldSize; j++)
                {
                    L2_DisplayedCells.Add(new CellViewModel(_BattleField.Cells[i, j]));
                }

                L1_DisplayedCells.Add(L2_DisplayedCells);
            }
        }
    }
}
