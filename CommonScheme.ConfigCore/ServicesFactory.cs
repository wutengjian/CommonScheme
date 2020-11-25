using CommonScheme.ConfigCore.DBServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore
{
    public class ServicesFactory
    {
        public static void MapFactory(string oaType)
        {
            CacheServices.CacheFactory.SetCache(new CacheServices.RedisCache());
            ClientServices.ClientFactory.SetClient("HttpPushClient", new ClientServices.HttpPushClient());
            ClientServices.ClientFactory.SetClient("RabbitMQClient", new ClientServices.RabbitMQClient());
            ClientServices.ClientFactory.SetClient("WebSocketClient", new ClientServices.WebSocketClient());
            ClientServices.ClientMonitor.Initialization();
            DBServices.DBFactory.Factory();
            DBServices.DBFactory.MapModel<IDBConfigDal>("IDBConfigDal", new DBServices.SqlServers.DBConfigDal());
            DBServices.DBFactory.MapModel<IDBClientDal>("IDBClientDal", new DBServices.SqlServers.DBClientDal());
            DBServices.DBFactory.MapModel<IDBClientPushDal>("IDBClientPushDal", new DBServices.SqlServers.DBClientPushDal());

            OAServices.OAServiceBase oa = null;
            if (oaType == "DevelopmentOA")
                oa = new OAServices.DevelopmentOA();
            else if (oaType == "ProductionOA")
                oa = new OAServices.ProductionOA();
        }
    }
}
