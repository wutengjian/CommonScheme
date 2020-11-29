using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommonScheme.ConfigCore.ClientServices
{
    public class HttpPushClient : ClientServiceBase
    {
        public override void Push(ClientOptionModel client, ConfigEntity entity)
        {
            Dictionary<string, string> headers = null;
            if (string.IsNullOrEmpty(client.Headers) == false)
                JsonConvert.DeserializeObject<Dictionary<string, string>>(client.Headers);
            PostAsync(client.Url, JsonConvert.SerializeObject(entity), headers);
        }

        private async Task<string> GetAsync(string url)
        {
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Method", "Get");
                httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");

                HttpResponseMessage response = await httpClient.GetAsync(url);
                // var response = await httpClient.GetStringAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            return responseBody;
        }

        private async Task<string> PostAsync(string url, Dictionary<string, string> postParams)
        {
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");
                HttpContent postContent = new FormUrlEncodedContent(postParams);
                HttpResponseMessage response = await httpClient.PostAsync(url, postContent);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            return responseBody;
        }
        private async Task<string> PostAsync(string url, string jsonData, Dictionary<string, string> headers = null)
        {
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");
                HttpContent postContent = new StringContent(jsonData);
                postContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, postContent);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            return responseBody;
        }

    }
}
