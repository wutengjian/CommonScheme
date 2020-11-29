using CommonScheme.ConfigCore.Models;
using CommonScheme.NetCore;
using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages.SqlServers
{
    public class DBConfigDal : IDBConfigDal
    {
        private static string _connStr = AppSettings.GetAppSeting("ConnectionStrings:CommonSchemeSqlServer");
        public int AddConfig(ConfigModel model)
        {
            int ID = 0;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                ID = conn.Insert<ConfigModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return ID;
        }

        public bool EditConfig(ConfigModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Update<ConfigModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public bool DeleteConfig(ConfigModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Delete<ConfigModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public ConfigModel GetConfig(ConfigModel model)
        {
            ConfigModel result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                if (model.ID > 0)
                    result = conn.Get<ConfigModel>(model.ID, commandTimeout: 60);
                else if (string.IsNullOrEmpty(model.Code) == false)
                {
                    string query = string.Format(@"select ID,ParentID,Code,Explain,Data,DataStatus,Remake from ConfigCore_Config with(nolock) where Code='{0}' ", model.Code);
                    if (model.ParentID >= 0)
                        query += " and ParentID=" + model.ParentID;
                    result = conn.QueryFirstOrDefault<ConfigModel>(query, commandTimeout: 60);
                }
                conn.Close();
            }
            return result;
        }

        public List<ConfigModel> GetConfigs(List<ConfigModel> models)
        {
            List<ConfigModel> result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.GetList<ConfigModel>(models.Select(x => x.ID).ToArray(), commandTimeout: 60).ToList();
                conn.Close();
            }
            return result;
        }
    }
}
