using Microsoft.Win32;
using MyLib.File;
using MyQuckLauncher.Data;
using MyQuckLauncher.Util;
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
            var fileUtil = FileUtil.Create(dialog.FileName);
            this.Model.FileUrl = fileUtil.FilePath;
            this.Model.DisplayName = fileUtil.Name;
            this.Model.Icon = $"{Constant.IconCache}{this.Model.PageNo}_{this.Model.Index}.png.tmp";
            AppUtil.CreateAppIcon(this.Model.FileUrl, this.Model.Icon);
            this.cIcon.Source = AppUtil.CreateImgeFromIconFile(this.Model.Icon);
        }

        /// <summary>
        /// Context Menu [Remoive] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemRemove_Click(object sender, RoutedEventArgs e) {
            var fileUtil = FileUtil.Create(this.cFileUrl.Text);
            if (fileUtil.Exists()) {
                this.Model.Icon = $"{Constant.IconCache}{this.Model.PageNo}_{this.Model.Index}.png.tmp";
                if (fileUtil.IsDirectory) {
                    AppUtil.CreateDirectoryIcon(this.cFileUrl.Text, this.Model.Icon);
                } else {
                    AppUtil.CreateAppIcon(this.cFileUrl.Text, this.Model.Icon);
                }
            } else {
                this.Model.Icon = Constant.NoItemIcon;
            }
            this.cIcon.Source = AppUtil.CreateImgeFromIconFile(this.Model.Icon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cFileUrl_TextChanged(object sender, TextChangedEventArgs e) {
            this.cOK.IsEnabled = (0 < this.cFileUrl.Text.Length);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 初期処理
        /// </summary>
        private void Initialize() {
            this.cIcon.Source = AppUtil.CreateImgeFromIconFile(this.Model.Icon);
            this.cDisplayName.Text = this.Model.DisplayName;
            this.cFileUrl.Text = this.Model.FileUrl;
        }
        #endregion
    }
}
