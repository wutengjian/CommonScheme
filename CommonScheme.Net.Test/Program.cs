using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommonScheme.Net.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            new RabbitMQSimpleTest().Run();
            //CSRedisTest.Run();
            while (true)
            {
                if (Console.ReadLine() == "exit")
                    break;
            }
        }
        //#region rabbitMQ
        //public static void TestRabbitMQ()
        //{
        //    MQConfig config = new MQConfig() { HostID = 1, HostUrl = "10.10.115.11", Port = 5672, VirtualHost = "CommonScheme", UserName = "CommonScheme", Password = "CommonScheme" };
        //    RabbitMQKit.CreateConnection(config);
        //    MQInfo mq = new MQInfo() { HostID = 1, ExchangeName = "CommonSchemeExchange", ExchangeType = "topic", QueueName = "CommonSchemeQueue" };
        //    ClineInfo cline = new ClineInfo() { ClineID = 1, ClineType = 1, Name = "CommonScheme", ProductCode = "CommonScheme", Position = "CommonScheme", Remake = "" };
        //    RabbitMQKit.RegisterCline(mq, cline);
        //    cline = new ClineInfo() { ClineID = 2, ClineType = -1, Name = "CommonScheme", ProductCode = "CommonScheme", Position = "CommonScheme", Remake = "" };
        //    RabbitMQKit.RegisterCline(mq, cline);
        //    ProductionMQ(1);
        //    ConsumerMQ(2);
        //}
        //private static void ProductionMQ(int clineID)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        RabbitMQKit.BuildProduction(clineID, Guid.NewGuid().ToString());
        //    }
        //}
        //private static void ConsumerMQ(int clineID)
        //{
        //    RabbitMQKit.BuildConsume(clineID, (data) =>
        //    {
        //        Console.WriteLine(data);
        //    });
        //}
        //#endregion
        //#region Redis
        //public static void TestRedis()
        //{
        //    var hosts = new string[] { "10.10.115.11:6380", "10.10.115.11:6381", "10.10.115.11:6382", "10.10.115.11:6390", "10.10.115.11:6391", "10.10.115.11:6392" };
        //    CSRedisCoreKit.Initialization(hosts);
        //    var data = CSRedisCoreKit.GetOrSet("Jianny", () => { return "wutengjian"; }, TimeSpan.FromDays(1));
        //    Console.WriteLine(data);

        //    CSRedisCoreKit.Initialization("10.10.111.28:8989", null);
        //    data = CSRedisCoreKit.GetOrSet("Jianny", () => { return "wutengjian"; }, TimeSpan.FromDays(1));
        //    Console.WriteLine(data);

        //    hosts = new string[] { "10.10.111.28:8989" };
        //    StackExchangeRedisKit.Initialization(hosts);
        //    StackExchangeRedisKit.GetOrSetString("Jianny", () => { return "wutengjian"; }, TimeSpan.FromDays(1));
        //    Console.WriteLine(data);
        //}

        //public static void TestServiceStackRedis()
        //{
        //    var redis = ServiceStackRedis.Instence("10.10.111.28:28880;10.10.111.28:28881;10.10.111.28:28882");
        //    for (int i = 0; i < 100; i++)
        //    {
        //        string keydata = Guid.NewGuid().ToString();
        //        redis.SetValue(keydata, keydata, TimeSpan.FromDays(1));
        //        Console.WriteLine(redis.GetValue(keydata));
        //    }
        //}
        //#endregion
        //#region MongoDB
        //public static void TestMongoDB()
        //{
        //    var mongoKit = DBMongoKit.GetMongoDBInstance(null);
        //    var log = new NLog() { Code = "业务标识", Modular = "模块", LogType = 1, Context = "日志内容" };
        //    mongoKit.InsertOneData<NLog>(log, "NLog");
        //    var data = mongoKit.FindData<NLog>("NLog");
        //    Console.WriteLine(JsonConvert.SerializeObject(data));
        //}
        //#endregion

        //#region DB
        //public static void TestSQLite()
        //{
        //    DBSQLiteKit.Instance(@"D:\data\sqliteData.db");
        //    string sql = "CREATE table NLog(Code varchar(50),Modular varchar(50),LogType int,Context varchar(500),LogTime DATETIME,CreateTime DATETIME,IsDelete bool)";
        //    // DBSQLiteKit.CreateTable(sql);
        //    sql = "insert into NLog(Code,Modular,LogType,Context,LogTime,CreateTime,IsDelete) values(@Code,@Modular,@LogType,@Context,DATETIME(),DATETIME(),@IsDelete)";
        //    Dictionary<string, object> dic = new Dictionary<string, object>();
        //    dic.Add("@Code", "业务标识");
        //    dic.Add("@Modular", "模块");
        //    dic.Add("@LogType", 1);
        //    dic.Add("@Context", "日志内容");
        //    dic.Add("@IsDelete", false);
        //    DBSQLiteKit.Execute(sql, dic);
        //}
        //#endregion
    }
}
