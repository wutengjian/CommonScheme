using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonScheme.Net
{
    public class RabbitMQSimpleKit
    {
        public static MQConfig _config { get; set; }
        private static IConnection _connection = null;
        private static IModel _channelProduction = null;
        private static IModel _channelConsume = null;
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
            //_channelProduction = _connection.CreateModel();//创建通道
            _channelConsume = _connection.CreateModel();//创建通道
        }
        public static void BuildProduction(MQInfo mq, string data)
        {
            _channelProduction.ExchangeDeclare(mq.ExchangeName, mq.ExchangeType, false, false, null); //声明交换机
            _channelProduction.QueueDeclare(mq.QueueName, false, false, false, null);//声明一个队列
            _channelProduction.QueueBind(mq.QueueName, mq.ExchangeName, mq.RouteKey);
            var sendBytes = Encoding.UTF8.GetBytes(data);
            _channelProduction.BasicPublish(mq.ExchangeName, mq.RouteKey, null, sendBytes);
        }
        public static void BuildConsume(MQInfo mq, Action<string> action)
        {
            _channelConsume.ExchangeDeclare(mq.ExchangeName, mq.ExchangeType, false, false, null);
            _channelConsume.QueueDeclare(mq.QueueName, false, false, false, null);
            _channelConsume.QueueBind(mq.QueueName, mq.ExchangeName, mq.RouteKey);
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channelConsume);//事件基本消费者
            consumer.Received += (ch, ea) =>
            {
                var context = (BasicDeliverEventArgs)ea;
                var data = Encoding.UTF8.GetString(context.Body.ToArray());
                action(JsonConvert.SerializeObject(data));
                _channelConsume.BasicAck(ea.DeliveryTag, false);//确认该消息已被消费
            };
            _channelConsume.BasicConsume(mq.QueueName, false, consumer);//启动消费者 设置为手动应答消息
        }
    }
}
