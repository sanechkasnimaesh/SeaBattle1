using SeaBattle.Model;
using System.Collections.ObjectModel;

namespace SeaBattle.ViewModel
{
    public class StatsViewModel
    {
        public ObservableCollection<GameStatsEntry> GameStats { get; set; }

        public StatsViewModel()
        {
            // Инициализация коллекции статистики из статической коллекции Game
            GameStats = new ObservableCollection<GameStatsEntry>(Game.GameStats);
        }
    }
}