using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommonScheme.NetCoreTest
{
    public class HttpCoreHepler
    {
        HttpClient client = null;
        IHttpClientFactory clientFactory = null;
        public HttpCoreHepler()
        {
            client = new HttpClient();
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }
        public HttpCoreHepler(HttpClient _client)
        {
            if (_client == null)
                client = new HttpClient();
            else
                client = _client;
        }
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await client.GetAsync(url);
        }
        public string Get(string url)
        {
            var _httpClient = clientFactory.CreateClient("CTCCMonitor");
            var response = _httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                if (t != null)
                    return t.Result;
            }
            return null;
        }
    }
}
