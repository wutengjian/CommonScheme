using CommonScheme.ConfigCore.Models;
using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CommonScheme.ConfigCore.DBServices.SqlServers
{
    public class DBConfigDal:IDBConfigDal
    {
        private string _connStr = "";
        public  int AddConfig(ConfigModel model)
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

        public  bool EditConfig(ConfigModel model)
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

        public  bool DeleteConfig(ConfigModel model)
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

        public  ConfigModel GetConfig(ConfigModel model)
        {
            ConfigModel result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Get<ConfigModel>(model.ID, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public  List<ConfigModel> GetConfigs(List<ConfigModel> models)
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
