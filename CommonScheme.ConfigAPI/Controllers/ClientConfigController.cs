using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.ConfigCore.CacheServices;
using CommonScheme.ConfigCore.ClientServices;
using CommonScheme.ConfigCore.DBStorages;
using CommonScheme.ConfigCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonScheme.ConfigAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClientConfigController : ControllerBase
    {
        [Route("/api/ClientConfig/SubscribeConfig")]
        [HttpPost]
        public void SubscribeConfig(ClientConfigModel model)
        {
            if (model.ClientID < 1 && string.IsNullOrEmpty(model.ConfigCode) == false)
            {
                ConfigModel config = DBFactory.GetModel<IDBConfigDal>("IDBConfigDal").GetConfig(new ConfigModel() { ID = model.ConfigID, Code = model.ConfigCode, ParentID = model.ConfigParentID }); ;
                if (config == null || config.ID < 1)
                    return;
                model.ConfigID = config.ID;
                model.ConfigParentID = config.ParentID;
            }
            if (model.ClientID < 1)
                return;
            if (model.ClientState < 1)
            {
                ClientMonitor.CancelClient(model.ClientID);
                return;
            }
            if (model.ConfigID > 0)
                ClientMonitor.RegisterClient(model.ClientID, model.ConfigID);
            else
                ClientMonitor.RegisterClient(model.ClientID);
        }
    }
}