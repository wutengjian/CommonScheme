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
    public class OAConfigController : ControllerBase
    {
        public OAConfigController() { }

        #region 添加配置
        [Route("/api/OAConfig/AddConfig")]
        [HttpPost]
        public int AddConfig(ConfigModel config)
        {
            return OAFactory.GetInstace().AddConfig(config);
        }

        [Route("/api/OAConfig/DeleteConfig")]
        [HttpPost]
        public bool DeleteConfig(ConfigModel config)
        {
            return OAFactory.GetInstace().DeleteConfig(config);
        }

        [Route("/api/OAConfig/EditConfig")]
        [HttpPost]
        public bool EditConfig(ConfigModel config)
        {
            return OAFactory.GetInstace().EditConfig(config);
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
    }
}