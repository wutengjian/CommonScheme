using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.DBServices
{
    public interface IDBClientDal
    {
        public int AddConfig(ClientModel model);
        public bool EditConfig(ClientModel model);
        public bool DeleteConfig(ClientModel model);
        public ClientModel GetConfig(ClientModel model);
        public List<ClientModel> GetConfigs(List<ClientModel> models);
    }
}
