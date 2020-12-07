using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.CacheServices
{
    public abstract class CacheServiceBase : ICacheService
    {
        public CacheServiceBase()
        {
            Initialization();
        }
        public abstract bool Initialization();
        public virtual bool ExistsConfig(string key)
        {
            return GetConfig(key) == null ? false : true;
        }

        public abstract ConfigEntity GetConfig(string key);
        public abstract bool RemoveConfig(string key);

        public abstract bool SetConfig(string key, ConfigEntity config, TimeSpan ts);

        public abstract bool SetConfig(string key, ConfigEntity config);

        public abstract bool SetTime(string key, TimeSpan ts);
    }
}
