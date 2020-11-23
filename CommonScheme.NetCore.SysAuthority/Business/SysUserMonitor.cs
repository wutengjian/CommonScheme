using CommonScheme.NetCore.SysAuthority.Entitys;
using CommonScheme.NetCore.SysAuthority.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore.SysAuthority.Business
{
    public class SysUserMonitor
    {
        public void RegisterUser(SysUserInfo model)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                var user = db.sysUsers.Add(model.UserInfo);
                if (model.RoleIDs.Length > 0)
                    SignUserRoles(user.Entity.UserID, model.RoleIDs, db);
                db.SaveChanges();
            }
        }
        public void UpdateUser(SysUserBase model)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                var user = db.sysUsers.Update(model);
                db.SaveChanges();
            }
        }
        public void SignUserRoles(long userID, int[] roleIds, AuthorityDBContext db = null)
        {
            if (db == null)
                db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>();
            foreach (var roleId in roleIds)
                db.userRoles.Add(new UserRole() { UserID = userID, RoleID = roleId });
            db.SaveChanges();
        }
        public void SignUserRole(long userID, int roleId, AuthorityDBContext db = null)
        {
            if (db == null)
                db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>();
            db.userRoles.Add(new UserRole() { UserID = userID, RoleID = roleId });
            db.SaveChanges();
        }
        public void RelieveUserRoles(long userID, int[] roleIds)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                foreach (var roleId in roleIds)
                    db.userRoles.Update(new UserRole() { RoleID = roleId, UserID = userID });
                db.SaveChanges();
            }
        }
        public void RelieveUserRole(long userID, int roleID)
        {
            using (var db = ServiceLocator.Instance.GetRequiredService<AuthorityDBContext>())
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@UserId", userID));
                parms.Add(new SqlParameter("@RoleID", roleID));
                db.Database.ExecuteSqlRaw("update sysUserRole set DataState=1,UpdateUserID=0,UpdateTime=GetDate() where UserId=@UserId and RoleID=@RoleID;", parms.ToArray());
            }
        }
    }
}
