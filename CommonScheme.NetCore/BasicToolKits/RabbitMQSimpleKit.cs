using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore.RabbitMQSimple
{
    public class RabbitMQSimpleKit
    {
        private static Dictionary<string, IConnection> mapConn;
        public static void Initialization()
        {
            mapConn = new Dictionary<string, IConnection>();
        }
        public static void CreateConnection(SimpleMQConfig config)
        {
            string key = config.HostUrl + "&" + config.Port + "&" + config.VirtualHost + "&" + config.UserName;
            if (mapConn.ContainsKey(key) == false)
                CreateConnection(key, config);
        }
        public static void CreateConnection(string key, SimpleMQConfig config)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config.HostUrl,
                Port = config.Port > 0 ? config.Port : 5672,
                UserName = config.UserName,
                Password = config.Password,
                VirtualHost = config.VirtualHost,
                AutomaticRecoveryEnabled = true
            };
            if (mapConn.ContainsKey(key) == false)
                mapConn.Add(key, null);
            mapConn[key] = factory.CreateConnection();
        }
        public static void BuildProduction(SimpleMQConfig config, string data)
        {
            string key = config.HostUrl + "&" + config.Port + "&" + config.VirtualHost + "&" + config.UserName;
            if (mapConn.ContainsKey(key) == false)
                CreateConnection(key, config);
            var _channelProduction = mapConn[key].CreateModel();
            _channelProduction.ExchangeDeclare(config.ExchangeName, config.ExchangeType, false, false, null); //声明交换机
            _channelProduction.QueueDeclare(config.QueueName, false, false, false, null);//声明一个队列
            _channelProduction.QueueBind(config.QueueName, config.ExchangeName, config.RouteKey);
            var sendBytes = Encoding.UTF8.GetBytes(data);
            _channelProduction.BasicPublish(config.ExchangeName, config.RouteKey, null, sendBytes);
        }
        public static void BuildConsume(SimpleMQConfig config, Action<string> action)
        {
            string key = config.HostUrl + "&" + config.Port + "&" + config.VirtualHost + "&" + config.UserName;
            if (mapConn.ContainsKey(key) == false)
                CreateConnection(key, config);
            var _channelConsume = mapConn[key].CreateModel();
            _channelConsume.ExchangeDeclare(config.ExchangeName, config.ExchangeType, false, false, null);
            _channelConsume.QueueDeclare(config.QueueName, false, false, false, null);
            _channelConsume.QueueBind(config.QueueName, config.ExchangeName, config.RouteKey);
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channelConsume);//事件基本消费者
            consumer.Received += (ch, ea) =>
            {
                var context = (BasicDeliverEventArgs)ea;
                var data = Encoding.UTF8.GetString(context.Body.ToArray());
                action(JsonConvert.SerializeObject(data));
                _channelConsume.BasicAck(ea.DeliveryTag, false);//确认该消息已被消费
            };
            _channelConsume.BasicConsume(config.QueueName, false, consumer);//启动消费者 设置为手动应答消息
        }
    }

    public class SimpleMQConfig
    {
        public string HostUrl { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string QueueName { get; set; }
        public string RouteKey { get; set; }
    }
}