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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyQuckLauncher.data;
using MyLib.File;
using System.IO;
using System.Drawing.Imaging;
using MyQuckLauncher.Util;
using Microsoft.Win32;

namespace MyQuckLauncher.component {
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

            this.cKey.Text = key.ToString().Replace("NumPad","");
            this.ShowIcon(model.Icon);
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
            this.CreateAppIcon(model.FileUrl, model.Icon);
            this.ItemAdded?.Invoke(this, new ItemEventArgs(model));
        }

        /// <summary>
        /// Context Menu [Edit] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemEdit_Click(object sender, RoutedEventArgs e) {

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
                this.CreateDirectoryIcon(model.FileUrl, model.Icon);
            } else {
                this.CreateAppIcon(model.FileUrl, model.Icon);
            }
            this.ItemAdded?.Invoke(this, new ItemEventArgs(model));
        }

        /// <summary>
        /// Image Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cIcon_MouseDown(object sender, MouseButtonEventArgs e) {
            this.ItemClick?.Invoke(this, new ItemEventArgs(this._model));
        }
        #endregion

        #region Private Method
        /// <summary>
        /// show icon
        /// </summary>
        /// <param name="icon"></param>
        private void ShowIcon(string icon) {
            // https://fkmt5.hatenadiary.org/entry/20130729/1375090831
            var bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.CacheOption = BitmapCacheOption.OnLoad; 
            bmpImage.CreateOptions = BitmapCreateOptions.None; 
            bmpImage.UriSource = new Uri(icon);
            bmpImage.EndInit();
            bmpImage.Freeze();
            this.cIcon.Source = bmpImage;
        }

        /// <summary>
        /// create app icon
        /// </summary>
        /// <param name="inputFile">input file name</param>
        /// <param name="iconFile">icon file name</param>
        private void CreateAppIcon(string inputFile, string iconFile) {
            using (var icon = System.Drawing.Icon.ExtractAssociatedIcon(inputFile)){
                icon.ToBitmap().Save(iconFile, ImageFormat.Png);
            }
        }

        /// <summary>
        /// create directory icon
        /// </summary>
        /// <param name="inputFile">input file name</param>
        /// <param name="iconFile">icon file name</param>
        private void CreateDirectoryIcon(string inputFile, string iconFile) {
            var shinfo = new NativeMethod.SHFILEINFO();
            IntPtr hImg = NativeMethod.SHGetFileInfo(
              inputFile, 0, out shinfo, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(NativeMethod.SHFILEINFO)),
              (uint)(NativeMethod.SHGFI.SHGFI_ICON | NativeMethod.SHGFI.SHGFI_LARGEICON));
            if (IntPtr.Zero != hImg) {
                using (var icon = System.Drawing.Icon.FromHandle(shinfo.hIcon)) { 
                    icon.ToBitmap().Save(iconFile, ImageFormat.Png);
                }
            }
        }
        #endregion

    }
}
