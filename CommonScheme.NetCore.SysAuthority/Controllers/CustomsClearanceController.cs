using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.NetCore.SysAuthority.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommonScheme.NetCore.SysAuthority.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomsClearanceController : ControllerBase
    {
        [Route("/api/CustomsClearance/CustomsClearanceOrderPush")]
        [HttpPost]
        public ResultHttp CustomsClearanceOrderPush(PushOrder model)
        {
            // PushOrder model = JsonConvert.DeserializeObject<PushOrder>(data);
            return new ResultHttp() { ResponseCode = "10", Message = "成功" };
        }
        [Route("/api/CustomsClearance/CustomsClearanceBatchFinish")]
        [HttpPost]
        public ResultHttp CustomsClearanceBatchFinish(PushBatch model)
        {
            return new ResultHttp() { ResponseCode = "10", Message = "成功" };
        }
        [Route("/api/CustomsClearance/CustomsClearanceReceipt")]
        [HttpPost]
        public ResultHttp CustomsClearanceReceipt(ClearanceReceiptData model)
        {
            return new ResultHttp() { ResponseCode = "10", Message = "成功" };
        }
    }
}
