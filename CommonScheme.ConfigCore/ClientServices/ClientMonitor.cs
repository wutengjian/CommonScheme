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
        }
        public static void AddConfig(string clinetCode)
        {
            if (MapConfigClient.ContainsKey(clinetCode) == false)
                MapConfigClient.Add(clinetCode, new ConcurrentQueue<string>());
        }
        public static void AddConfig(string clinetCode, string configKey)
        {
            AddConfig(clinetCode);
            MapConfigClient[clinetCode].Enqueue(configKey); ;
        }
        public static void Monitor()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 * 10);
                    if (num < 1 && MapConfigClient.Any(x => x.Value.Count > 0))
                        continue;
                    foreach (var clinetCode in MapConfigClient.Keys)
                    {
                        if (MapConfigClient[clinetCode].Count() <= 1)
                            continue;
                        Task.Factory.StartNew(new Action<object>(PushConfig), clinetCode);
                    }
                }
            });
        }
        private static void PushConfig(object data)
        {
            string clinetCode = (string)data;
            while (true)
            {
                string configKey = null;
                if (MapConfigClient[clinetCode].TryDequeue(out configKey) == false || configKey == null)
                    break;
                ConfigEntity config = new ConfigEntity();
                try
                {
                    ClientHttpModel client = new ClientHttpModel();
                    ClientFactory.GetInstace("HttpPushClient").Push(config);
                }
                catch (Exception ex) {
                    MapConfigClient[clinetCode].Enqueue(configKey);
                }
            }
        }
    }
}
