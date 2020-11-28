using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace CommonScheme.NetCore
{
    public class RabbitMQKit
    {
        private static Dictionary<string, MQConfig> mapMQConfig;
        private static Dictionary<string, MQInfo> mapMQInfo;
        private static Dictionary<string, IConnection> mapConnection;
        private static Dictionary<string, IModel> mapChannel;
        private static List<ClineInfo> clineInfos;
        private static ConcurrentDictionary<string, ClineMapInfo> mapClineMapInfo;
        public static void Initialization()
        {
            mapMQConfig = new Dictionary<string, MQConfig>();
            mapMQInfo = new Dictionary<string, MQInfo>();
            mapConnection = new Dictionary<string, IConnection>();
            mapChannel = new Dictionary<string, IModel>();
            clineInfos = new List<ClineInfo>();
            mapClineMapInfo = new ConcurrentDictionary<string, ClineMapInfo>();
        }
        public static void AddMQConfigMap(MQConfig config)
        {
            string key = config.HostUrl + "&" + config.Port + "&" + config.VirtualHost;
            if (mapMQConfig.ContainsKey(key) == false)
                mapMQConfig.Add(key, config);
        }
        public static void AddMQInfoList(MQInfo mq)
        {
            string key = mq.HostID + "&" + mq.ExchangeName + "&" + mq.ExchangeType + "&" + mq.QueueName + "&" + mq.RouteKey;
            if (mapMQInfo.ContainsKey(key) == false)
                mapMQInfo.Add(key, null);
            mapMQInfo[key] = mq;
        }

        public static void CreateConnection()
        {
            foreach (var key in mapMQConfig.Keys)
            {
                CreateConnection(key, mapMQConfig[key]);
            }
        }
        public static void CreateConnection(MQConfig config, bool conn = true)
        {
            string key = config.HostUrl + "&" + config.Port + "&" + config.VirtualHost;
            if (mapMQConfig.ContainsKey(key) == false)
                mapMQConfig.Add(key, null);
            mapMQConfig[key] = config;
            CreateConnection(key, config, conn);
        }
        public static bool CreateConnection(string key, MQConfig config, bool conn = true)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config.HostUrl,
                Port = config.Port > 0 ? config.Port : 5672,
                UserName = config.UserName,
                Password = config.Password,
                VirtualHost = config.VirtualHost,
                AutomaticRecoveryEnabled = true,
                //NetworkRecoveryInterval=TimeSpan.FromMilliseconds(1000)
            };
            try
            {
                if (mapConnection.ContainsKey(key) == false)
                {
                    mapConnection.Add(key, null);
                    mapChannel.Add(key, null);
                }
                if (conn == true)
                {
                    IConnection _connection = factory.CreateConnection();//创建连接
                    mapConnection[key] = _connection;
                }
                mapChannel[key] = mapConnection[key].CreateModel();//创建通道
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static void BuildMQ()
        {
            var configs = mapMQConfig.Values.ToDictionary(x => x.HostID, x => x);
            foreach (var key in mapMQInfo.Keys)
            {
                if (configs.ContainsKey(mapMQInfo[key].HostID) == false)
                    continue;
                BuildMQ(configs[mapMQInfo[key].HostID], mapMQInfo[key]);
            }
        }
        public static bool BuildMQ(MQConfig config, MQInfo mq)
        {
            try
            {
                string key = config.HostUrl + "&" + config.Port + "&" + config.VirtualHost;
                mapChannel[key].ExchangeDeclare(mq.ExchangeName, mq.ExchangeType); //声明交换机
                mapChannel[key].QueueDeclare(mq.QueueName, false, false, false, null);//声明一个队列
                mapChannel[key].QueueBind(mq.QueueName, mq.ExchangeName, mq.RouteKey);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static void RegisterCline(ClineInfo cline, MQInfo mqInfo)
        {
            if (clineInfos.Any(x => x.ClineID == cline.ClineID) == false)
                clineInfos.Add(cline);
            string key = cline.ClineID + "&" + cline.ClineType + "&" + mqInfo.HostID;
            if (mapClineMapInfo.ContainsKey(key))
                return;
            var clineMapInfo = new ClineMapInfo() { MQID = mqInfo.MQID, ClineID = cline.ClineID, ClineType = cline.ClineType, MapStatus = 1 };
            mapClineMapInfo.TryAdd(key, clineMapInfo);
        }
        public static void CancellationCline(ClineInfo cline, MQInfo mqInfo)
        {

        }
        public static void BuildProduction(MQHeard heard, string data)
        {
            try
            {
                var sendBytes = Encoding.UTF8.GetBytes(data);
                string key = heard.Config.HostUrl + "&" + heard.Config.Port + "&" + heard.Config.VirtualHost;
                if (mapChannel[key].IsClosed)
                {
                    Console.WriteLine("连接已关闭，重新打开");
                    CreateConnection(heard.Config, false);
                }
                mapChannel[key].BasicPublish(heard.Info.ExchangeName, heard.Info.RouteKey, null, sendBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("推mq消息异常");
            }
        }
        public static void BuildProductionConfirms(MQHeard heard, string[] datas)
        {
            try
            {
                string key = heard.Config.HostUrl + "&" + heard.Config.Port + "&" + heard.Config.VirtualHost;
                if (mapChannel[key].IsClosed)
                {
                    Console.WriteLine("连接已关闭，重新打开");
                    CreateConnection(heard.Config, false);
                }
                mapChannel[key].ConfirmSelect();
                foreach (string data in datas)
                {
                    var sendBytes = Encoding.UTF8.GetBytes(data);
                    mapChannel[key].BasicPublish(heard.Info.ExchangeName, heard.Info.RouteKey, null, sendBytes);
                }
                if (mapChannel[key].WaitForConfirms() == false)
                    Console.Write("Confirm失败");
            }
            catch (Exception ex)
            {
                Console.WriteLine("推mq消息异常");
            }
        }
        public static void BuildProductionTest(MQHeard heard, string data)
        {
            string key = heard.Config.HostUrl + "&" + heard.Config.Port + "&" + heard.Config.VirtualHost;
            if (mapConnection[key].IsOpen == false)
            {
                Console.WriteLine("连接已关闭，重新打开");
                CreateConnection(heard.Config);
            }

            Console.WriteLine("准备发送 " + data);
            var sendBytes = Encoding.UTF8.GetBytes(data);
            using (var channel = mapConnection[key].CreateModel())
            {
                channel.ExchangeDeclare(heard.Info.ExchangeName, heard.Info.ExchangeType); //声明交换机
                channel.QueueDeclare(heard.Info.QueueName, false, false, false, null);//声明一个队列
                channel.QueueBind(heard.Info.QueueName, heard.Info.ExchangeName, heard.Info.RouteKey);
                channel.ConfirmSelect();
                channel.BasicPublish(heard.Info.ExchangeName, heard.Info.RouteKey, null, sendBytes);
                if (channel.WaitForConfirms() == false)
                    Console.Write("Confirm失败");
            }
        }
        public static void BuildConsume(MQHeard heard, Action<string> action)
        {
            string key = heard.Config.HostUrl + "&" + heard.Config.Port + "&" + heard.Config.VirtualHost;
            if (mapConnection.ContainsKey(key) == false)
                return;
            IModel _channel = mapChannel[key];
            _channel.ExchangeDeclare(heard.Info.ExchangeName, heard.Info.ExchangeType); //声明交换机
            _channel.QueueDeclare(heard.Info.QueueName, false, false, false, null);//声明一个队列
            _channel.QueueBind(heard.Info.QueueName, heard.Info.ExchangeName, heard.Info.RouteKey);
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);//事件基本消费者
            consumer.Received += (ch, ea) =>
            {
                var context = (BasicDeliverEventArgs)ea;
                var data = Encoding.UTF8.GetString(context.Body.ToArray());
                action(JsonConvert.SerializeObject(data));
                _channel.BasicAck(ea.DeliveryTag, false);//确认该消息已被消费
            };
            _channel.BasicConsume(heard.Info.QueueName, false, consumer);//启动消费者 设置为手动应答消息
        }
    }
    #region 相关类
    public class MQConfig
    {
        public int HostID { get; set; }
        public string HostUrl { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class MQInfo
    {
        public int MQID { get; set; }
        public int HostID { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string QueueName { get; set; }
        public string RouteKey { get; set; }
    }
    public class ClineInfo
    {
        public int ClineID { get; set; }
        public int ClineType { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 客户端与mq交互的位置
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 客户端说明
        /// </summary>
        public string Remake { get; set; }
    }
    public class ClineMapInfo
    {
        public int MQID { get; set; }
        public int ClineID { get; set; }
        /// <summary>
        /// 生产者：1，消费者：-1
        /// </summary>
        public int ClineType { get; set; }
        public int MapStatus { get; set; }
    }

    public class MQHeard
    {
        public int ClineID { get; set; }
        public MQConfig Config { get; set; }
        public MQInfo Info { get; set; }
    }
    #endregion
}
