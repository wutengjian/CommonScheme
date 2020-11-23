using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonScheme.Net.Test
{
    public class RabbitMQSimpleTest
    {
        public void Run()
        {
            MQConfig config = new MQConfig() { HostUrl = "10.10.115.11", Port = 5672, VirtualHost = "CommonScheme", UserName = "CommonScheme", Password = "CommonScheme" };
            RabbitMQSimpleKit.CreateConnection(config);

            MQInfo MQtopic01 = new MQInfo() { HostID = 1, ExchangeName = "CommonSchemeExchangeTopic", ExchangeType = "topic", QueueName = "CommonSchemeQueue01", RouteKey = "www.*" };
            MQInfo MQtopic02 = new MQInfo() { HostID = 1, ExchangeName = "CommonSchemeExchangeTopic", ExchangeType = "topic", QueueName = "CommonSchemeQueue02", RouteKey = "bbb.*" };
            MQInfo MQdirect = new MQInfo() { HostID = 1, ExchangeName = "CommonSchemeExchangeDirect", ExchangeType = "direct", QueueName = "CommonSchemeQueue03", RouteKey = "jianny" };
            MQInfo MQfanout = new MQInfo() { HostID = 1, ExchangeName = "CommonSchemeExchangeFanout", ExchangeType = "fanout", QueueName = "CommonSchemeQueue04", RouteKey = "fanout" };

            ConsumerMQ(MQtopic01, "MQtopic01");
            ConsumerMQ(MQtopic02, "MQtopic02");
            ConsumerMQ(MQdirect, "MQdirect");
            ConsumerMQ(MQfanout, "MQfanout");
        }

        private static void ProductionMQ(MQInfo mq)
        {
            RabbitMQSimpleKit.BuildProduction(mq, mq.RouteKey + "@ " + Guid.NewGuid().ToString());
            for (int i = 0; i < 100; i++)
            {
                RabbitMQSimpleKit.BuildProduction(mq, mq.RouteKey + "@ " + i);
            }
        }
        private static void ConsumerMQ(MQInfo mq, string key)
        {
            RabbitMQSimpleKit.BuildConsume(mq, (data) =>
            {
                Console.WriteLine(key + ": " + data);
                Thread.Sleep(100);
            });
        }
    }
}
