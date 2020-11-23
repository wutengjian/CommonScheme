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
        private static Dictionary<string, ConcurrentQueue<string>> MapConfigClient;
        private static int num = 0;
        public static void Initialization()
        {
            MapConfigClient = new Dictionary<string, ConcurrentQueue<string>>();
            Monitor();
        }
        public static void RegisterConfig(string clientCode)
        {
            if (MapConfigClient.ContainsKey(clientCode) == false)
                MapConfigClient.Add(clientCode, new ConcurrentQueue<string>());
        }
        public static void RegisterConfig(string clientCode, string configKey)
        {
            RegisterConfig(clientCode);
            MapConfigClient[clientCode].Enqueue(configKey); ;
        }
        public static void CancelConfig(string clientCode)
        {
            if (MapConfigClient.ContainsKey(clientCode))
                MapConfigClient.Remove(clientCode);
        }
        private static void Monitor()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 * 10);
                    if (num < 1 && MapConfigClient.Any(x => x.Value.Count > 0))
                        continue;
                    try
                    {
                        foreach (var clientCode in MapConfigClient.Keys)
                        {
                            if (MapConfigClient[clientCode].Count() <= 1)
                                continue;
                            Task.Factory.StartNew(new Action<object>(PushConfig), clientCode);
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
            string clientCode = (string)data;
            while (true)
            {
                string configKey = null;
                if (MapConfigClient.ContainsKey(clientCode) == false || MapConfigClient[clientCode].TryDequeue(out configKey) == false || configKey == null)
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
                    try { MapConfigClient[clientCode].Enqueue(configKey); }
                    finally { }
                }
                finally { num--; }
            }
        }
    }
}
