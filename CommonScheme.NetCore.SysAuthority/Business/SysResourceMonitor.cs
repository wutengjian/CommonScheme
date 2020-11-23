using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore.SysAuthority.Business
{
    public class SysResourceMonitor
    {
        public void AddResource(Entitys.SysResourceBase model)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                var user = db.sysResources.Add(model);
                db.SaveChanges();
            }
        }
        public void UpdateResource(Entitys.SysResourceBase model)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                var user = db.sysResources.Update(model);
                db.SaveChanges();
            }
        }
    }
}
