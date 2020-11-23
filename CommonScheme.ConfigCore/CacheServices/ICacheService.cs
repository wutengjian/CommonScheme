using System;
using System.Collections.Generic;
using System.Text;
using CommonScheme.ConfigCore.Models;

namespace CommonScheme.ConfigCore
{
    /// <summary>
    /// 缓存管理服务
    /// </summary>
    public interface ICacheService
    {
        public bool Initialization();
        public ConfigEntity GetConfig(string key);
        public bool SetConfig(string key, ConfigEntity config, TimeSpan ts);
        public bool SetConfig(string key, ConfigEntity config);
        public bool RemoveConfig(string key);
        public bool ExistsConfig(string key);
        public bool SetTime(string key, TimeSpan ts);
    }
}
