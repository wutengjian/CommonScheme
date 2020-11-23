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
    public class DBClientPushDal : IDBClientPushDal
    {
        private string _connStr = "";
        public int AddConfig(ClientPushModel model)
        {
            int ID = 0;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                ID = conn.Insert<ClientPushModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return ID;
        }

        public bool EditConfig(ClientPushModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Update<ClientPushModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public bool DeleteConfig(ClientPushModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Delete<ClientPushModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public ClientPushModel GetConfig(ClientPushModel model)
        {
            ClientPushModel result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Get<ClientPushModel>(model.ClientID, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }

        public List<ClientPushModel> GetConfigs(List<ClientPushModel> models)
        {
            List<ClientPushModel> result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.GetList<ClientPushModel>(models.Select(x => x.ClientID).ToArray(), commandTimeout: 60).ToList();
                conn.Close();
            }
            return result;
        }
    }
}
