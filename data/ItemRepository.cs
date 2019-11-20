﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Util;
using MyLib.Data;
using MyQuckLauncher.Util;

namespace MyQuckLauncher.Data {
    public class ItemRepository : AppDataBase<ItemRepository> {
        #region Declaration   

        public ItemModel[] ItemList { set; get; } = new ItemModel[16];

        private static string _settingFile;
        #endregion

        #region Public Method
        public static ItemRepository Init(string file) {
            _settingFile = file;
            GetInstanceBase(file);
            if (!System.IO.File.Exists(file)) {
                for(int i=0; i< _instance.ItemList.Length; i++) {
                    _instance.ItemList[i] = new ItemModel();
                    _instance.ItemList[i].Icon = Constant.NoItemIcon;
                }
                _instance.Save();
            }
            return _instance;
        }

        /// <summary>
        /// get instance
        /// </summary>
        /// <returns></returns>
        public static ItemRepository GetInstance() {
            return GetInstanceBase();
        }

        /// <summary>
        /// save settings
        /// </summary>
        public void Save() {
            GetInstanceBase().SaveToXml(_settingFile);
        }

        /// <summary>
        /// set model
        /// </summary>
        /// <param name="model"></param>
        public void SetItem(ItemModel model) {
            this.ItemList[model.Index] = model;
        }
        #endregion
    }
}
