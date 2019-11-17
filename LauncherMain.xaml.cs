using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyQuckLauncher {
    /// <summary>
    /// LauncherMain.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherMain : Window {
        public LauncherMain() {
            InitializeComponent();

            var grid = new Grid();
            grid.Width = 300;
            grid.ShowGridLines = true;

            var col1 = new ColumnDefinition();
            col1.Width = new GridLength( 75);
            var col2 = new ColumnDefinition();
            col2.Width = new GridLength(75);
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);

            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(45);
            RowDefinition gridRow2 = new RowDefinition();
            gridRow2.Height = new GridLength(45);
            RowDefinition gridRow3 = new RowDefinition();
            gridRow3.Height = new GridLength(45);

            grid.RowDefinitions.Add(gridRow1);
            grid.RowDefinitions.Add(gridRow2);
            grid.RowDefinitions.Add(gridRow3);

            TextBlock txtBlock1 = new TextBlock();

            txtBlock1.Text = "Author Name";

            txtBlock1.FontSize = 14;

            txtBlock1.FontWeight = FontWeights.Bold;

            txtBlock1.Foreground = new SolidColorBrush(Colors.Green);

            txtBlock1.VerticalAlignment = VerticalAlignment.Top;


            Grid.SetRow(txtBlock1, 1);
            Grid.SetColumn(txtBlock1, 2);

            grid.Children.Add(txtBlock1);
            this.Content = grid;
        }
    }
}
