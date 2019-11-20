using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuckLauncher.Data {
    /// <summary>
    /// data model
    /// </summary>
    public class ItemModel {

        #region Property
        /// <summary>
        /// ページ番号
        /// </summary>
        public int PageNo { set; get; } = -1;

        /// <summary>
        /// インデックス
        /// </summary>
        public int Index { set; get; } = -1;

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FileUrl { set; get; } = "";

        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { set; get; } = "";

        /// <summary>
        /// アイコン
        /// </summary>
        public string Icon { set; get; } = "";
        #endregion

        #region Public Method
        /// <summary>
        /// copy instance
        /// </summary>
        /// <returns></returns>
        public ItemModel Clone() {
            var model = new ItemModel();
            model.PageNo = this.PageNo;
            model.Index = this.Index;
            model.FileUrl = this.FileUrl;
            model.DisplayName = this.DisplayName;
            model.Icon = this.Icon;
            return model;
        }

        /// <summary>
        /// clear model data
        /// </summary>
        public void Clear() {
            this.DisplayName = "";
            this.FileUrl = "";
            this.Icon = "";
        }
        #endregion
    }
}
