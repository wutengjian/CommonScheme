using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.ConfigCore.CacheServices;
using CommonScheme.ConfigCore.ClientServices;
using CommonScheme.ConfigCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonScheme.ConfigAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientConfigController : ControllerBase
    {
        [Route("/api/ClientConfig/RegisterClient")]
        [HttpPost]
        public void SubscribeConfig(RegistClientModel clientConfig)
        {
            if (clientConfig.ID < 1)
                return;
            if (clientConfig.ClientState > 0&&clientConfig.Config!=null)
            {
                ClientMonitor.RegisterConfig(clientConfig.ID, CacheFactory.MadePrefix(clientConfig.Config.Code, clientConfig.Config.ParentID));
            }
            else if (clientConfig.ClientState < 0)
            {
                ClientMonitor.CancelConfig(clientConfig.ID);
            }
            else if (string.IsNullOrEmpty(clientConfig.PushType) == false)
            {
                //更改推送给客户端配置
            }
        }
    }
}