﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Util;
using MyLib.Data;

namespace MyQuckLauncher.Data {
     public class AppRepository : AppDataBase<AppRepository> {
        #region Declaration   
        public class Location {
            public double X { set; get; } = -1;
            public double Y { set; get; } = -1;
        }
        public Location Pos { set; get; } = new Location();
        public int Page { set; get; } = 0;
        private static string _settingFile;
        #endregion

        #region Public Method
        public static AppRepository Init(string file) {
            _settingFile = file;
            GetInstanceBase(file);
            if (!System.IO.File.Exists(file)) {
                _instance.Save();
            }
            return _instance;
        }

        /// <summary>
        /// get instance
        /// </summary>
        /// <returns></returns>
        public static AppRepository GetInstance() {
            return GetInstanceBase();
        }

        /// <summary>
        /// save settings
        /// </summary>
        public void Save() {
            GetInstanceBase().SaveToXml(_settingFile);
        }
        #endregion
    }
}
