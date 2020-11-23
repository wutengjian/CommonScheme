using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommonScheme.NetCore.SysAuthority
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureJwt(Configuration);//≈‰÷√jwt
            services.Configure<JwtConfig>(Configuration.GetSection("Authentication:JwtBearer"));//◊¢»ÎJWT≈‰÷√Œƒº˛
            //services.AddDbContext<AuthorityDBContext>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetRequiredService<AuthorityDBContext>();
            //    context.Database.SetCommandTimeout(180);
            //    context.Database.EnsureCreated();
            //    context.Database.Migrate();
            //    new InitDataSysAuthority().BuildData(app);
            //}
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("any");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Welcome Use SysAuthority."); });
            });
            ServiceLocator.Instance = app.ApplicationServices;
        }
    }

    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }
}
