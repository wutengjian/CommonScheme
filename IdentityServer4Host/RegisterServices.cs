using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Host
{
    public class RegisterServices : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //InstancePerLifetimeScope  (每个生命周期作用域)
            //InstancePerMatchingLifetimeScope （每个匹配生命周期作用域）
            //InstancePerOwned （每次被拥有的一个实例）

            //builder.RegisterType<UserCenterDbContext>().As<DbContext>().InstancePerDependency(); //瞬时的
            //builder.RegisterType<UserCenterDbContext>().As<DbContext>().InstancePerLifetimeScope();  //每个生命周期作用域单例
            //builder.RegisterType<UserCenterDbContext>().As<DbContext>().SingleInstance();    //全局单例 
            //ServiceLocator.Container = builder.Build();
        }
    }
}
