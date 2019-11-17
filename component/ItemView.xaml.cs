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

namespace MyQuckLauncher.component {
    /// <summary>
    /// LauncherItemView.xaml の相互作用ロジック
    /// </summary>
    public partial class ItemView : UserControl {
        #region Declaration
        private Window _owner;
        private ItemModel _model;
        private Key _key;
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

            this.cKey.Text = this._key.ToString().Replace("NumPad","");
            this.ShowIcon(this._model.Icon);

            this.Drop += ItemView_Drop;
        }

        #endregion

        #region Event
        private void ItemView_Drop(object sender, DragEventArgs e) {
            throw new NotImplementedException();
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
        #endregion
    }
}
