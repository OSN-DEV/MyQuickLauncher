using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib;
using MyLib.Util;

namespace MyQuckLauncher.Util {
    internal static class Constant {
        /// <summary>
        /// アプリの設定関連情報
        /// </summary>
        public static readonly string SettingFile =  MyLibUtil.GetAppPath() + @"app.settings";

        /// <summary>
        /// アイテム情報
        /// </summary>
        public static readonly string AppDataFile = MyLibUtil.GetAppPath() + @"app.data";

        /// <summary>
        /// アイコンのキャッシュフォルダ
        /// </summary>
        public static readonly string IconCache = MyLibUtil.GetAppPath() + @"icon\";

        /// <summary>
        /// アイコン画像なし
        /// </summary>
        public static readonly string NoItemIcon =  MyLibUtil.GetAppPath() + @"res\no item.png";

        /// <summary>
        /// テンポラリアイコンの拡張子
        /// </summary>
        public static readonly string TmpIconExt = ".png.tmp";

        /// <summary>
        /// アイコンの拡張子
        /// </summary>
        public static readonly string IconExt = ".png";

        /// <summary>
        /// ページ数
        /// </summary>
        public static readonly int PageCount = 4;

        /// <summary>
        /// ページあたりのアイテム数
        /// </summary>
        public static readonly int ItemCount = 16;

    }
}
