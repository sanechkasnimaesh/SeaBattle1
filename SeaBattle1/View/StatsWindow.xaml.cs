using SeaBattle;
using SeaBattle.ViewModel;
using System.Windows;

namespace SeaBattle
{
    public partial class StatsWindow : Window
    {
        public StatsWindow()
        {
            InitializeComponent();
            DataContext = new StatsViewModel();
        }
    }
}