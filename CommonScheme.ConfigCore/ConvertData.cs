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
            if (model == null)
                return null;
            ConfigEntity entity = new ConfigEntity() { Code = model.Code, ParentID = model.ParentID, Data = model.Data, DataStatus = model.DataStatus, ID = model.ID };
            return entity;
        }
        public static ConfigModel ConfigEntityToModel(ConfigEntity entity)
        {
            if (entity == null)
                return null;
            ConfigModel model = new ConfigModel() { ID = entity.ID, Code = entity.Code, ParentID = entity.ParentID, Data = entity.Data, DataStatus = entity.DataStatus };
            return model;
        }
    }
}
