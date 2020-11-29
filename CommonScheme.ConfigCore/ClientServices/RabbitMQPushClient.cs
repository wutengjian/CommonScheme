using CommonScheme.ConfigCore.Models;
using CommonScheme.NetCore.RabbitMQSimple;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CommonScheme.ConfigCore.ClientServices
{
    public class RabbitMQPushClient : ClientServiceBase
    {
        public override void Push(ClientOptionModel client, ConfigEntity entity)
        {
            var mq = getMQConfig(client);
            RabbitMQSimpleKit.CreateConnection(mq);
            RabbitMQSimpleKit.BuildProduction(mq, JsonConvert.SerializeObject(entity));
        }
        private SimpleMQConfig getMQConfig(ClientOptionModel client)
        {
            NetCore.MQConfig hostConfig = JsonConvert.DeserializeObject<NetCore.MQConfig>(client.MQConn);
            SimpleMQConfig mq = new SimpleMQConfig() { HostUrl = hostConfig.HostUrl, Port = hostConfig.Port, VirtualHost = hostConfig.VirtualHost, UserName = hostConfig.UserName, Password = hostConfig.Password, ExchangeName = client.ExchangName, ExchangeType = client.ExchangeType, QueueName = client.QueueName, RouteKey = client.RouteKey };
            return mq;
        }
    }
}
