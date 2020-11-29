using System.IO;
using CommonScheme.NetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CommonScheme.ConfigAPI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            AppSettings.UseAppSetting(builder);
            //string a = JsonConvert.SerializeObject(new ConfigCore.Models.RegistClientModel() { ID = 3, ClientState = 1, PushType = "Http", Config = new ConfigCore.Models.ConfigEntity() { ParentID = 0, Code = "123" } });
            NetCore.RabbitMQSimple.RabbitMQSimpleKit.Initialization();
            ServicesFactory.MapFactory();
            MemoryCacheKit.Initialization(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
            var host = CreateWebHostBuilder(args, builder.Build()).Build();
            host.Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration)
        {
            return Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder = webBuilder
                       .ConfigureKestrel(serverOptions =>
                       {
                           serverOptions.AllowSynchronousIO = true;//启用同步 IO
                       });
                      webBuilder = webBuilder.UseStartup<Startup>();
                      //方式二
                      webBuilder = webBuilder.UseUrls(AppSettings.GetAppSeting("AppConfigs:url"));
                  });
        }
    }
}
