using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore
{
    /// <summary>
    /// 客户端管理服务
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// 推送更改的配置给客户端
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="entity">配置数据</param>
        public void Push(ClientOptionModel client,ConfigEntity entity);
        public ConfigEntity GetEntity(ConfigEntity config);
        public ConfigModel GetModel(ConfigEntity config);
        public int AddClientOption(ClientOptionModel model); 
        public bool EditClientOption(ClientOptionModel model);
        public ClientOptionModel GetClientOption(int id);
    }
}
