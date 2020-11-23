using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.NetCore.SysAuthority.Entitys;

namespace CommonScheme.NetCore.SysAuthority
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public class InitDataSysAuthority
    {
        public void BuildData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AuthorityDBContext>();
                context.Database.SetCommandTimeout(180);
                MadeResource(context);
                MadeRoles(context);
                MadeUsers(context);
                MadeRoleResource(context);
                MadeUserRole(context);
            }
        }
        private void MadeResource(AuthorityDBContext context)
        {
            if (context.sysResources.Any())
                return;
            List<SysResourceBase> list = new List<SysResourceBase>();
            list.Add(new SysResourceBase() { ResourceType = 2, ResourceName = "http://10.10.115.11:8181" });
            list.Add(new SysResourceBase() { ResourceType = 2, ResourceName = "http://10.10.115.11:8181" });
            list.Add(new SysResourceBase() { ResourceType = 2, ResourceName = "http://10.10.115.11:8181" });
            list.Add(new SysResourceBase() { ResourceType = 2, ResourceName = "http://10.10.115.11:8181" });
            context.sysResources.AddRange(list);
            context.SaveChanges();
        }
        private void MadeRoles(AuthorityDBContext context)
        {
            if (context.sysRoles.Any())
                return;
            List<SysRoleBase> list = new List<SysRoleBase>();
            list.Add(new SysRoleBase() { RoleName = "超级管理员" });
            list.Add(new SysRoleBase() { RoleName = "AAA" });
            list.Add(new SysRoleBase() { RoleName = "BBB" });
            context.sysRoles.AddRange(list);
            context.SaveChanges();
        }
        private void MadeUsers(AuthorityDBContext context)
        {
            if (context.sysUsers.Any())
                return;
            List<SysUserBase> list = new List<SysUserBase>();
            list.Add(new SysUserBase() { UserName = "admin", Password = "123456" });
            list.Add(new SysUserBase() { UserName = "Jianny", Password = "123456" });
            list.Add(new SysUserBase() { UserName = "adminApi", Password = "123456" });
            context.sysUsers.AddRange(list);
            context.SaveChanges();
        }
        private void MadeRoleResource(AuthorityDBContext context)
        {
            if (context.roleResources.Any())
                return;
            List<RoleResource> list = new List<RoleResource>();
            list.Add(new RoleResource() { RoleID = 1, ResourceID = 1 });
            list.Add(new RoleResource() { RoleID = 1, ResourceID = 2 });
            list.Add(new RoleResource() { RoleID = 1, ResourceID = 3 });
            list.Add(new RoleResource() { RoleID = 2, ResourceID = 1 });
            list.Add(new RoleResource() { RoleID = 2, ResourceID = 2 });
            list.Add(new RoleResource() { RoleID = 2, ResourceID = 4 });
            list.Add(new RoleResource() { RoleID = 3, ResourceID = 2 });
            list.Add(new RoleResource() { RoleID = 3, ResourceID = 3 });
            list.Add(new RoleResource() { RoleID = 3, ResourceID = 4 });
            context.roleResources.AddRange(list);
            context.SaveChanges();
        }
        private void MadeUserRole(AuthorityDBContext context)
        {
            if (context.userRoles.Any())
                return;
            List<UserRole> list = new List<UserRole>();
            list.Add(new UserRole() { UserID = 1, RoleID = 1 });
            list.Add(new UserRole() { UserID = 1, RoleID = 2 });
            list.Add(new UserRole() { UserID = 2, RoleID = 1 });
            list.Add(new UserRole() { UserID = 2, RoleID = 3 });
            list.Add(new UserRole() { UserID = 3, RoleID = 2 });
            list.Add(new UserRole() { UserID = 3, RoleID = 3 });
            context.userRoles.AddRange(list);
            context.SaveChanges();
        }
    }
}
