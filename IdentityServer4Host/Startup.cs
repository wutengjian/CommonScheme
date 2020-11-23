using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CommonScheme.NetCore;
using Autofac;

namespace IdentityServer4Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ServiceLocator.Configuration = Configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = "IdentityServer4Host";
            //string connectionString = @"Data Source=ZYXTJSTD-WUTJ\MSSQLSERVER12;Initial Catalog=IdentityServerDB;Persist Security Info=True;User ID=sa;Password=wutengjian123";
            var connectionString = Configuration.GetConnectionString("UserCenterConnection");
            services.AddMvc();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = builder => builder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                     //options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                 })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    //options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 300;
                })
            #region 内存方式
                // .AddInMemoryIdentityResources(ID4Config.GetIdentityResources())
                //.AddInMemoryApiResources(ID4Config.GetApis())
                //.AddInMemoryClients(ID4Config.GetClients())
                //.AddTestUsers(ID4Config.GetUsers())
            #endregion

            #region 数据库存储方式
                .AddClientStore<ClientStore>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()//账户密码方式验证
                .AddExtensionGrantValidator<WeiXinOpenGrantValidator>()//添加微信端自定义方式的验证
                ;
            #endregion
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<RegisterServices>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseRealIpExtensions();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseHostHeader();
            app.UseRouting();
            app.UseCors("any");
            app.UseExceptionHandle();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Welcome Use IdentityServer4Host."); });
            });
            ServiceLocator.Instance = app.ApplicationServices;
        }



    }
    public static class ServiceLocator
    {
        public static IConfiguration Configuration { get; set; }
        public static IServiceProvider Instance { get; set; }
        public static IContainer Container { get; set; }
    }
}
