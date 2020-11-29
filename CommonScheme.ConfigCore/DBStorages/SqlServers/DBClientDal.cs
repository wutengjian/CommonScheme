using CommonScheme.ConfigCore.Models;
using CommonScheme.NetCore;
using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages.SqlServers
{
    public class DBClientDal : IDBClientDal
    {
        private static string _connStr = AppSettings.GetAppSeting("ConnectionStrings:CommonSchemeSqlServer");
        public int AddClient(ClientModel model)
        {
            int ID = 0;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                ID = conn.Insert<ClientModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return ID;
        }
        public bool EditClient(ClientModel model)
        {
            bool result = false;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Update<ClientModel>(model, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }
        public bool DeleteClient(ClientModel model)
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
        public ClientModel GetClient(ClientModel model)
        {
            ClientModel result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Get<ClientModel>(model.ID, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }
        public List<ClientModel> GetClients(List<ClientModel> models)
        {
            List<ClientModel> result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.GetList<ClientModel>(models.Select(x => x.ID).ToArray(), commandTimeout: 60).ToList();
                conn.Close();
            }
            return result;
        }
        public ClientOptionModel GetClientOption(int id)
        {
            ClientOptionModel result = null;
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                result = conn.Get<ClientOptionModel>(id, commandTimeout: 60);
                conn.Close();
            }
            return result;
        }
        public bool AddClientOption(ClientOptionModel model)
        {
            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    conn.Insert<ClientOptionModel>(model, commandTimeout: 60);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool EditClientOption(ClientOptionModel model)
        {
            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    conn.Update<ClientOptionModel>(model, commandTimeout: 60);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
