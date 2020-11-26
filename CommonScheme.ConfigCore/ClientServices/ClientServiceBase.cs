using CommonScheme.ConfigCore.CacheServices;
using CommonScheme.ConfigCore.DBStorages;
using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.ClientServices
{
    public abstract class ClientServiceBase : IClientService
    {
        public void Push(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }
        public virtual ConfigEntity GetEntity(ConfigEntity config)
        {
            string key = CacheFactory.MadePrefix(config.Code, config.ParentID);
            return CacheFactory.GetInstace().GetConfig(key);
        }
        public virtual ConfigModel GetModel(ConfigEntity config)
        {
            ConfigModel model = ConvertData.ConfigEntityToModel(config);
            return DBFactory.GetModel<IDBConfigDal>("IDBConfigModel").GetConfig(model);
        }
    }
}
