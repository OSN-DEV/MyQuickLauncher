using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuckLauncher.data {
    /// <summary>
    /// data model
    /// </summary>
    public class ItemModel {
        /// <summary>
        /// ページ番号
        /// </summary>
        public int PageNo { set; get; }

        /// <summary>
        /// インデックス
        /// </summary>
        public int Index { set; get; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FileUrl { set; get; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { set; get; }

        /// <summary>
        /// アイコン
        /// </summary>
        public string Icon { set; get; }
    }
}
