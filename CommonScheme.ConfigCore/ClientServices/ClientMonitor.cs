using System;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonScheme.ConfigCore.Models;
using System.Collections.Generic;

namespace CommonScheme.ConfigCore.ClientServices
{
    public class ClientMonitor
    {
        private static Dictionary<int, ConcurrentQueue<string>> MapConfigClient;
        private static int num = 0;
        public static void Initialization()
        {
            MapConfigClient = new Dictionary<int, ConcurrentQueue<string>>();
            Monitor();
        }
        public static void RegisterConfig(int clientID)
        {
            if (MapConfigClient.ContainsKey(clientID) == false)
                MapConfigClient.Add(clientID, new ConcurrentQueue<string>());
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
                    ClientHttpModel client = new ClientHttpModel();
                    ClientFactory.GetInstace("HttpPushClient").Push(config);
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
