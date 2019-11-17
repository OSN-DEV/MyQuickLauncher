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
using MyQuckLauncher.Component;
using MyQuckLauncher.Data;
using MyLib.Util;
using MyQuckLauncher.Util;
using MyLib.File;

namespace MyQuckLauncher {
    /// <summary>
    /// ランチャーメイン
    /// </summary>
    public partial class LauncherMain : Window {

        #region Declaration
        private AppRepository _settings;
        private ItemRepository _items;
        private Key[] _keybinding = { Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4,
                                      Key.Q,Key.W,Key.E,Key.R,
                                      Key.A,Key.S,Key.D,Key.F,
                                      Key.Z,Key.X,Key.C,Key.V};
        private readonly System.Windows.Forms.NotifyIcon _notifyIcon = new System.Windows.Forms.NotifyIcon();
        #endregion

        #region Constructor
        public LauncherMain() {
            InitializeComponent();
            this.Initialize();
            this.SetUpNotifyIcon();
            this.KeyDown += LauncherMain_KeyDown;
        }
        #endregion


        #region Event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LauncherMain_KeyDown(object sender, KeyEventArgs e) {
            var index = Array.IndexOf(this._keybinding, e.Key);
            if (-1 < index) {
                e.Handled = true;
            } else if (e.Key == Key.Escape) {
                e.Handled = true;
                this.SetWindowsState(true);
            }
        }

        /// <summary>
        /// [show] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyMenuShow_Click(object sender, EventArgs e) {
            this.SetWindowsState(false);
            this.Activate();
        }

        /// <summary>
        /// [exit] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyMenuExit_Click(object sender, EventArgs e) {
            this._notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// item clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemClick(object sender, ItemView.ItemEventArgs e) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// item removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemRemoved(object sender, ItemView.ItemEventArgs e) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// item updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemUpdated(object sender, ItemView.ItemEventArgs e) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// item added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemAdded(object sender, ItemView.ItemEventArgs e) {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Method
        /// <summary>
        /// 初期処理
        /// </summary>
        private void Initialize() {
            // delete cache icon
            var dirUtil = new DirectoryOperator(Constant.IconCache);
            dirUtil.Create();
            dirUtil.ParseChildren(false, new List<string>() { "tmp" });
            foreach(var child in dirUtil.Children) {
                ((FileOperator)child).Delete(); 
            }

            // show title
            var fullname = typeof(App).Assembly.Location;
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullname);
            this.Title = $"MyQuickLauncher({info.FileVersion})";

            // set window position
            this._settings = AppRepository.Init(Constant.SettingFile);
            if (0 <= this._settings.Pos.X && (this._settings.Pos.X + this.Width) < SystemParameters.VirtualScreenWidth) {
                this.Left = this._settings.Pos.X;
            }
            if (0 <= this._settings.Pos.Y && (this._settings.Pos.Y + this.Height) < SystemParameters.VirtualScreenHeight) {
                this.Top = this._settings.Pos.Y;
            }

            // setup grid items
            this._items  = _items = ItemRepository.Init(Constant.AppDataFile);
            var index = 0;
            for (int row = 0; row < this.cContainer.RowDefinitions.Count; row++) {
                for (int col = 0; col < this.cContainer.ColumnDefinitions.Count; col++) {
                    var model = this._items.ItemList[index];
                    model.PageNo = 1;
                    model.Index = index;
                    var item = new ItemView(this, model, this._keybinding[index]);
                    index++;
                    item.VerticalAlignment = VerticalAlignment.Bottom;
                    Grid.SetRow(item, row);
                    Grid.SetColumn(item, col);
                    item.ItemClick += ItemClick;
                    item.ItemAdded += ItemAdded;
                    item.ItemUpdated += ItemUpdated;
                    item.ItemRemoved += ItemRemoved;
                    this.cContainer.Children.Add(item);
                }
            }
        }

        /// <summary>
        /// set up notify icon
        /// </summary>
        private void SetUpNotifyIcon() {
            this._notifyIcon.Text = "My Simple Launcher";
            this._notifyIcon.Icon = new System.Drawing.Icon("app.ico");
            var menu = new System.Windows.Forms.ContextMenuStrip();
            var menuItemShow = new System.Windows.Forms.ToolStripMenuItem {
                Text = "show"
            };
            menuItemShow.Click += this.NotifyMenuShow_Click;
            menu.Items.Add(menuItemShow);

            var menuItemExit = new System.Windows.Forms.ToolStripMenuItem {
                Text = "exit"
            };
            menuItemExit.Click += this.NotifyMenuExit_Click;
            menu.Items.Add(menuItemExit);

            this._notifyIcon.ContextMenuStrip = menu;
            this._notifyIcon.Visible = false;
        }

        /// <summary>
        /// set window state
        /// </summary>
        /// <param name="minimize">true:minize, false:normalize</param>
        private void SetWindowsState(bool minimize) {
            this.WindowState = minimize ? WindowState.Minimized : WindowState.Normal;
            this.ShowInTaskbar = !minimize;
            this._notifyIcon.Visible = minimize;
            if (minimize) {
                this._settings.Pos.X = this.Left;
                this._settings.Pos.Y = this.Top;
                this._settings.Save();
            } else { 
                this.Activate();
            }
        }
        #endregion
    }
}
