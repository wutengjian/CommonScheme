using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommonScheme.NetCore.SysAuthority
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            AppSettings.SetAppSetting("appConfig", configuration.GetSection("appConfig"));
            //AppSettings.SetAppSetting("Authentication", configuration.GetSection("Authentication"));
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
