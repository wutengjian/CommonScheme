using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages
{
    public interface IDBClientDal
    {
        public int AddClient(ClientModel model);
        public bool EditClient(ClientModel model);
        public bool DeleteClient(ClientModel model);
        public ClientModel GetClient(ClientModel model);
        public List<ClientModel> GetClients(List<ClientModel> models);
        public ClientOptionModel GetClientOption(int id);
        public bool AddClientOption(ClientOptionModel model);
        public bool EditClientOption(ClientOptionModel model);
    }
}
