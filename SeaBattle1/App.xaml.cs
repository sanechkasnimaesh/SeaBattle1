using SeaBattle.View;
using System.Windows;

namespace SeaBattle
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //SQLiteHelper.InitializeDatabase();
            base.OnStartup(e);
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.Show();
        }
    }
}
