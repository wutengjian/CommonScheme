using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    public class ClientModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        /// <summary>
        /// 证书编码
        /// </summary>
        public string CertCode { get; set; }
        /// <summary>
        /// 客户端状态，大于0：启用、小于0：关闭
        /// </summary>
        public int ClientState { get; set; }

        public string Remake { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
    public class ClientHttpModel : ClientModel
    {
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
    public class ClientRabbitMQModel : ClientModel
    {
        public string MQConn { get; set; }
        public string VirtualHost { get; set; }
        public string ExchangName { get; set; }
        public string EueueName { get; set; }
    }
    public class ClientWebSocketModel : ClientModel
    {
    }
    public class RegistClientModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 客户端状态，大于0：启用、小于0：关闭
        /// </summary>
        public int ClientState { get; set; }
        /// <summary>
        /// 推送给客户端的类型：rabbit、api等
        /// </summary>
        public string PushType { get; set; }
        public ConfigEntity Config { get; set; }
    }
}
