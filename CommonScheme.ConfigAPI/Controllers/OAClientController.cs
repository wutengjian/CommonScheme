using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.ConfigCore.ClientServices;
using CommonScheme.ConfigCore.Models;
using CommonScheme.ConfigCore.OAServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonScheme.ConfigAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OAClientController : ControllerBase
    {
        #region 客户端注册
        [Route("/api/OAClient/AddClient")]
        [HttpPost]
        public int AddClient(ClientModel model)
        {
            return OAFactory.GetInstace().AddClient(model);
        }
        [Route("/api/OAClient/EditClient")]
        [HttpPost]
        public bool EditClient(ClientModel model)
        {
            return OAFactory.GetInstace().EditClient(model);
        }
        [Route("/api/OAClient/DeleteClient")]
        [HttpPost]
        public bool DeleteClient(ClientModel model)
        {
            return OAFactory.GetInstace().DeleteClient(model);
        }
        [Route("/api/OAClient/GetClient")]
        [HttpPost]
        public ClientModel GetClient(ClientModel model)
        {
            return OAFactory.GetInstace().GetClient(model);
        }
        [Route("/api/OAClient/GetClients")]
        [HttpPost]
        public List<ClientModel> GetClients(List<ClientModel> models)
        {
            return OAFactory.GetInstace().GetClients(models);
        }
        [Route("/api/OAClient/RegisterClientOption")]
        [HttpPost]
        public void RegisterClientOption(ClientOptionModel client)
        {
            if (client.ID > 0 && string.IsNullOrEmpty(client.PushType) == false)
                ClientMonitor.RegisterClient(client.ID);
        }
        #endregion

        #region 客户端模块关系
        [Route("/api/OAClient/GetClientAppItems")]
        [HttpGet]
        public List<ClientAppItemModel> GetClientAppItems(int clientID)
        {
            return OAFactory.GetInstace().GetClientAppItems(clientID);
        }
        [Route("/api/OAClient/AddClientAppItem")]
        [HttpPost]
        public int AddClientAppItem(ClientAppItemModel model) { return OAFactory.GetInstace().AddClientAppItem(model); }
        [Route("/api/OAClient/EditClientAppItem")]
        [HttpPost]
        public bool EditClientAppItem(ClientAppItemModel model) { return OAFactory.GetInstace().EditClientAppItem(model); }
        [Route("/api/OAClient/DeleteClientAppItem")]
        [HttpPost]
        public bool DeleteClientAppItem(ClientAppItemModel model) { return OAFactory.GetInstace().DeleteClientAppItem(model); }
        #endregion
    }
}