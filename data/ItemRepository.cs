using MyLib.Data;
using MyQuckLauncher.Util;
using System.Collections.Generic;

namespace MyQuckLauncher.Data {
    public class ItemRepository : AppDataBase<ItemRepository> {

        #region Declaration   
        public List<List<ItemModel>> ItemList { set; get; } = new List<List<ItemModel>>();

        private static string _settingFile;
        #endregion

        #region Public Method
        public static ItemRepository Init(string file) {
            _settingFile = file;
            GetInstanceBase(file);
            if (!System.IO.File.Exists(file) || 0 == GetInstanceBase().ItemList.Count) {
                for (int page = 0; page < Constant.PageCount; page++) {
                    _instance.ItemList.Add( new List<ItemModel>());
                    for (int index = 0; index < Constant.ItemCount; index++) {
                        _instance.ItemList[page].Add( new ItemModel());
                        _instance.ItemList[page][index].PageNo = page;
                        _instance.ItemList[page][index].Index = index;
                        _instance.ItemList[page][index].Icon = Constant.NoItemIcon;
                    }
                }
                _instance.Save();
            } else if (_instance.ItemList[0].Count < Constant.ItemCount) {
                int start = _instance.ItemList[0].Count;
                for (int page = 0; page < Constant.PageCount; page++) {
                    for (int index = start; index < Constant.ItemCount; index++) {
                        _instance.ItemList[page].Add(new ItemModel());
                        _instance.ItemList[page][index].PageNo = page;
                        _instance.ItemList[page][index].Index = index;
                        _instance.ItemList[page][index].Icon = Constant.NoItemIcon;
                    }
                }
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
        public void SetItem(int page, ItemModel model) {
            this.ItemList[page][model.Index] = model;
        }
        #endregion
    }
}
