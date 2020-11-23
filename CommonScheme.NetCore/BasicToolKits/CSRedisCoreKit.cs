using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore.BasicToolKits
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
        public static void Initialization(string host)
        {
            var csredis = new CSRedis.CSRedisClient(host);
            RedisHelper.Initialization(csredis);
        }
        public static T Get<T>(string key)
        {
            return RedisHelper.Get<T>(key);
        }
        public static bool Set<T>(string key, T data, TimeSpan ts)
        {
            return RedisHelper.Set(key, data, ts);
        }
        public static bool Remove(string key)
        {
            return RedisHelper.Del(key) > 0;
        }
        public static bool SetTime(string key, TimeSpan ts)
        {
            return RedisHelper.Expire(key, ts);
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
