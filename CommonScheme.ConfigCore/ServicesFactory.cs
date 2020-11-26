using CommonScheme.ConfigCore.DBStorages;
using CommonScheme.ConfigCore.OAServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore
{
    public class ServicesFactory
    {
        public static void MapFactory()
        {
            CacheServices.CacheFactory.SetCache(new CacheServices.RedisCache());
            ClientServices.ClientFactory.SetClient("HttpPushClient", new ClientServices.HttpPushClient());
            ClientServices.ClientFactory.SetClient("RabbitMQClient", new ClientServices.RabbitMQClient());
            ClientServices.ClientFactory.SetClient("WebSocketClient", new ClientServices.WebSocketClient());
            ClientServices.ClientMonitor.Initialization();
            DBFactory.Factory();
            DBFactory.MapModel<IDBConfigDal>("IDBConfigDal", new DBStorages.SqlServers.DBConfigDal());
            DBFactory.MapModel<IDBClientDal>("IDBClientDal", new DBStorages.SqlServers.DBClientDal());
            OAFactory.SetOA(new OAService());
        }
    }
}
