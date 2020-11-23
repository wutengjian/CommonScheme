using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CommonScheme.NetCore;
using Autofac.Extensions.DependencyInjection;

namespace IdentityServer4Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            AppSettings.SetAppSetting("AppSettings", configuration.GetSection("AppSettings"));
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder = webBuilder
                       .ConfigureKestrel(serverOptions =>
                       {
                           serverOptions.AllowSynchronousIO = true;//启用同步 IO
                       });
                      //方式一
                      //.ConfigureAppConfiguration(builder =>
                      //{
                      //    builder.AddJsonFile("hosting.json", optional: true);
                      //})
                      webBuilder = webBuilder.UseStartup<Startup>();
                      HostDataInfo.Urls = AppSettings.GetAppSeting("AppSettings:urls");
                      //方式二
                      webBuilder = webBuilder.UseUrls(HostDataInfo.Urls);
                  });
        }
    }
}
