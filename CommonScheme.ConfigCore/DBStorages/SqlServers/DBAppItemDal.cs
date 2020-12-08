using CommonScheme.ConfigCore.Models;
using CommonScheme.NetCore;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages.SqlServers
{
    public class DBAppItemDal : IDBAppItemDal
    {
        private static string _connStr = AppSettings.GetAppSeting("ConnectionStrings:CommonSchemeSqlServer");
        public int AddAppItem(AppItemModel model)
        {
            int ID = 0;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                ID = conn.Insert<AppItemModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return ID;
        }

        public bool DeleteAppItem(AppItemModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result= conn.Delete<AppItemModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public bool EditAppItem(AppItemModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Update<AppItemModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public AppItemModel GetAppItem(AppItemModel model)
        {
            AppItemModel result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Get<AppItemModel>(model.ID, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public List<AppItemModel> GetAppItems(List<AppItemModel> models)
        {
            List<AppItemModel> result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.GetList<AppItemModel>(models.Select(x => x.ID).ToArray(), commandTimeout: 60).ToList();
                conn.Close();
            }
            return result;
        }
        public List<ClientAppItemModel> GetClientAppItems(int clientID)
        {
            List<ClientAppItemModel> result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.GetList<ClientAppItemModel>(new List<int>(clientID).ToArray(), commandTimeout: 60).ToList();
                conn.Close();
            }
            return result;
        }
        public int AddClientAppItem(ClientAppItemModel model)
        {
            int ID = 0;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                ID = conn.Insert<ClientAppItemModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return ID;
        }
        public bool EditClientAppItem(ClientAppItemModel model)
        {
            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    conn.Update<ClientAppItemModel>(model, commandTimeout: 60);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool DeleteClientAppItem(ClientAppItemModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Delete<ClientModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }
    }
}
