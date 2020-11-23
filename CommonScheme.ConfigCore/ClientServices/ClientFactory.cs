using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.ClientServices
{
    public class ClientFactory
    {
        private static Dictionary<string, IClientService> _mapClient;
        public static void SetClient(string key, IClientService client)
        {
            if (_mapClient == null)
                _mapClient = new Dictionary<string, IClientService>();
            if (_mapClient.ContainsKey(key) == false)
                _mapClient.Add(key, client);
            else
                _mapClient[key] = client;
        }
        public static IClientService GetInstace(string key)
        {
            return _mapClient[key];
        }
    }
}
