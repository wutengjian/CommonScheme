using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.NetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommonScheme.ConfigAPI
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string a = JsonConvert.SerializeObject(new ConfigCore.Models.RegistClientModel() { ID = 3, ClientState = 1, PushType = "Http", Config = new ConfigCore.Models.ConfigEntity() { ParentID = 0, Code = "123" } });
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            AppSettings.SetAppSetting("appConfig", configuration.GetSection("appConfig"));
            //AppSettings.SetAppSetting("Authentication", configuration.GetSection("Authentication"));
            AppSettings.SetAppSetting("ConnectionStrings", configuration.GetSection("ConnectionStrings"));
            return Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder = webBuilder
                       .ConfigureKestrel(serverOptions =>
                       {
                           serverOptions.AllowSynchronousIO = true;//启用同步 IO
                       });
                      webBuilder = webBuilder.UseStartup<Startup>();
                      HostDataInfo.Urls = AppSettings.GetAppSeting("appConfig:urls");

                      //方式二
                      webBuilder = webBuilder.UseUrls(HostDataInfo.Urls);
                  });
        }
    }
}
