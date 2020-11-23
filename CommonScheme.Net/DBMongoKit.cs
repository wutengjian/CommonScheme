using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonScheme.Net
{
    public class DBMongoKit
    {
        private static DBMongoKit mongoKit = null;
        private static readonly object lockobject = new object();
        private MongoClient mongoClient { get; set; }
        private IMongoDatabase db { get; set; }
        private Dictionary<string, IMongoCollection<BsonDocument>> collections;

        private IEnumerable<BsonDocument> documents { get; set; }

        private DBMongoKit(DBMongoConfig config, string tableName)
        {
            if (config == null)
            {
                //mongodb://Jianny:a123456@localhost/CommonSchemeCoreLog
                config = new DBMongoConfig() { Host = "10.10.111.28:27017", UserName = "Jianny", Password = "a123456", DBName = "CommonSchemeCoreLog" };
            }
            string connStr = string.Format("mongodb://{0}:{1}@{2}/{3}", config.UserName, config.Password, config.Host, config.DBName);
            mongoClient = new MongoClient(connStr);
            db = mongoClient.GetDatabase(config.DBName);
            collections = new Dictionary<string, IMongoCollection<BsonDocument>>();
            //collection = db.GetCollection<BsonDocument>(tableName);
        }
        public static DBMongoKit GetMongoDBInstance(DBMongoConfig config)
        {
            if (mongoKit == null)
            {
                lock (nameof(DBMongoKit))// lockobject)
                {
                    if (mongoKit == null)
                    {
                        mongoKit = new DBMongoKit(config, null);
                    }
                }
            }

            return mongoKit;
        }

        /// <summary>
        /// 同步插入数据
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool InsertOneData<T>(T value, string tableName)
        {
            try
            {
                getMongoCollection(tableName);
                collections[tableName].InsertOne(value.ToBsonDocument());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsyncOneData<T>(T value, string tableName)
        {
            try
            {
                getMongoCollection(tableName);
                await collections[tableName].InsertOneAsync(value.ToBsonDocument());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 同步插入多条数据
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        public bool InsertManyData<T>(IEnumerable<T> values, string tableName)
        {
            try
            {
                getMongoCollection(tableName);
                List<BsonDocument> data = new List<BsonDocument>();
                foreach (var value in values)
                {
                    data.Add(value.ToBsonDocument());
                }
                collections[tableName].InsertMany(data);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 同步插入多条数据
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsyncManyData<T>(IEnumerable<T> values, string tableName)
        {
            try
            {
                getMongoCollection(tableName);
                List<BsonDocument> data = new List<BsonDocument>();
                foreach (var value in values)
                {
                    data.Add(value.ToBsonDocument());
                }
                await collections[tableName].InsertManyAsync(documents);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 查找有数据。
        /// </summary>
        /// <returns></returns>
        public List<T> FindData<T>(string tableName)
        {
            getMongoCollection(tableName);
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            var datas = collections[tableName].Find(new BsonDocument()).Project(projection).ToList();
            if (datas == null || datas.Count < 1)
                return null;

            List<T> list = new List<T>(datas.Count);
            foreach (var data in datas)
            {
                list.Add(BsonSerializer.Deserialize<T>(data));
            }
            return list;
        }

        /// <summary>
        /// 取排除_id字段以外的数据。然后转换成泛型。
        /// </summary>
        /// <returns></returns>
        public List<T> FindAnsyncData<T>(string tableName)
        {
            getMongoCollection(tableName);
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            var datas = collections[tableName].Find(new BsonDocument()).Project(projection).ToListAsync().Result;
            if (datas == null || datas.Count < 1)
                return null;

            List<T> list = new List<T>(datas.Count);
            foreach (var data in datas)
            {
                list.Add(BsonSerializer.Deserialize<T>(data));
            }
            return list;
        }

        /// <summary>
        /// 按某些列条件查询
        /// </summary>
        /// <param name="bson"></param>
        /// <returns></returns>
        public List<T> FindFilterlData<T>(BsonDocument bson, string tableName)
        {
            getMongoCollection(tableName);
            var buildfilter = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = null;
            foreach (var bs in bson)
            {
                filter = buildfilter.Eq(bs.Name, bs.Value);
            }
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            var datas = collections[tableName].Find(filter).Project(projection).ToList();
            if (datas == null || datas.Count < 1)
                return null;

            List<T> list = new List<T>(datas.Count);
            foreach (var data in datas)
            {
                list.Add(BsonSerializer.Deserialize<T>(data));
            }
            return list;
        }


        /// <summary>
        /// 返回受影响行
        /// </summary>
        /// <returns></returns>
        public long DeleteData(string tableName)
        {
            getMongoCollection(tableName);
            //删除count大于0的文档。
            var filter = Builders<BsonDocument>.Filter.Gt("count", 0);
            DeleteResult deleteResult = collections[tableName].DeleteMany(filter);
            return deleteResult.DeletedCount;
        }

        /// <summary>
        /// 根据id更新文档中单条数据。
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="bson"></param>
        public UpdateResult UpdateOneData(string _id, BsonDocument bson, string tableName)
        {
            getMongoCollection(tableName);
            //修改条件（相当于sql where）
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("name", "MongoDB");
            UpdateDefinition<BsonDocument> update = null;
            foreach (var bs in bson)
            {
                if (bs.Name.Equals("name"))
                {
                    update = Builders<BsonDocument>.Update.Set(bs.Name, bs.Value);
                }
            }
            //UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("name", bson[0].ToString());
            UpdateResult result = collections[tableName].UpdateOne(filter, update);//默认更新第一条。
            return result;
        }

        private bool getMongoCollection(string tableName)
        {
            if (collections.ContainsKey(tableName) == false)
            {
                var collection = db.GetCollection<BsonDocument>(tableName);
                collections.Add(tableName, collection);
                return true;
            }
            return true;
        }
    }
    public class DBMongoConfig
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DBName { get; set; }
    }
    public abstract class MongoEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDelete { get; set; }
        public MongoEntity()
        {
            CreateTime = DateTime.Now;
            IsDelete = false;
        }
    }
    public class NLog : MongoEntity
    {
        public string Code { get; set; }
        public string Modular { get; set; }
        public int LogType { get; set; }
        public string Context { get; set; }
        public DateTime LogTime { get; set; }
        public NLog()
        {
            LogTime = DateTime.Now;
        }
    }
}
