using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    public class ConfigMapper : ClassMapper<ConfigModel>
    {
        public ConfigMapper()
        {
            Table("ConfigCore_Config");
            Map(m => m.ID).Key(KeyType.Identity);// 主键的类型            
            AutoMap();//启用自动映射，一定要调用此方法
        }
    }
    public class ClientMapper : ClassMapper<ClientModel>
    {
        public ClientMapper()
        {
            Table("ConfigCore_Client");
            Map(m => m.ID).Key(KeyType.Identity);// 主键的类型            
            AutoMap();//启用自动映射，一定要调用此方法
        }
    }
    public class ClientOptionMapper : ClassMapper<ClientOptionModel>
    {
        public ClientOptionMapper()
        {
            Table("ConfigCore_ClientOption");
            Map(m => m.ID).Key(KeyType.Identity);// 主键的类型            
            AutoMap();//启用自动映射，一定要调用此方法
        }
    }
}
