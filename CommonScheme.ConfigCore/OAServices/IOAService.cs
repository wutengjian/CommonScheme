using CommonScheme.ConfigCore.Models;
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

        #region 配置
        public ConfigEntity GetConfig(string code, int parentId);
        public List<ConfigEntity> GetConfigs(string[] codes, int parentId);
        public ConfigModel GetConfig(ConfigModel config);
        public List<ConfigModel> GetConfigs(List<ConfigModel> configs);
        public int AddConfig(ConfigModel config);
        public bool EditConfig(ConfigModel config);
        public bool DeleteConfig(ConfigModel config);
        #endregion

        #region 客户端
        public int AddClient(ClientModel model);
        public bool EditClient(ClientModel model);
        public bool DeleteClient(ClientModel model);
        public ClientModel GetClient(ClientModel model);
        public List<ClientModel> GetClients(List<ClientModel> models);
        #endregion
    }
}
