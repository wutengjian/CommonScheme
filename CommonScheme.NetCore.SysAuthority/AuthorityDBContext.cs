using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CommonScheme.NetCore.SysAuthority.Entitys;


namespace CommonScheme.NetCore.SysAuthority
{
    public class AuthorityDBContext : DbContext
    {
        public AuthorityDBContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=ZYXTJSTD-WUTJ\MSSQLSERVER12;Initial Catalog=SysAuthorityDB;Persist Security Info=True;User ID=sa;Password=wutengjian123");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<SysUserBase> sysUsers { get; set; }
        public DbSet<SysRoleBase> sysRoles { get; set; }
        public DbSet<SysResourceBase> sysResources { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<RoleResource> roleResources { get; set; }

    }
}
