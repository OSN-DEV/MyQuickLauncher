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
using MyQuckLauncher.component;
using MyQuckLauncher.data;
using MyLib.Util;

namespace MyQuckLauncher {
    /// <summary>
    /// ランチャーメイン
    /// </summary>
    public partial class LauncherMain : Window {

        #region Declaration
        private AppRepository _settings;
        private ItemRepository _items;
        #endregion

        #region Constructor
        public LauncherMain() {
            InitializeComponent();
            this.SetupScreen();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 画面生成
        /// </summary>
        private void SetupScreen() {
            // show title
            var fullname = typeof(App).Assembly.Location;
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullname);
            this.Title = $"MyQuickLauncher({info.FileVersion})";

            // set window position
            this._settings = AppRepository.Init(MyLibUtil.GetAppPath() + @"\app.settings");
            if (0 <= this._settings.Pos.X && (this._settings.Pos.X + this.Width) < SystemParameters.VirtualScreenWidth) {
                this.Left = this._settings.Pos.X;
            }
            if (0 <= this._settings.Pos.Y && (this._settings.Pos.Y + this.Height) < SystemParameters.VirtualScreenHeight) {
                this.Top = this._settings.Pos.Y;
            }


            // setup grid items
            this._items  = _items = ItemRepository.Init(MyLibUtil.GetAppPath() + @"\app.data");
            for (int row = 0; row < this.cContainer.RowDefinitions.Count; row++) {
                for (int col = 0; col < this.cContainer.ColumnDefinitions.Count; col++) {
                    var item = new ItemView();
                    item.VerticalAlignment = VerticalAlignment.Bottom;
                    Grid.SetRow(item, row);
                    Grid.SetColumn(item, col);
                    this.cContainer.Children.Add(item);
                }
            }
        }
        #endregion
    }
}
