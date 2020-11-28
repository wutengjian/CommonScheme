using CommonScheme.ConfigCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigAPI
{
    public class ServicesFactory
    {
        public static void MapFactory()
        {
            ConfigCore.CacheServices.CacheFactory.SetCache(new ConfigCore.CacheServices.RedisCache());
            ConfigCore.ClientServices.ClientFactory.SetClient("HttpPushClient", new ConfigCore.ClientServices.HttpPushClient());
            ConfigCore.ClientServices.ClientFactory.SetClient("RabbitMQPushClient", new ConfigCore.ClientServices.RabbitMQPushClient());
            ConfigCore.ClientServices.ClientMonitor.Initialization();
            ConfigCore.DBStorages.DBFactory.Factory();
            ConfigCore.DBStorages.DBFactory.MapModel<ConfigCore.DBStorages.IDBConfigDal>("IDBConfigDal", new ConfigCore.DBStorages.SqlServers.DBConfigDal());
            ConfigCore.DBStorages.DBFactory.MapModel<ConfigCore.DBStorages.IDBClientDal>("IDBClientDal", new ConfigCore.DBStorages.SqlServers.DBClientDal());
            ConfigCore.OAServices.OAFactory.SetOA(new ConfigCore.OAServices.OAService());
        }
    }
}
