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
        private static Dictionary<int, ConcurrentQueue<string>> MapConfigClient;
        private static Dictionary<int, ClientOptionModel> MapPushType;
        private static int num = 0;
        public static void Initialization()
        {
            MapConfigClient = new Dictionary<int, ConcurrentQueue<string>>();
            MapPushType = new Dictionary<int, ClientOptionModel>();
            //Monitor();
        }
        public static void RegisterConfig(int clientID)
        {
            if (MapConfigClient.ContainsKey(clientID))
                return;
            MapConfigClient.Add(clientID, new ConcurrentQueue<string>());
            MapPushType.Add(clientID, null);
        }
        public static void RegisterConfig(int clientID, string configKey)
        {
            RegisterConfig(clientID);
            MapConfigClient[clientID].Enqueue(configKey); ;
        }
        public static void CancelConfig(int clientID)
        {
            if (MapConfigClient.ContainsKey(clientID))
                MapConfigClient.Remove(clientID);
        }
        private static void Monitor()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 * 5);
                    if (num < 1 && MapConfigClient.Any(x => x.Value.Count > 0))
                        continue;
                    try
                    {
                        foreach (var clientID in MapConfigClient.Keys)
                        {
                            if (MapConfigClient[clientID].Count() <= 1)
                                continue;
                            if (MapPushType.ContainsKey(clientID) == false)
                                MapPushType.Add(clientID, null);
                            if (MapPushType[clientID] == null)
                            {
                                var option = DBFactory.GetModel<IDBClientDal>("IDBClientDal").GetClientOption(clientID);
                                MapPushType[clientID] = option;
                            }
                            Task.Factory.StartNew(new Action<object>(PushConfig), clientID);
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
            int clientID = (int)data;
            while (true)
            {
                string configKey = null;
                if (MapConfigClient.ContainsKey(clientID) == false || MapConfigClient[clientID].TryDequeue(out configKey) == false || configKey == null)
                    break;
                ConfigEntity config = new ConfigEntity();
                try
                {
                    num++;
                    ClientOptionModel client = MapPushType[clientID];
                    if(client.PushType== "Http")
                    ClientFactory.GetInstace("HttpPushClient").Push(client,config);
                    else if(client.PushType== "RabbitMQ")
                        ClientFactory.GetInstace("RabbitMQPushClient").Push(client,config);
                }
                catch (Exception ex)
                {
                    try { MapConfigClient[clientID].Enqueue(configKey); }
                    finally { }
                }
                finally { num--; }
            }
        }
    }
}
