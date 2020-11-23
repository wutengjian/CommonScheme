using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSRedis;
using Newtonsoft.Json;

namespace CommonScheme.Net.Test
{
    public class CSRedisTest
    {
        static CSRedisClient csredis = null;
        public static void Run()
        {
            Init();
            //TestString();
            //TestList();
            TestSet();
            TestHash();
            Other();
            TestPubSub();
        }
        public static void Init()
        {
            var hosts = new string[] { "10.10.115.11:6380", "10.10.115.11:6381", "10.10.115.11:6382", "10.10.115.11:6390", "10.10.115.11:6391", "10.10.115.11:6392" };
            csredis = new CSRedis.CSRedisClient(null, hosts);
            RedisHelper.Initialization(csredis);
        }
        public static T GetOrSet<T>(string key, Func<T> action, TimeSpan ts)
        {
            var data = csredis.Get(key);
            if (data != null)
                return JsonConvert.DeserializeObject<T>(data);
            else
            {
                var result = action();
                csredis.SetAsync(key, JsonConvert.SerializeObject(result), ts);
                return result;
            }
        }
        public static void TestString()
        {
            csredis.Set("name", "wutengjian");
            RedisHelper.Set("keytime", "时间过期", 60);//1分钟失效 
            csredis.Append("name", "-123");
            csredis.MSet("key1", "1", "key2", "2");
            csredis.MGet("key1", "key2");
            csredis.Set("num-key", "55");
            csredis.IncrBy("num-key", -10);//结果：45
        }
        public static void TestList()
        {
            csredis.RPush("listkey", "value1", "value2", "value3", "value4");
            csredis.LSet("listkey", 1, "valueNew");
            foreach (var item in csredis.LRange("listkey", 0, -1))
            {
                Console.WriteLine(item);
            }
            csredis.RPop("listkey");
        }
        public static void TestSet()
        {
            csredis.SAdd("seta", "item1", "item2", "item3");
            csredis.SAdd("setb", "item2", "item3", "item4");
            #region 分区模式开启后无法使用
            //csredis.SDiff("seta", "setb");// 差集
            //csredis.SInter("seta", "setb");// 交集
            //csredis.SUnion("seta", "setb");// 并集
            #endregion

            foreach (var member in csredis.SMembers("seta"))
            {
                Console.WriteLine($"集合成员：{member.ToString()}");
            }
            csredis.SRem("setkey", "item2");
            //有序集合
            csredis.ZAdd("Quiz", (79, "Math"));
        }
        public static void TestHash()
        {
            csredis.HSet("hUserID:10001", "Title", "了解简单的Redis数据结构");
            csredis.HMSet("hUserID:1001", "Author", "Author", "PublishTime", "2019-01-01");
            csredis.HGet("ArticleID:10001", "Title");
            // 获取散列中的所有元素
            foreach (var item in csredis.HGetAll("ArticleID:10001"))
            {
                Console.WriteLine(item.Value);
            }
        }

        public static void Other()
        {
            var pl = csredis.StartPipe();//开启事务管道
            for (int i = 0; i < 10; i++)
            {
                csredis.IncrBy("key-key", 2);
            }
            pl.EndPipe();
            Console.WriteLine($"{csredis.Get("key-one")}");
            csredis.Expire("MyKey", 5); // key在5秒后过期，也可以使用ExpireAt方法让它在指定时间自动过期
        }
        /// <summary>
        /// 发布订阅
        /// </summary>
        public static void TestPubSub()
        {
            //势：支持多端订阅、简单、性能高
            //缺点：数据会丢失
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    csredis.Publish("chann1", "Data" + i);
                }
            });
            Task.Run(() =>
            {
                var sub = csredis.Subscribe(("chann1", msg => { Console.WriteLine(msg.Body); }));
                //  sub.Dispose();
            });
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    csredis.LPush("listchann1", "list1" + i);
                    csredis.LPush("listchann2", "list1" + i);
                }
            });
            //争抢
            csredis.SubscribeList("listchann1", msg => Console.WriteLine("sub1>>" + msg));
            csredis.SubscribeList("listchann1", msg => Console.WriteLine("sub1>>" + msg));
            //不争抢
            csredis.SubscribeListBroadcast("listchann2", "sub1", msg => Console.WriteLine("sub1>>" + msg));
            csredis.SubscribeListBroadcast("listchann2", "sub2", msg => Console.WriteLine("sub2>>" + msg));
        }
    }
}
