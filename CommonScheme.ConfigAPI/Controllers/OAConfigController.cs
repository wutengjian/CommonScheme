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
    public class OAConfigController : ControllerBase
    {
        [Route("/api/OAConfig/RegisterClient")]
        [HttpPost]
        public void RegisterClient(ClientModel client)
        {
            registerConfig(client,null);
        }
        [Route("/api/OAConfig/RegisterHttpClient")]
        [HttpPost]
        public void RegisterHttpClient(ClientHttpModel client)
        {
            registerConfig(client,"Http");
        }
        [Route("/api/OAConfig/RegisterRabbitMQClient")]
        [HttpPost]
        public void RegisterRabbitMQClient(ClientRabbitMQModel client)
        {
            registerConfig(client, "RabbitMQ");
        }
        [Route("/api/OAConfig/RegisterWebSocketClient")]
        [HttpPost]
        public void RegisterWebSocketClient(ClientWebSocketModel client)
        {
            registerConfig(client, "WebSocket");
        }
        private void registerConfig(ClientModel client,string pushType)
        {
            if (client.ID > 0 && client.ClientState > 0&&string.IsNullOrEmpty(pushType)==false)
                ClientMonitor.RegisterConfig(client.ID);
        }
    }
}