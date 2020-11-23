using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Host
{
    public class DBUserCenter
    {
        public static MySqlConnection SqlConnection()
        {
            string connStr = ServiceLocator.Configuration.GetConnectionString("UserCenterConnection");
            return SqlConnection(connStr);
        }
        public static MySqlConnection SqlConnection(string connStr)
        {
            var connection = new MySqlConnection(connStr);
            connection.Open();
            return connection;
        }
    }
}
