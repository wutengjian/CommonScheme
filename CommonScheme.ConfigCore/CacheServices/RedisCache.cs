using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CommonScheme.NetCore;

namespace CommonScheme.ConfigCore.CacheServices
{
    public class RedisCache : CacheServiceBase
    {
        public RedisCache() : base() { }
        public override bool Initialization()
        {
            CSRedisCoreKit.Initialization(AppSettings.GetAppSeting("ConnectionStrings:CommonSchemeRedis"));
            return true;
        }
        public override bool ExistsConfig(string key)
        {
            return GetConfig(key) == null ? false : true;
        }

        public override ConfigEntity GetConfig(string key)
        {
            return CSRedisCoreKit.Get<ConfigEntity>(key);
        }

        public override bool RemoveConfig(string key)
        {
            return CSRedisCoreKit.Remove(key);
        }

        public override bool SetConfig(string key, ConfigEntity config, TimeSpan ts)
        {
            return CSRedisCoreKit.Set<ConfigEntity>(key, config, ts);
        }

        public override bool SetConfig(string key, ConfigEntity config)
        {
            return SetConfig(key, config, TimeSpan.FromMinutes(10));
        }

        public override bool SetTime(string key, TimeSpan ts)
        {
            return CSRedisCoreKit.SetTime(key, ts);
        }
    }
}
