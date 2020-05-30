using MyLib.File;
using MyLib.Util;
using MyQuckLauncher.Component;
using MyQuckLauncher.Data;
using MyQuckLauncher.Util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyQuckLauncher {
    /// <summary>
    /// ランチャーメイン
    /// </summary>
    public partial class LauncherMain : Window {

        #region Declaration
        private readonly HotKeyHelper _hotkey;
        private AppRepository _settings;
        private ItemRepository _items;
        private Key[] _keybinding = { Key.D1, Key.D2, Key.D3, Key.D4,
                                      Key.Q,Key.W,Key.E,Key.R,
                                      Key.A,Key.S,Key.D,Key.F,
                                      Key.Z,Key.X,Key.C,Key.V};
        private ItemView[] _itemViews;
        private readonly System.Windows.Forms.NotifyIcon _notifyIcon = new System.Windows.Forms.NotifyIcon();
        private string _appTitle = "";
        #endregion

        #region Constructor
        public LauncherMain() {
            InitializeComponent();

            this._hotkey = new HotKeyHelper(this);
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
            //change page
            if (Key.D1 <= e.Key && e.Key <= Key.D4 && this.IsModifierPressed(ModifierKeys.Shift)) {
                e.Handled = true;
                int page = (int)e.Key - 35;
                if (this._settings.Page != page) {
                    this._settings.Page = page;
                    this._settings.Save();
                    this.ShowCurrentPage();
                }
                return;
            }
            if(Key.Left == e.Key || Key.Right == e.Key) {
                e.Handled = true;
                if (Key.Left == e.Key) {
                    this._settings.Page--;
                    if (this._settings.Page < 0) {
                        this._settings.Page = Constant.PageCount - 1;
                    }
                } else {
                    this._settings.Page++;
                    if (Constant.PageCount <= this._settings.Page) {
                        this._settings.Page = 0;
                    }
                }
                this._settings.Save();
                this.ShowCurrentPage();
                return;
            }

            // launch or minimize
            var index = Array.IndexOf(this._keybinding, e.Key);
            if (-1 < index) {
                e.Handled = true;
                var model = GetModel(index);
                if (0 < model.FileUrl.Length && this.LaunchApp(model)) {
                    this.SetWindowsState(true);
                }
                this.Activate();
            } else if (e.Key == Key.Escape) {
                e.Handled = true;
                this.SetWindowsState(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LauncherMain_Loaded(object sender, RoutedEventArgs e) {
            this.SetWindowsState(true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LauncherMain_Closed(object sender, EventArgs e) {
            this._hotkey.Dispose();
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
            if (0 == e.Model.FileUrl.Length) {
                return;
            }
            if (this.LaunchApp(e.Model)) {
                this.SetWindowsState(true);
            }
        }

        /// <summary>
        /// item removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemRemoved(object sender, ItemView.ItemEventArgs e) {
            new FileOperator(e.Model.Icon).Delete();
            var model = this.GetModel(e.Model.Index);
            model.Clear();
            model.Icon = Constant.NoItemIcon;
            this._items.Save();
            this._itemViews[e.Model.Index].UpdateModel(model);
        }

        /// <summary>
        /// item dir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemDir(object sender, ItemView.ItemEventArgs e) {
            var model = this.GetModel(e.Model.Index);
            var file = FileUtil.Create(model.FileUrl);
            if (null == file) {
                return;
            }
            var fileDir = "";
            if (file.IsDirectory) {
                fileDir = file.FilePath;
            } else {
                fileDir = ((FileOperator)file).GetParendDir().FilePath;
            }
            MyLibUtil.RunApplication(fileDir, false);
        }

        /// <summary>
        /// item updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemUpdated(object sender, ItemView.ItemEventArgs e) {
            this.ItemAdded(sender, e);
        }

        /// <summary>
        /// item added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemAdded(object sender, ItemView.ItemEventArgs e) {
            this.RenameTmpIcon(e.Model);
            this._items.SetItem(this._settings.Page, e.Model);
            this._items.Save();
            this._itemViews[e.Model.Index].UpdateModel(e.Model);
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
            foreach (var child in dirUtil.Children) {
                ((FileOperator)child).Delete();
            }

            // set window position
            this._settings = AppRepository.Init(Constant.SettingFile);
            if (0 <= this._settings.Pos.X && (this._settings.Pos.X + this.Width) < SystemParameters.VirtualScreenWidth) {
                this.Left = this._settings.Pos.X;
            }
            if (0 <= this._settings.Pos.Y && (this._settings.Pos.Y + this.Height) < SystemParameters.VirtualScreenHeight) {
                this.Top = this._settings.Pos.Y;
            }

            // show title
            var fullname = typeof(App).Assembly.Location;
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullname);
            this._appTitle = $"MyQuickLauncher({info.ProductMajorPart}.{info.ProductMinorPart}.{info.ProductPrivatePart})";
            this.UpdateTitle();

            // setup grid items
            this._items = _items = ItemRepository.Init(Constant.AppDataFile);
            var index = 0;
            this._itemViews = new ItemView[Constant.ItemCount];
            for (int row = 0; row < this.cContainer.RowDefinitions.Count; row++) {
                for (int col = 0; col < this.cContainer.ColumnDefinitions.Count; col++) {
                    var model = this._items.ItemList[this._settings.Page][index];
                    //model.PageNo = 1;
                    //model.Index = index;
                    if (!System.IO.File.Exists(model.Icon)) {
                        model.Icon = Constant.NoItemIcon;
                    }
                    var item = new ItemView(this, model, this._keybinding[index]);
                    this._itemViews[index] = item;
                    index++;
                    item.VerticalAlignment = VerticalAlignment.Bottom;
                    Grid.SetRow(item, row);
                    Grid.SetColumn(item, col);
                    item.ItemClick += ItemClick;
                    item.ItemAdded += ItemAdded;
                    item.ItemUpdated += ItemUpdated;
                    item.ItemRemoved += ItemRemoved;
                    item.ItemDir += ItemDir;
                    item.VerticalAlignment = VerticalAlignment.Top;
                    this.cContainer.Children.Add(item);

                }
            }

            // register hot key
            this._hotkey.Register(ModifierKeys.None, Key.ImeNonConvert, (_, __) => {
                if (!this.ShowInTaskbar) {
                    NotifyMenuShow_Click(null, null);
                } else {
                    if (this.WindowState == WindowState.Minimized) {
                        this.WindowState = WindowState.Normal;
                    }
                    this.Activate();
                }
            }
                                  );
        }

        /// <summary>
        /// set up notify icon
        /// </summary>
        private void SetUpNotifyIcon() {
            this._notifyIcon.Text = "My Quick Launcher";
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

        /// <summary>
        /// launch app
        /// </summary>
        /// <param name="model"></param>
        private bool LaunchApp(ItemModel model) {
            var result = false;
            if (0 <= model.FileUrl.Length) {
                if (!MyLibUtil.RunApplication(model.FileUrl, false)) {
                    AppUtil.ShowErrorMsg(string.Format(ErrorMsg.FailToLaunch, model.FileUrl));
                } else {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// rename tmp icon
        /// </summary>
        /// <param name="model"></param>
        private void RenameTmpIcon(ItemModel model) {
            if (model.Icon == Constant.NoItemIcon) {
                return;
            }
            if (!model.Icon.EndsWith(Constant.TmpIconExt)) {
                return;
            }
            var file = new FileOperator(model.Icon.Replace(Constant.TmpIconExt, Constant.IconExt));
            file.Delete();
            System.IO.File.Move(model.Icon, file.FilePath);
            model.Icon = file.FilePath;
        }

        /// <summary>
        /// 現在のページのアイテムを取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private ItemModel GetModel(int index) {
            return this._items.ItemList[this._settings.Page][index];
        }

        /// <summary>
        /// タイトルを更新する
        /// </summary>
        private void UpdateTitle() {
            this.Title = $"{this._appTitle} - ({this._settings.Page + 1}/{Constant.PageCount})";
        }

        /// <summary>
        /// check if modifiered key is pressed
        /// </summary>
        /// <param name="key">modifier key</param>
        /// <returns>true:modifiered key is pressed, false:otherwise</returns>
        private bool IsModifierPressed(ModifierKeys key) {
            return (Keyboard.Modifiers & key) != ModifierKeys.None;
        }

        /// <summary>
        /// show current page's items
        /// </summary>
        private void ShowCurrentPage() {
            for (int i = 0; i < Constant.ItemCount; i++) {
                this._itemViews[i].UpdateModel(this.GetModel(i));
            }
            this.UpdateTitle();
        }
        #endregion
    }
}
