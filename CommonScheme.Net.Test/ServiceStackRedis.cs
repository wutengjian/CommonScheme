using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonScheme.Net.Test
{
    public class ServiceStackRedis
    {
        private static ServiceStackRedis redis;
        private static PooledRedisClientManager PooledClientManager { get; set; }
        private static IRedisClientsManager redisManager { get; set; }
        private static ICacheClient redisClient { get; set; }

        private static object oLock = new object();
        public static ServiceStackRedis Instence(string connStr)
        {
            if (redis == null)
            {
                lock (oLock)
                {
                    if (redis == null)
                    {
                        redis = new ServiceStackRedis();
                        if (connStr.Contains(";"))
                            Initialization(connStr.Split(';'));
                        else
                            Initialization(connStr, 0);
                    }
                }
            }
            return redis;
        }
        private static void Initialization(string connStr, int dbIndex = 0)
        {
            var config = new RedisClientManagerConfig
            {
                AutoStart = true,
                MaxWritePoolSize = 10,
                MaxReadPoolSize = 10,
                DefaultDb = 0
            };
            List<string> connList = new List<string>();
            connList.Add(connStr);
            PooledClientManager = new PooledRedisClientManager(connList, connList, config);
        }
        private static void Initialization(string[] hosts)
        {
            var sentinel = new RedisSentinel(hosts, "redis-master");
            sentinel.SentinelWorkerConnectTimeoutMs = 200;
            sentinel.SentinelWorkerReceiveTimeoutMs = 200;
            sentinel.SentinelWorkerSendTimeoutMs = 200;
            //sentinel.OnFailover = manager =>
            //   {
            //       Console.WriteLine("Redis Managers were Failed Over to new hosts");
            //   };
            //sentinel.OnWorkerError = ex =>
            //    {
            //        Console.WriteLine("Worker error: {0}", ex.Message);
            //    };
            //sentinel.OnSentinelMessageReceived = (channel, msg) =>
            //    {
            //        Console.WriteLine("Received '{0}' on channel '{1}' from Sentinel", channel, msg);
            //        if (msg.Contains("new reported role is master"))
            //        {
            //            Regex regex = new Regex(@"(?<info>\d+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //            redisClient = redisManager.GetCacheClient(); 
            //        }
            //    };
            redisManager = sentinel.Start();
            redisClient = redisManager.GetCacheClient();
        }

        public RedisClient GetRedisClient(bool isReadOnly = false)
        {
            RedisClient result;
            if (!isReadOnly)
            {
                //RedisClientManager.GetCacheClient()会返回一个新实例，而且只提供一小部分方法，它的作用是帮你判断是否用写实例还是读实例
                result = PooledClientManager.GetClient() as RedisClient;
            }
            else
            {
                //如果你读写是两个做了主从复制的Redis服务端，那么要考虑主从复制是否有延迟。有一些读操作是否是即时的，需要在写实例中获取。
                result = PooledClientManager.GetReadOnlyClient() as RedisClient;
            }
            //如果你的需求需要经常切换Redis数据库，则下一句可以用。否则一般都只用默认0数据库，集群是没有数据库的概念。
            //result.ChangeDb(Db);
            return result;
        }

        public bool SetValue(string key, string value, TimeSpan sp)
        {
            try
            {
                if (redisClient != null)
                    return redisClient.Set(key, value, sp);

                using (RedisClient redisClient = GetRedisClient())
                {
                    return redisClient.Set(key, value, sp);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Exceeded timeout of 00:00:10")
                {
                    redisClient = redisManager.GetCacheClient();
                }
            }
            return false;
        }

        public string GetValue(string key)
        {
            if (redisClient != null)
                return redisClient.Get<string>(key);
            using (RedisClient redisClient = GetRedisClient(true))
            {
                var val = redisClient.GetValue(key);
                return val;
            }
        }
    }
}
