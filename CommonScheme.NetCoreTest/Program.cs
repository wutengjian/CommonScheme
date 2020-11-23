using CommonScheme.NetCore.BasicToolKits;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonScheme.NetCoreTest
{
    class Program
    {
        static HttpCoreHepler http = new HttpCoreHepler();
        static void Main(string[] args)
        {
            //TestRabbitMQ();
            for (int i = 0; i < 30; i++)
            {
                Task.Factory.StartNew((a) =>
                {
                    for (int n = 0; n < 1000; n++)
                    {
                        var result = http.Get("https://localhost:8081/api/WebApiManager/Stressing?id=" + Guid.NewGuid().ToString());
                        Console.WriteLine(a + "@" + n + "@" + result);
                    }
                }, i);
            }
            while (true)
            {
                if (Console.ReadLine() == "exit")
                    break;
            }
        }
        #region RabbitMQ
        public static void TestRabbitMQ()
        {
            RabbitMQKit.Initialization();
            MQConfig config = new MQConfig() { HostID = 1, HostUrl = "10.10.115.11", Port = 5672, VirtualHost = "CommonScheme", UserName = "CommonScheme", Password = "CommonScheme" };
            RabbitMQKit.CreateConnection(config);
            MQInfo MQtopic01 = new MQInfo() { HostID = 1, MQID = 1, ExchangeName = "CommonSchemeExchangeTopic", ExchangeType = "topic", QueueName = "CommonSchemeQueue01", RouteKey = "www.01" };
            MQInfo MQtopic02 = new MQInfo() { HostID = 1, MQID = 2, ExchangeName = "CommonSchemeExchangeTopic", ExchangeType = "topic", QueueName = "CommonSchemeQueue01", RouteKey = "www.02" };
            MQInfo MQtopic03 = new MQInfo() { HostID = 1, MQID = 3, ExchangeName = "CommonSchemeExchangeTopic", ExchangeType = "topic", QueueName = "CommonSchemeQueue02", RouteKey = "bbb.01" };
            MQInfo MQtopic04 = new MQInfo() { HostID = 1, MQID = 4, ExchangeName = "CommonSchemeExchangeTopic", ExchangeType = "topic", QueueName = "CommonSchemeQueue02", RouteKey = "bbb.02" };
            MQInfo MQdirect = new MQInfo() { HostID = 1, MQID = 5, ExchangeName = "CommonSchemeExchangeDirect", ExchangeType = "direct", QueueName = "CommonSchemeQueue03", RouteKey = "jianny" };
            MQInfo MQfanout = new MQInfo() { HostID = 1, MQID = 6, ExchangeName = "CommonSchemeExchangeFanout", ExchangeType = "fanout", QueueName = "CommonSchemeQueue04", RouteKey = "fanout" };

            RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = 1, ClineType = 1 }, new MQInfo() { HostID = 1, MQID = 1 });
            RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = 2, ClineType = 1 }, new MQInfo() { HostID = 1, MQID = 2 });
            RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = 3, ClineType = 1 }, new MQInfo() { HostID = 1, MQID = 3 });
            RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = 4, ClineType = 1 }, new MQInfo() { HostID = 1, MQID = 4 });
            RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = 5, ClineType = 1 }, new MQInfo() { HostID = 1, MQID = 5 });
            RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = 6, ClineType = 1 }, new MQInfo() { HostID = 1, MQID = 6 });
            //RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = -1, ClineType = -1 }, new MQInfo() { HostID = 1, MQID = 1 });
            //RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = -2, ClineType = -1 }, new MQInfo() { HostID = 1, MQID = 2 });
            //RabbitMQKit.RegisterCline(new ClineInfo() { ClineID = -3, ClineType = -1 }, new MQInfo() { HostID = 1, MQID = 3 });
            RabbitMQKit.BuildMQ();
            ProductionMQ(1, new MQHeard() { Config = config, Info = MQtopic01 });
            ProductionMQ(2, new MQHeard() { Config = config, Info = MQtopic02 });
            ProductionMQ(3, new MQHeard() { Config = config, Info = MQtopic03 });
            ProductionMQ(4, new MQHeard() { Config = config, Info = MQtopic04 });
            ProductionMQ(5, new MQHeard() { Config = config, Info = MQdirect });
            ProductionMQ(6, new MQHeard() { Config = config, Info = MQfanout });
            //ConsumerMQ(-1, new MQHeard() { Config = config, Info = MQtopic });
            //ConsumerMQ(-2, new MQHeard() { Config = config, Info = MQdirect });
            //ConsumerMQ(-3, new MQHeard() { Config = config, Info = MQfanout });
        }
        private static void ProductionMQ(int clineID, MQHeard heard)
        {
            Task.Run(() =>
            {
                try { }
                catch (Exception ex)
                {
                    Console.WriteLine("线程失败:" + ex.Message);
                }
                heard.ClineID = clineID;
                while (true)
                {
                    RabbitMQKit.BuildProduction(heard, heard.ClineID + "&" + heard.Info.RouteKey);
                    Thread.Sleep(1000);
                }
            });
        }
        private static void ConsumerMQ(int clineID, MQHeard heard)
        {
            Task.Run(() =>
            {
                heard.ClineID = clineID;
                RabbitMQKit.BuildConsume(heard, (data) =>
                {
                    Console.WriteLine(heard.ClineID + " 输出：" + data);
                });
            });
        }
        #endregion
    }
}
