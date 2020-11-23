using System;
using System.Collections.Generic;
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
    }
    public class ClientHttpModel: ClientModel
    {
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
    public class ClientRabbitMQModel: ClientModel
    {
        public string MQConn { get; set; }
        public string VirtualHost { get; set; }
        public string ExchangName { get; set; }
        public string EueueName { get; set; }
    }
    public class ClientWebSocketModel: ClientModel
    {
    }
    public class ClientPushModel
    {
        public int ClientID { get; set; }
        public string ConfigID { get; set; }
        public string ClientCode { get; set; }
        public string ConfigCode { get; set; }
        public DateTime PushTime { get; set; }
        public int PushStatus { get; set; }
    }
}
