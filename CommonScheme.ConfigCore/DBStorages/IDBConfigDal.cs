using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages
{
    public interface IDBConfigDal
    {
        public int AddConfig(ConfigModel model);
        public bool EditConfig(ConfigModel model);
        public bool DeleteConfig(ConfigModel model);
        public ConfigModel GetConfig(ConfigModel model);
        public List<ConfigModel> GetConfigs(List<ConfigModel> models);
    }
}
