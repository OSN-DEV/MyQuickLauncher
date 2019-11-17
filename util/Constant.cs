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

    }
}
