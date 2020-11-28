using CommonScheme.ConfigCore.Models;
using CommonScheme.ConfigCore.CacheServices;
using CommonScheme.ConfigCore.DBStorages;
using System;
using System.Collections.Generic;
using System.Text;
using CommonScheme.ConfigCore.ClientServices;

namespace CommonScheme.ConfigCore.OAServices
{
    public class OAService : IOAService
    {
        #region 配置
        public int AddConfig(ConfigModel config)
        {
            config.ID = DBFactory.GetModel<IDBConfigDal>("IDBConfigDal").AddConfig(config);
            if (config.ID > 0)
            {
                ConfigEntity entity = ConvertData.ConfigModelToEntity(config);
                ICacheService cache = CacheFactory.GetInstace();
                if (cache != null)
                {
                    string key = CacheFactory.MadePrefix(config.Code, config.ParentID);
                    cache.SetConfig(key, entity);
                }
            }
            return config.ID;
        }
        public bool AddConfigs(List<ConfigModel> configs)
        {
            foreach (var config in configs)
            {
                this.AddConfig(config);
            }
            return true;
        }
        public bool DeleteConfig(ConfigModel config)
        {
            DBFactory.GetModel<IDBConfigDal>("IDBConfigDal").DeleteConfig(config);
            ICacheService cache = CacheFactory.GetInstace();
            if (cache != null)
            {
                string key = CacheFactory.MadePrefix(config.Code, config.ParentID);
                cache.RemoveConfig(key);
            }
            return true;
        }
        public bool DeleteConfigs(List<ConfigModel> configs)
        {
            foreach (var config in configs)
            {
                DeleteConfig(config);
            }
            return true;
        }
        public bool EditConfig(ConfigModel config)
        {
            DBFactory.GetModel<IDBConfigDal>("IDBConfigDal").EditConfig(config);
            ConfigEntity entity = ConvertData.ConfigModelToEntity(config);
            ICacheService cache = CacheFactory.GetInstace();
            if (cache != null)
            {
                string key = CacheFactory.MadePrefix(config.Code, config.ParentID);
                cache.SetConfig(key, entity);
            }
            return true;
        }
        public bool EditConfigs(List<ConfigModel> configs)
        {
            foreach (var config in configs)
            {
                EditConfig(config);
            }
            return true;
        }
        public ConfigEntity GetConfig(string code, int parentId)
        {
            ICacheService cache = CacheFactory.GetInstace();
            if (cache != null)
            {
                string key = CacheFactory.MadePrefix(code, parentId);
                return cache.GetConfig(key);
            }
            else
            {
                var model = GetConfig(new ConfigModel() { Code = code, ParentID = parentId });
                return ConvertData.ConfigModelToEntity(model);
            } 
        }
        public List<ConfigEntity> GetConfigs(string[] codes, int parentId = 0)
        {
            List<ConfigEntity> list = new List<ConfigEntity>(codes.Length);
            foreach (var code in codes)
            {
                list.Add(GetConfig(code, parentId));
            }
            return list;
        }
        public ConfigModel GetConfig(ConfigModel config)
        {
            return DBFactory.GetModel<IDBConfigDal>("IDBConfigDal").GetConfig(config);
        }
        public List<ConfigModel> GetConfigs(List<ConfigModel> configs)
        {
            return DBFactory.GetModel<IDBConfigDal>("IDBConfigDal").GetConfigs(configs);
        }
        #endregion

        #region 客户端
        public int AddClient(ClientModel model)
        {
            model.ID = DBFactory.GetModel<IDBClientDal>("IDBClientDal").AddClient(model);
            if (model.ID > 0 && model.ClientState > 0)
                ClientMonitor.RegisterConfig(model.ID);
            return model.ID;
        }
        public bool EditClient(ClientModel model)
        {
            if (model.ID <= 0)
                return false;
            bool result = DBFactory.GetModel<IDBClientDal>("IDBClientDal").DeleteClient(model);
            if (result == true && model.ClientState < 0)
                ClientMonitor.CancelConfig(model.ID);
            return result;
        }
        public bool DeleteClient(ClientModel model)
        {
            if (model.ID <= 0)
                return false;
            bool result = DBFactory.GetModel<IDBClientDal>("IDBClientDal").DeleteClient(model);
            if (result == true)
                ClientMonitor.CancelConfig(model.ID);
            return result;
        }
        public ClientModel GetClient(ClientModel model)
        {
            if (model.ID <= 0)
                return null;
            return DBFactory.GetModel<IDBClientDal>("IDBClientDal").GetClient(model);
        }
        public List<ClientModel> GetClients(List<ClientModel> models)
        {
            return DBFactory.GetModel<IDBClientDal>("IDBClientDal").GetClients(models);
        }
        #endregion
    }
}
