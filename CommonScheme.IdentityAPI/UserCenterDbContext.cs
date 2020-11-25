using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonScheme.IdentityAPI
{
    public class UserCenterDbContext : DbContext
    {
        public UserCenterDbContext(DbContextOptions<UserCenterDbContext> options) : base(options) { }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ClientClaim> ClientClaims { get; set; }
        public DbSet<DeviceCode> DeviceCodes { get; set; }
        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        //public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }
        //public DbSet<ApiResourcePropertie> ApiResourceProperties { get; set; }
        //public DbSet<ApiResourceScope> ApiResourceScopes { get; set; }
        //public DbSet<ApiResourceSecret> ApiResourceSecrets { get; set; }
        //public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
        //public DbSet<ApiScopePropertie> ApiScopeProperties { get; set; }
        //public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        //public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        //public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        //public DbSet<ClientPostLoutRedirectUri> ClientPostLoutRedirectUris { get; set; }
        //public DbSet<ClientPropertie> ClientProperties { get; set; }
        //public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        //public DbSet<ClientScope> ClientScopes { get; set; }
        //public DbSet<ClientSecret> ClientSecrets { get; set; }
        //public DbSet<ResourceClaim> ResourceClaims { get; set; }
        //public DbSet<ResourcePropertie> ResourceProperties { get; set; }
    }
}
