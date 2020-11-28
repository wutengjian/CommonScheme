using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    public class ConfigModelMapper : ClassMapper<ConfigModel>
    {
        public ConfigModelMapper()
        {
            Table("ConfigCore_Config");
            Map(m => m.ID).Key(KeyType.Identity);// 主键的类型            
            AutoMap();//启用自动映射，一定要调用此方法
        }
    }
    public class ClientModelMapper : ClassMapper<ClientModel>
    {
        public ClientModelMapper()
        {
            Table("ConfigCore_Client");
            Map(m => m.ID).Key(KeyType.Identity);// 主键的类型            
            AutoMap();//启用自动映射，一定要调用此方法
        }
    }
}
