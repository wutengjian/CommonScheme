using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace CommonScheme.NetCore.BasicToolKits
{
    public class StackExchangeRedisKit
    {
        private static IConnectionMultiplexer connmultiplexer = null;
        private static IDatabase db = null;

        public static void Initialization(string[] hosts, int dbIndex = 0)
        {
            ConfigurationOptions config = new ConfigurationOptions
            {
                Ssl = false,
                AllowAdmin = false,
                AbortOnConnectFail = false,
                KeepAlive = 180,//发送消息以帮助保持套接字活动的时间（秒）（默认时间60s）
                Password = ""
            };
            foreach (var host in hosts)
            {
                config.EndPoints.Add(host);
            }
            //配置连接
            connmultiplexer = ConnectionMultiplexer.Connect(config);
            db = connmultiplexer.GetDatabase(dbIndex);//指定连接的库 0
            AddRegisterEvent();
        }
        /// <summary>
        /// 哨兵集群
        /// </summary>
        /// <param name="SentinelName"></param>
        /// <param name="hosts"></param>
        /// <param name="dbIndex"></param>
        private static void Initialization(string SentinelName, string[] hosts, int dbIndex = 0)
        {
            ConfigurationOptions config = new ConfigurationOptions
            {
                Ssl = false,
                AllowAdmin = false,
                AbortOnConnectFail = false,
                SslHost = hosts[0],
                KeepAlive = 180,//发送消息以帮助保持套接字活动的时间（秒）（默认时间60s）
                Password = "",
                ServiceName = SentinelName
            };
            foreach (var host in hosts)
            {
                config.EndPoints.Add(host);
            }
            config.CommandMap = CommandMap.Sentinel;
            config.TieBreaker = "";

            //配置连接
            connmultiplexer = ConnectionMultiplexer.Connect(config);
            /******* 高版本的StackExchangeRedis组件才支持哨兵模式
                        //ConfigurationOptions redisServiceOptions = new ConfigurationOptions();
                        //redisServiceOptions.ServiceName = SentinelName;   //master名称
                        //redisServiceOptions.Password = "";     //master访问密码
                        //redisServiceOptions.AbortOnConnectFail = true;
                        //connmultiplexer = connmultiplexer.GetSentinelMasterConnection(redisServiceOptions);
            ******/
            db = connmultiplexer.GetDatabase(dbIndex);//指定连接的库 0
            AddRegisterEvent();
        }

        public static string GetOrSetString(string key, Func<string> func, TimeSpan expiresIn)
        {
            var rdata = db.StringGet(key);
            if (rdata.HasValue == false)
            {
                var t = func();
                db.StringSet(key, t, expiresIn);
                return t;
            }
            return rdata.ToString();
        }
        public static T GetOrSet<T>(string key, Func<T> func, TimeSpan expiresIn)
        {
            var rdata = db.StringGet(key);
            if (rdata.HasValue == false)
            {
                var t = func();
                db.StringSet(key, JsonConvert.SerializeObject(t), expiresIn);
                return t;
            }
            return JsonConvert.DeserializeObject<T>(rdata.ToString());
        }
        public static T HGetOrSet<T>(string key, string field, Func<T> func)
        {
            var rdata = db.HashGet(key, field);
            if (rdata.HasValue == false)
            {
                var t = func();
                db.HashSet(key, field, JsonConvert.SerializeObject(t));
                return t;
            }
            return JsonConvert.DeserializeObject<T>(rdata.ToString());
        }

        #region 集群事件
        private static void AddRegisterEvent()
        {
            connmultiplexer.ConnectionRestored += ConnectionRestored;
            connmultiplexer.ConnectionFailed += ConnectionFailed;
            connmultiplexer.ErrorMessage += ErrorMessage;
            connmultiplexer.ConfigurationChanged += ConfigurationChanged;
            connmultiplexer.HashSlotMoved += HashSlotMoved;
            connmultiplexer.InternalError += InternalError;
            connmultiplexer.ConfigurationChangedBroadcast += ConfigurationChangedBroadcast;
        }
        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {

        }
        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {

        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ErrorMessage(object sender, RedisErrorEventArgs e)
        {

        }
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConfigurationChanged(object sender, EndPointEventArgs e)
        {

        }
        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {

        }
        /// <summary>
        /// 发生内部错误时（主要用于调试）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void InternalError(object sender, InternalErrorEventArgs e)
        {

        }
        /// <summary>
        /// 重新配置广播时（通常意味着主从同步更改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {

        }
        #endregion
    }
}
