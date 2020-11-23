using System;
using System.Collections.Generic;
using System.Text;
using CSRedis;

namespace CommonScheme.Net
{
    public class CSRedisCoreKit
    {
        public static void Initialization(string SentinelName, string[] hosts)
        {
            var csredis = new CSRedis.CSRedisClient(SentinelName, hosts);
            RedisHelper.Initialization(csredis);
        }
        public static void Initialization(string[] hosts)
        {
            var csredis = new CSRedis.CSRedisClient(null, hosts);
            RedisHelper.Initialization(csredis);
        }
        public static T GetOrSet<T>(string key, Func<T> func, TimeSpan expiresIn)
        {
            var t = RedisHelper.Get<T>(key);
            if (t == null)
            {
                t = func();
                RedisHelper.SetAsync(key, t, expiresIn);
            }
            return t;
        }
        public static T HGetOrSet<T>(string key, string field, Func<T> func)
        {
            var t = RedisHelper.HGet<T>(key, field);
            if (t == null)
            {
                t = func();
                RedisHelper.HSet(key, field, t);
            }
            return t;
        }
    }
}
