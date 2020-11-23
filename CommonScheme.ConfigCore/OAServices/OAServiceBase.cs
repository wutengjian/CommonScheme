using CommonScheme.ConfigCore.Models;
using CommonScheme.ConfigCore.CacheServices;
using CommonScheme.ConfigCore.DBServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.OAServices
{
    public abstract class OAServiceBase : IOAService
    {
        public string Service { get; set; }
        public OAServiceBase(string service) { Service = service; }
        public virtual int AddConfig(ConfigModel config)
        {
            config.ID = DBFactory.GetModel<IDBConfigDal>("IDBConfigModel").AddConfig(config);
            if (config.ID > 0)
            {
                ConfigEntity entity = ConvertData.ConfigModelToEntity(config);
                string key = CacheFactory.MadePrefix(config.Code, config.ParentID);
                CacheFactory.GetInstace().SetConfig(key, entity);
            }
            return config.ID;
        }

        public virtual bool AddConfigs(List<ConfigModel> configs)
        {
            foreach (var config in configs)
            {
                this.AddConfig(config);
            }
            return true;
        }

        public virtual bool DeleteConfig(ConfigModel config)
        {
            DBFactory.GetModel<IDBConfigDal>("IDBConfigModel").DeleteConfig(config);
            string key = CacheFactory.MadePrefix(config.Code, config.ParentID);
            return CacheFactory.GetInstace().RemoveConfig(key);
        }

        public virtual bool DeleteConfigs(List<ConfigModel> configs)
        {
            foreach (var config in configs)
            {
                DeleteConfig(config);
            }
            return true;
        }

        public virtual bool EditConfig(ConfigModel config)
        {
            DBFactory.GetModel<IDBConfigDal>("IDBConfigModel").EditConfig(config);
            ConfigEntity entity = ConvertData.ConfigModelToEntity(config);
            string key = CacheFactory.MadePrefix(config.Code, config.ParentID);
            return CacheFactory.GetInstace().SetConfig(key, entity);
        }

        public virtual bool EditConfigs(List<ConfigModel> configs)
        {
            foreach (var config in configs)
            {
                EditConfig(config);
            }
            return true;
        }

        public virtual ConfigEntity GetConfig(string code, int parentId)
        {
            string key = CacheFactory.MadePrefix(code, parentId);
            return CacheFactory.GetInstace().GetConfig(key);
        }
        public virtual List<ConfigEntity> GetConfigs(string[] codes, int parentId = 0)
        {
            List<ConfigEntity> list = new List<ConfigEntity>(codes.Length);
            foreach (var code in codes)
            {
                list.Add(GetConfig(code, parentId));
            }
            return list;
        }
        public virtual ConfigModel GetConfigs(ConfigModel config)
        {
            return DBFactory.GetModel<IDBConfigDal>("IDBConfigModel").GetConfig(config);
        }

        public virtual List<ConfigModel> GetConfigs(List<ConfigModel> configs)
        {
            return DBFactory.GetModel<IDBConfigDal>("IDBConfigModel").GetConfigs(configs);
        }
    }
}
