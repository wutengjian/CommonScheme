using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.DBServices
{
    public interface IDBClientPushDal
    {
        public int AddConfig(ClientPushModel model);
        public bool EditConfig(ClientPushModel model);
        public bool DeleteConfig(ClientPushModel model);
        public ClientPushModel GetConfig(ClientPushModel model);
        public List<ClientPushModel> GetConfigs(List<ClientPushModel> models);
    }
}
