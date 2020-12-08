using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    /// <summary>
    /// 客户端配置
    /// </summary>
    public class ClientOptionModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 客户端通讯模式
        /// </summary>
        public string PushType { get; set; }
        /// <summary>
        /// http地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// httpheader设置 Dictionary<string, string>
        /// </summary>
        public string Headers { get; set; }
        public string MQConn { get; set; }
        public string VirtualHost { get; set; }
        public string ExchangName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeType { get; set; }
        public string RouteKey { get; set; }
    }
}
