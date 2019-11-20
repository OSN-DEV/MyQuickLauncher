using Microsoft.Win32;
using MyLib.File;
using MyQuckLauncher.Data;
using MyQuckLauncher.Util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyQuckLauncher.Component {
    /// <summary>
    /// LauncherItemView.xaml の相互作用ロジック
    /// </summary>
    public partial class ItemView : UserControl {
        #region Declaration
        private Window _owner;
        private ItemModel _model;
        private Key _key;


        public class ItemEventArgs : EventArgs {
            public ItemModel Model { private set; get; }
            public ItemEventArgs(ItemModel model) {
                this.Model = model;
            }
        }
        public delegate void ItemViewEventHandler(object sender, ItemEventArgs e);
        public event ItemViewEventHandler ItemClick;
        public event ItemViewEventHandler ItemAdded;
        public event ItemViewEventHandler ItemUpdated;
        public event ItemViewEventHandler ItemRemoved;
        #endregion

        #region Constructor
        public ItemView() {
            InitializeComponent();
        }

        public ItemView(Window owner, ItemModel model, Key key) {
            InitializeComponent();

            this._owner = owner;
            this._model = model;
            this._key = key;

            this.cKey.Text = key.ToString();
            if (2 == this.cKey.Text.Length) {
                this.cKey.Text = this.cKey.Text.Substring(1, 1);
            }
            this.cIcon.SetImageFromFile(model.Icon);
            this.cMenuRemove.IsEnabled = (0 < model.FileUrl?.Length);
            this.Drop += ItemView_Drop;
        }
        #endregion

        #region Event
        /// <summary>
        /// Context Menu [Add] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAdd_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog() {
                Filter = "all|*.*",
                FilterIndex = 0,
                Title = "ファイルを選択"
            };
            if (true != dialog.ShowDialog()) {
                return;
            }
            var model = this._model.Clone();
            var fileUtil = FileUtil.Create(dialog.FileName);
            model.FileUrl = fileUtil.FilePath;
            model.DisplayName = fileUtil.Name;
            model.Icon = $"{Constant.IconCache}{this._model.PageNo}_{this._model.Index}.png.tmp";
            AppUtil.CreateAppIcon(model.FileUrl, model.Icon);
            this.ItemAdded?.Invoke(this, new ItemEventArgs(model));
        }

        /// <summary>
        /// Context Menu [Edit] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemEdit_Click(object sender, RoutedEventArgs e) {
            var dialog = new EditItem(this._owner, this._model);
            if (true != dialog.ShowDialog()) {
                return;
            }
            this.ItemUpdated?.Invoke(this, new ItemEventArgs( dialog.Model));
        }

        /// <summary>
        /// Context Menu [Remoive] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemRemove_Click(object sender, RoutedEventArgs e) {
            this.ItemRemoved?.Invoke(this, new ItemEventArgs(this._model));
        }

        /// <summary>
        /// Item Drop event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemView_Drop(object sender, DragEventArgs e) {
            if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files)) {
                return;
            }
            var model = this._model.Clone();
            var fileUtil = FileUtil.Create(files[0]);
            model.FileUrl = fileUtil.FilePath;
            model.DisplayName = fileUtil.Name;
            model.Icon = $"{Constant.IconCache}{this._model.PageNo}_{this._model.Index}.png.tmp";

            if (fileUtil.IsDirectory) {
                AppUtil.CreateDirectoryIcon(model.FileUrl, model.Icon);
            } else {
                AppUtil.CreateAppIcon(model.FileUrl, model.Icon);
            }
            this.ItemAdded?.Invoke(this, new ItemEventArgs(model));
        }

        /// <summary>
        /// Image Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Icon_MouseDown(object sender, MouseButtonEventArgs e) {
            this.ItemClick?.Invoke(this, new ItemEventArgs(this._model));
        }
        #endregion

        #region Public Method
        /// <summary>
        /// update model
        /// </summary>
        /// <param name="model"></param>
        public void UpdateModel(ItemModel model) {
            this._model = model;
            this.cIcon.SetImageFromFile(model.Icon);
            this.cMenuRemove.IsEnabled = (0 < model.FileUrl?.Length);
        }

        #endregion

    }
}
