using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CommonScheme.NetCore;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using  Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiManagerController : ControllerBase
    {
        //public WebApiManagerController(IHttpContextAccessor httpContextAccessor)
        //{
        //    httpContextAccessor.HttpContext.RequestServices.GetService<DbContext>();
        //}
        [HttpGet]
        [MyActionFilter]
        public string Get()
        {
            return HostDataInfo.Urls + "@     JiannyWu";
        }
        [Route("/api/WebApiManager/MadeGuid")]
        [HttpGet]
        [Authorize]
        public string MadeGuid()
        {
            
            return Guid.NewGuid().ToString();
        }
        [Route("/api/WebApiManager/TestException")]
        [HttpGet]
        public string TestException()
        {
            return Convert.ToInt32("abc").ToString();
        }

        [Route("/api/WebApiManager/Stressing")]
        [HttpGet]
        public string Stressing(string id)
        {
            var data = new { Id = id, Time = DateTime.Now };
            return JsonConvert.SerializeObject(data);
        }
    }
}
