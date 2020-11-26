using CommonScheme.ConfigCore.Models;
using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages.SqlServers
{
    public class DBClientDal:IDBClientDal
    {
        private string _connStr = "";
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
    }
}
