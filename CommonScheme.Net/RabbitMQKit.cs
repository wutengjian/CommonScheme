using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonScheme.Net
{
    public class RabbitMQKit
    {
        public static MQConfig _config { get; set; }
        private static IConnection _connection = null;
        private static IModel _channel = null;
        private static List<IModel> ChannelList = null;
        private static int ChannelIndex = 0;
        private static List<MQInfo> MQList = new List<MQInfo>();
        private static List<ClineMapInfo> maps = new List<ClineMapInfo>();
        public static void CreateConnection(MQConfig config = null)
        {
            if (config != null)
                _config = config;
            var factory = new ConnectionFactory()
            {
                HostName = _config.HostUrl,
                Port = _config.Port > 0 ? _config.Port : 5672,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost,
                AutomaticRecoveryEnabled = true
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();//创建通道
            if (ChannelList == null || ChannelList.Count < 1)
            {
                ChannelList = new List<IModel>(10);
                for (int i = 0; i < 10; i++)
                {
                    ChannelList.Add(_connection.CreateModel());
                }
            }
        }
        /// <summary>
        /// 初始化时需要加载一次已注册的mq
        /// </summary>
        /// <param name="list"></param>
        public static void BuildMQ()
        {
            if (_connection == null || _connection.IsOpen == false)
                CreateConnection();
            Dictionary<string, MQInfo> dic = new Dictionary<string, MQInfo>();
            foreach (var mq in MQList)
            {
                if (dic.ContainsKey(mq.ExchangeName + "&" + mq.QueueName) == false)
                {
                    string routeKey = "";
                    _channel.ExchangeDeclare(mq.ExchangeName, mq.ExchangeType); //声明交换机
                    _channel.QueueDeclare(mq.QueueName, false, false, false, null);//声明一个队列
                    _channel.QueueBind(mq.QueueName, mq.ExchangeName, routeKey);
                    dic.Add(mq.ExchangeName + "&" + mq.QueueName, mq);
                }
            }
            _channel.Close();
            _connection.Close();
        }
        /// <summary>
        /// 注册客户端
        /// </summary>
        /// <param name="mqInfo">mq信息</param>
        /// <param name="cline">客户端信息</param>
        public static void RegisterCline(MQInfo mqInfo, ClineInfo cline)
        {
            var mq = MQList.Where(x => x.ExchangeName == mqInfo.ExchangeName && x.QueueName == mqInfo.QueueName).FirstOrDefault();
            ClineMapInfo map = new ClineMapInfo() { MapStatus = 0, ClienID = cline.ClineID, ClineType = cline.ClineType };
            if (cline.ClineType == 1)
            {

                if (mq == null || string.IsNullOrEmpty(mq.ExchangeName))
                {
                    BuildMQ(mqInfo);
                    MQList.Add(mqInfo);
                }
                mq = mqInfo;
            }
            else if (mq == null || string.IsNullOrEmpty(mq.ExchangeName))
                return;//没有查到mq
            else if (cline.ClineType == 0)
                return;
            map.MQID = mqInfo.HostID;
            if (maps.Any(x => x.MQID == mq.HostID && x.ClienID == cline.ClineID && x.ClineType == cline.ClineType) == false)
                maps.Add(map);//向 ClineMapInfo加数据
        }
        /// <summary>
        /// 生产者
        /// </summary>
        /// <param name="mq"></param>
        /// <param name="data"></param>
        public static void BuildProduction(int clineID, string data)
        {
            //根据客户端ID匹配其注册的MQ信息
            var mqID = maps.Where(x => x.ClienID == clineID && x.ClineType == 1).FirstOrDefault().MQID;
            MQInfo mq = MQList.Where(x => x.HostID == mqID).FirstOrDefault();
            string routeKey = "";
            var channel = GetChannel();
            channel.ExchangeDeclare(mq.ExchangeName, mq.ExchangeType); //声明交换机 
            channel.QueueDeclare(mq.QueueName, false, false, false, null);//声明一个队列
            channel.QueueBind(mq.QueueName, mq.ExchangeName, routeKey);
            var sendBytes = Encoding.UTF8.GetBytes(data);
            channel.BasicPublish(mq.ExchangeName, routeKey, null, sendBytes);
        }
        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="mq"></param>
        public static void BuildConsume(int clineID, Action<string> action)
        {
            //根据客户端ID匹配其注册的MQ信息
            var mqID = maps.Where(x => x.ClienID == clineID && x.ClineType == -1).FirstOrDefault().MQID;
            MQInfo mq = MQList.Where(x => x.HostID == mqID).FirstOrDefault();
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);//事件基本消费者
            consumer.Received += (ch, ea) =>
            {
                var context = (BasicDeliverEventArgs)ea;
                var data = Encoding.UTF8.GetString(context.Body.ToArray());
                action(JsonConvert.SerializeObject(data));
                _channel.BasicAck(ea.DeliveryTag, false);//确认该消息已被消费
            };
            _channel.BasicConsume(mq.QueueName, false, consumer);//启动消费者 设置为手动应答消息
        }

        private static void BuildMQ(MQInfo mq)
        {
            if (_connection == null || _connection.IsOpen == false)
                CreateConnection();
            string routeKey = "";
            _channel.ExchangeDeclare(mq.ExchangeName, mq.ExchangeType); //声明交换机
            _channel.QueueDeclare(mq.QueueName, false, false, false, null);//声明一个队列
            _channel.QueueBind(mq.QueueName, mq.ExchangeName, routeKey);
        }
        private static IModel GetChannel()
        {
            if (_connection == null || _connection.IsOpen == false)
                CreateConnection();
            if (ChannelIndex > ChannelList.Count)
                ChannelIndex = 0;
            return ChannelList[ChannelIndex];
        }
    }
    #region 相关类
    public class MQConfig
    {
        public int HostID { get; set; }
        public string HostUrl { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
    }
    public class MQInfo
    {
        public int HostID { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string QueueName { get; set; }
        public string RouteKey { get; set; }
    }
    public class ClineInfo
    {
        public int ClineID { get; set; }
        /// <summary>
        /// 生产者：1，消费者：-1
        /// </summary>
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

        /// <summary>
        /// 客户端ID
        /// </summary>
        public int ClienID { get; set; }
        public int ClineType { get; set; }
        public int MapStatus { get; set; }
    }
    #endregion
}
