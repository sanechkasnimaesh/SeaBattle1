using Microsoft.EntityFrameworkCore;
using SeaBattle;
using SeaBattle.Context;
using SeaBattle.Model;
using SeaBattle.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattle
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();
            
            //db.Database.EnsureCreated();
            //db.GameStats.Load();

            MainWindowViewModel _MainWindowVM = new MainWindowViewModel();
            DataContext = _MainWindowVM;

            // Создаем экземпляры BattleFieldView
            var userBattleFieldView = new BattleFieldView
            {
                DataContext = _MainWindowVM.UserBattleFieldVM,
                Margin = new Thickness(10)
            };

            var enemyBattleFieldView = new BattleFieldView
            {
                DataContext = _MainWindowVM.EnemyBattleFieldVM,
                Margin = new Thickness(10)
            };

            // Добавляем их в StackPanel
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(userBattleFieldView);
            stackPanel.Children.Add(enemyBattleFieldView);

            // Добавляем StackPanel в Grid
            var grid = (Grid)Content;
            grid.Children.Add(stackPanel);

        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}