using System;
using System.Collections.Generic;
using System.Text;
using CommonScheme.ConfigCore;
using CommonScheme.ConfigCore.DBServices;

namespace CommonScheme.ConfigCore
{
    public class Startup
    {
        public void Configration()
        {
            string oaType = "DevelopmentOA";
            MapModel(oaType);
        }
        public void MapModel(string oaType)
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
