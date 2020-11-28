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
        public void Push(ClientOptionModel client,ConfigEntity entity);
        public ConfigEntity GetEntity(ConfigEntity config);
        public ConfigModel GetModel(ConfigEntity config);
        public ClientOptionModel GetClientOption(int id);
    }
}
