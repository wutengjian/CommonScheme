using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using System.Data;
using Dapper.Contrib.Extensions;

namespace CommonScheme.IdentityAPI
{
    public class ClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            //  return GetClient(clientId);
            #region 用户名密码
            var memoryClients = ID4Config.GetClients();
            if (memoryClients.Any(oo => oo.ClientId == clientId))
            {
                return memoryClients.FirstOrDefault(oo => oo.ClientId == clientId);
            }
            #endregion

            #region 通过数据库查询Client 信息
            return GetClient(clientId);
            #endregion
        }
        private Client GetClient(string clientId)
        {
            using (IDbConnection conn = DBUserCenter.SqlConnection())
            {
                return conn.GetAll<Client>().Where(x => x.ClientId == clientId).FirstOrDefault();
            }
        }
    }
}
