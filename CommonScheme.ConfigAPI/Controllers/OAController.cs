using System.Collections.Generic;
using CommonScheme.ConfigCore.ClientServices;
using CommonScheme.ConfigCore.Models;
using CommonScheme.ConfigCore.OAServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommonScheme.ConfigAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OAController : ControllerBase
    {
        #region 添加配置
        [Route("/api/OAConfig/AddConfig")]
        [HttpPost]
        public int AddConfig(ConfigModel config)
        {
            return OAFactory.GetInstace().AddConfig(config);
        }
        [Route("/api/OAConfig/AddConfigs")]
        [HttpPost]
        public bool AddConfigs(List<ConfigModel> configs)
        {
            return OAFactory.GetInstace().AddConfigs(configs);
        }
        [Route("/api/OAConfig/DeleteConfig")]
        [HttpPost]
        public bool DeleteConfig(ConfigModel config)
        {
            return OAFactory.GetInstace().DeleteConfig(config);
        }
        [Route("/api/OAConfig/DeleteConfigs")]
        [HttpPost]
        public bool DeleteConfigs(List<ConfigModel> configs)
        {
            return OAFactory.GetInstace().DeleteConfigs(configs);
        }
        [Route("/api/OAConfig/EditConfig")]
        [HttpPost]
        public bool EditConfig(ConfigModel config)
        {
            return OAFactory.GetInstace().EditConfig(config);
        }
        [Route("/api/OAConfig/EditConfigs")]
        [HttpPost]
        public bool EditConfigs(List<ConfigModel> configs)
        {
            return OAFactory.GetInstace().EditConfigs(configs);
        }
        [Route("/api/OAConfig/GetConfigEntity")]
        [HttpPost]
        public ConfigEntity GetConfigEntity(string code, int parentId)
        {
            return OAFactory.GetInstace().GetConfig(code, parentId);
        }
        [Route("/api/OAConfig/GetConfigEntitys")]
        [HttpPost]
        public List<ConfigEntity> GetConfigEntitys(string[] codes, int parentId = 0)
        {
            return OAFactory.GetInstace().GetConfigs(codes, parentId);
        }
        [Route("/api/OAConfig/GetConfigModel")]
        [HttpGet]
        public ConfigModel GetConfigModel(ConfigModel config)
        {
            return OAFactory.GetInstace().GetConfig(config);
        }
        [Route("/api/OAConfig/GetConfigModels")]
        [HttpGet]
        public List<ConfigModel> GetConfigModels(List<ConfigModel> configs)
        {
            return OAFactory.GetInstace().GetConfigs(configs);
        }
        #endregion

        #region 客户端注册
        [Route("/api/OAConfig/AddClient")]
        [HttpPost]
        public int AddClient(ClientModel model)
        {
            return OAFactory.GetInstace().AddClient(model);
        }
        [Route("/api/OAConfig/EditClient")]
        [HttpPost]
        public bool EditClient(ClientModel model)
        {
            return OAFactory.GetInstace().EditClient(model);
        }
        [Route("/api/OAConfig/DeleteClient")]
        [HttpPost]
        public bool DeleteClient(ClientModel model)
        {
            return OAFactory.GetInstace().DeleteClient(model);
        }
        [Route("/api/OAConfig/GetClient")]
        [HttpPost]
        public ClientModel GetClient(ClientModel model)
        {
            return OAFactory.GetInstace().GetClient(model);
        }
        [Route("/api/OAConfig/GetClients")]
        [HttpPost]
        public List<ClientModel> GetClients(List<ClientModel> models)
        {
            return OAFactory.GetInstace().GetClients(models);
        }
        [Route("/api/OAConfig/RegisterClientOption")]
        [HttpPost]
        public void RegisterClientOption(ClientOptionModel client)
        {
            if (client.ID > 0 && string.IsNullOrEmpty(client.PushType) == false)
                ClientMonitor.RegisterConfig(client.ID);
        } 
        #endregion
    }
}