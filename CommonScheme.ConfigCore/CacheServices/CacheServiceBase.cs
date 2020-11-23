using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.CacheServices
{
    public abstract class CacheServiceBase : ICacheService
    {
        public virtual bool Initialization() {
            return true;
        }
        public virtual bool ExistsConfig(string key)
        {
            return GetConfig(key) == null ? false : true;
        }

        public virtual ConfigEntity GetConfig(string key)
        {
            throw new NotImplementedException();
        }

        public virtual bool RemoveConfig(string key)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetConfig(string key, ConfigEntity config, TimeSpan ts)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetConfig(string key, ConfigEntity config)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetTime(string key, TimeSpan ts)
        {
            throw new NotImplementedException();
        }
    }
}
