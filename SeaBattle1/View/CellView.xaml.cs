using System.Windows.Controls;

namespace SeaBattle
{
    /// <summary>
    /// Interaction logic for CellView.xaml
    /// </summary>
    public partial class CellView : UserControl
    {
        public static readonly int CellSize = 30;

        public CellView()
        {
            InitializeComponent();
            this.Width = CellSize;
            this.Height = CellSize;
        }

        /* private void CellButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
         {

         }*/
    }
}
