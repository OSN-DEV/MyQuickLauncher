using Microsoft.Win32;
using MyLib.File;
using MyQuckLauncher.Data;
using MyQuckLauncher.Util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyQuckLauncher {
    /// <summary>
    /// EditItem.xaml の相互作用ロジック
    /// </summary>
    public partial class EditItem : Window {
        #region Declaration
        #endregion

        #region Public Property
        public ItemModel Model { private set; get; }
        #endregion

        #region Constructor
        public EditItem() {
            InitializeComponent();
        }

        public EditItem(Window owner, ItemModel model) {
            InitializeComponent();

            this.Owner = owner;
            this.Model = model.Clone();
            this.Initialize();
        }
        #endregion

        #region Event
        /// <summary>
        /// CancelButton Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_Click(object sender, RoutedEventArgs e) {
            this.Model.DisplayName = this.cDisplayName.Text;
            this.Model.FileUrl = this.cFileUrl.Text;

            if (!System.IO.File.Exists(this.Model.Icon)) {
                this.Model.Icon = Constant.NoItemIcon;
            }
            this.DialogResult = true;
        }

        /// <summary>
        /// Context Menu [Edit] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemEdit_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog() {
                Filter = "icon image|*.png",
                FilterIndex = 0,
                Title = "ファイルを選択"
            };
            if (true != dialog.ShowDialog()) {
                return;
            }

            this.Model.Icon = $"{Constant.IconCache}{this.Model.PageNo}_{this.Model.Index}.png.tmp";
            System.IO.File.Copy(dialog.FileName, this.Model.Icon, true);
            this.cIcon.SetImageFromFile(this.Model.Icon);


        }

        /// <summary>
        /// Context Menu [Remove] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemRemove_Click(object sender, RoutedEventArgs e) {
            this.SetIcon();
            this.cIcon.Source = AppUtil.CreateImgeFromIconFile(this.Model.Icon);
        }

        /// <summary>
        /// change file url. update icon file if need.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileUrl_TextChanged(object sender, TextChangedEventArgs e) {
            this.cOK.IsEnabled = (0 < this.cFileUrl.Text.Length);
            if (0 < this.cFileUrl.Text.Length && this.cFileUrl.Text != this.cIcon.Tag.ToString()) {
                var fileUtil = FileUtil.Create(this.cFileUrl.Text);
                if (null != fileUtil && fileUtil.Exists()) {
                    this.SetIcon();
                    this.cIcon.SetImageFromFile(this.Model.Icon);
                    this.cIcon.Tag = this.cFileUrl.Text;
                }
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize screen
        /// </summary>
        private void Initialize() {
            this.cIcon.SetImageFromFile(this.Model.Icon);
            this.cIcon.Tag = this.Model.FileUrl;
            this.cDisplayName.Text = this.Model.DisplayName;
            this.cFileUrl.Text = this.Model.FileUrl;
            this.cOK.IsEnabled = (0 < this.cFileUrl.Text.Length);
        }

        /// <summary>
        /// show icon
        /// </summary>
        private void SetIcon() {
            var fileUtil = FileUtil.Create(this.cFileUrl.Text);
            if (null == fileUtil) {
                this.Model.Icon = Constant.NoItemIcon;
            } else {
                if (fileUtil.Exists()) {
                    this.Model.Icon = $"{Constant.IconCache}{this.Model.PageNo}_{this.Model.Index}{Constant.TmpIconExt}";
                    if (fileUtil.IsDirectory) {
                        AppUtil.CreateDirectoryIcon(this.cFileUrl.Text, this.Model.Icon);
                    } else {
                        AppUtil.CreateAppIcon(this.cFileUrl.Text, this.Model.Icon);
                    }
                } else {
                    this.Model.Icon = Constant.NoItemIcon;
                }
            }
        }
        #endregion
    }
}
