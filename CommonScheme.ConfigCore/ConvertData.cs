using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore
{
    public class ConvertData
    {
        public static ConfigEntity ConfigModelToEntity(ConfigModel model)
        {
            ConfigEntity entity = new ConfigEntity() { };
            return entity;
        }
        public static ConfigModel ConfigEntityToModel(ConfigEntity entity)
        {
            ConfigModel model = new ConfigModel();
            return model;
        }
    }
}
