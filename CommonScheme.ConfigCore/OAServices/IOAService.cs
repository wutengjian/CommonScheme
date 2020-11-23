﻿using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore
{
    /// <summary>
    /// 后台管理服务
    /// </summary>
    public interface IOAService
    {
        /**********
         * 只能通过此服务写入配置
         * **********/
        public ConfigEntity GetConfig(string code,int parentId);
        public List<ConfigEntity> GetConfigs(string[] codes,int parentId);
        public ConfigModel GetConfigs(ConfigModel config);
        public List<ConfigModel> GetConfigs(List<ConfigModel> configs);
        public int AddConfig(ConfigModel config);
        public bool AddConfigs(List<ConfigModel> configs);
        public bool EditConfig(ConfigModel config);
        public bool EditConfigs(List<ConfigModel> configs);
        public bool DeleteConfig(ConfigModel config);
        public bool DeleteConfigs(List<ConfigModel> configs);
    }
}
