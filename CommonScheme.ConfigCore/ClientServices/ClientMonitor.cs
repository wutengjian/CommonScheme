using System;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonScheme.ConfigCore.Models;
using System.Collections.Generic;
using CommonScheme.ConfigCore.DBStorages;

namespace CommonScheme.ConfigCore.ClientServices
{
    public class ClientMonitor
    {
        /// <summary>
        /// 注册的配置。键：configID，值：clientID
        /// </summary>
        private static Dictionary<int, List<int>> mapConfig;
        /// <summary>
        /// 注册的客户端。键：clientID，值：configID
        /// </summary>
        private static Dictionary<int, List<int>> mapClient;
        /// <summary>
        /// 带推送的队列。键：configID，值：clientID
        /// </summary>
        private static Dictionary<int, ConcurrentQueue<int>> mapPush;
        /// <summary>
        /// 推送的实际值
        /// </summary>
        private static ConcurrentDictionary<int, ConfigEntity> pushData;
        private static Dictionary<int, ClientOptionModel> MapPushType;
        private static int num = 0;
        public static void Initialization()
        {
            mapConfig = new Dictionary<int, List<int>>();
            mapClient = new Dictionary<int, List<int>>();
            mapPush = new Dictionary<int, ConcurrentQueue<int>>();
            pushData = new ConcurrentDictionary<int, ConfigEntity>();
            MapPushType = new Dictionary<int, ClientOptionModel>();
            Monitor();
        }
        /// <summary>
        /// 增加、修改推送配置项。维护mapConfig、mapClient、mapPush、pushData
        /// </summary>
        /// <param name="entity"></param>
        public static void SetConfig(ConfigEntity entity)
        {
            if (entity == null || entity.ID < 1) return;
            if (pushData.ContainsKey(entity.ID))
            {
                pushData[entity.ID] = entity;
                foreach (var clientID in mapConfig[entity.ID])
                {
                    mapPush[entity.ID].Enqueue(clientID);
                }
                return;
            }
            //首次添加config
            pushData.TryAdd(entity.ID, entity);
            if (mapConfig.ContainsKey(entity.ID) == false)
                mapConfig.Add(entity.ID, new List<int>());
            var arr = mapClient.Where(x => x.Value.Contains(entity.ID)).Select(x => x.Key).ToArray();
            if (arr == null || arr.Length < 1)
                return;
            mapConfig[entity.ID].AddRange(arr);
            if (mapPush.ContainsKey(entity.ID) == false)
                mapPush.Add(entity.ID, new ConcurrentQueue<int>());
            foreach (var clientID in arr)
            {
                mapPush[entity.ID].Enqueue(clientID);//向推送队列增加需要推送的客户端
            }
        }
        /// <summary>
        /// 注册客户端。维护mapClient、mapConfig、mapPush
        /// </summary>
        /// <param name="clientID">客户端ID</param>
        /// <param name="configID">配置项ID</param>
        public static void RegisterClient(int clientID, int configID = 0)
        {
            if (clientID < 1)
                return;
            if (mapClient.ContainsKey(clientID) == false)
                mapClient.Add(clientID, new List<int>());
            if (configID < 1)
                return;
            if (mapClient[clientID].Contains(configID) == false)
                mapClient[clientID].Add(configID);
            if (mapConfig.ContainsKey(configID) == false)
                mapConfig.Add(configID, new List<int>());
            //if (mapConfig[configID].Contains(clientID) == false)
            //    mapConfig[configID].Add(clientID);
        }
        public static void RemoveConfig(int configID) { }
        public static void CancelClient(int clientID) { }
        private static void Monitor()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 * 5);
                    if (num > 0 || mapPush.Any(x => x.Value.Count > 0) == false)
                        continue;
                    try
                    {
                        foreach (var clientID in mapClient.Keys)
                        {
                            if (MapPushType.ContainsKey(clientID) == false)
                                MapPushType.Add(clientID, null);
                            if (MapPushType[clientID] == null)
                            {
                                var option = DBFactory.GetModel<IDBClientDal>("IDBClientDal").GetClientOption(clientID);
                                MapPushType[clientID] = option;
                            }
                        }
                        foreach (var configID in mapPush.Keys)
                        {
                            if (mapPush[configID].Count() < 1)
                                continue;
                            Task.Factory.StartNew(new Action<object>(PushConfig), configID);
                        }
                    }
                    catch (Exception ex)
                    {
                        num = 0;
                    }
                }
            });
        }
        private static void PushConfig(object data)
        {
            int configID = (int)data;
            while (true)
            {
                int clientID = 0;
                if (mapPush.ContainsKey(configID) == false || mapPush[configID].TryDequeue(out clientID) == false || clientID < 1)
                    break;
                if (MapPushType.ContainsKey(clientID) == false)
                    continue;
                try
                {
                    num++;
                    ClientOptionModel client = MapPushType[clientID];
                    if (client.PushType == "Http")
                        ClientFactory.GetInstace("HttpPushClient").Push(client, pushData[configID]);
                    else if (client.PushType == "RabbitMQ")
                        ClientFactory.GetInstace("RabbitMQPushClient").Push(client, pushData[configID]);
                }
                catch (Exception ex)
                {
                    try { mapPush[clientID].Enqueue(configID); }
                    finally { }
                }
                finally { num--; }
            }
        }
    }
}
