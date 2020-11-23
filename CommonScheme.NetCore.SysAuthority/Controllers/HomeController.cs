using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommonScheme.NetCore.SysAuthority.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("/api/Home/Info")]
        [HttpGet]
        public string Info()
        {
            return "Jianny";
        }
        [Route("/api/Home/GetInfo")]
        [HttpGet]
        [Authorize]
        public string GetInfo()
        {
            return "已授权";
        }
      
    }
}
