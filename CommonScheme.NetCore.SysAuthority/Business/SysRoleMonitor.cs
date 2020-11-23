using CommonScheme.NetCore.SysAuthority.ViewModels;
using CommonScheme.NetCore.SysAuthority.Entitys;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore.SysAuthority.Business
{
    public class SysRoleMonitor
    {
        public void AddRole(SysRoleInfo model)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                var user = db.sysRoles.Add(model.RoleInfo);
                if (model.ResourceIDs.Length > 0)
                    AddRoleResources(user.Entity.RoleID, model.ResourceIDs, db);
                db.SaveChanges();
            }
        }
        public void UpdateRole(SysRoleBase model)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                var user = db.sysRoles.Update(model);
                db.SaveChanges();
            }
        }
        public void AddRoleResources(int RoleId, int[] resourceIds, AuthorityDBContext db = null)
        {
            if (db == null)
                db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>();
            foreach (var resourceId in resourceIds)
                db.roleResources.Add(new RoleResource() { RoleID = RoleId, ResourceID = resourceId });
            db.SaveChanges();
        }
        public void AddRoleResource(int RoleId, int resourceId, AuthorityDBContext db = null)
        {
            if (db == null)
                db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>();
            db.roleResources.Add(new RoleResource() { RoleID = RoleId, ResourceID = resourceId });
            db.SaveChanges();
        }
    }
}
