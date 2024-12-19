using System.Windows.Input;
using System.Windows;
using SeaBattle;

namespace SeaBattle
{
    public class MainMenuViewModel
    {
        public ICommand PlayCommand { get; }
        public ICommand ShowStatsCommand { get; }
        public ICommand ExitCommand { get; }

        public MainMenuViewModel()
        {
            PlayCommand = new RelayCommand(Play);
            ShowStatsCommand = new RelayCommand(ShowStats);
            ExitCommand = new RelayCommand(Exit);
        }

        private void Play(object parameter)
        {
            // Логика для запуска игры
            var gameWindow = new MainWindow(); // Создаем новое окно игры
            gameWindow.Show(); // Открываем окно игры

            // Если нужно, чтобы окно игры было модальным (блокирует доступ к окну меню)
            // gameWindow.ShowDialog();

            // Не закрываем окно меню, если оно является главным окном
            // Application.Current.MainWindow.Close();
            Application.Current.MainWindow.Close();

            // Устанавливаем новое окно игры как главное окно (если нужно)
            Application.Current.MainWindow = gameWindow;
        }

        private void ShowStats(object parameter)
        {
            // Логика для показа статистики
            var statsWindow = new StatsWindow();
            statsWindow.ShowDialog();
        }

        private void Exit(object parameter)
        {
            // Логика для выхода из приложения
            Application.Current.Shutdown();
        }
    }
}