using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages
{
    public interface IDBAppItemDal
    {
        public int AddAppItem(AppItemModel model);
        public bool EditAppItem(AppItemModel model);
        public bool DeleteAppItem(AppItemModel model);
        public AppItemModel GetAppItem(AppItemModel model);
        public List<AppItemModel> GetAppItems(List<AppItemModel> models);
        public List<ClientAppItemModel> GetClientAppItems(int id);
        public int AddClientAppItem(ClientAppItemModel model);
        public bool EditClientAppItem(ClientAppItemModel model);
        public bool DeleteClientAppItem(ClientAppItemModel model);
    }
}
