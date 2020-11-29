using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.ConfigCore.Models;
using CommonScheme.NetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonScheme.ConfigAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestClientController : ControllerBase
    {
        [Route("/api/TestClient/ReceiveConfig")]
        [HttpPost]
        public string ReceiveConfig(ConfigEntity config)
        {
            Console.WriteLine(config.Data);
            MemoryCacheKit.Set_NotExpire<ConfigEntity>(config.Code, config);
            return config.Data;
        }
    }
}