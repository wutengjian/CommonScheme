using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using System.Security.Claims;
using IdentityServer4.Test;
using IdentityModel;
using IdentityServer4;

namespace CommonScheme.IdentityAPI
{
    public class ID4Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()           {
               ClientId =OAuthConfig.UserApiClientId,
               AllowedGrantTypes = new List<string>()
               {
                  GrantTypes.ResourceOwnerPassword.FirstOrDefault(),//Resource Owner Password模式
                  //GrantTypes.HybridAndClientCredentials.FirstOrDefault(),                  
                  //GrantTypeConstants.ResourceWeixinOpen,//新增的自定义微信客户端的授权模式
               },
               ClientSecrets = {new Secret(OAuthConfig.UserApiSecret.Sha256()) },
               PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
               AllowedScopes= {
                        //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                        OAuthConfig.UserApiName,
                        IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
               AccessTokenLifetime = OAuthConfig.ExpireIn,
               AllowOfflineAccess=true,//如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true 
           },
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",
                  Claims = new List<Claim>(){new Claim(IdentityModel.JwtClaimTypes.Role,"superadmin") }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    },
                }
            };
        }
        /// <summary>
        /// 为了演示，硬编码了，
        /// 这个方法可以通过DDD设计到底层数据库去查询数据库
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static UserModel GetUserByUserName(string userName)
        {
            var normalUser = new UserModel()
            {
                DisplayName = "张三",
                ProviderId = 10001,
                Password = "123456",
                Role = EnumUserRole.Normal,
                SubjectId = "1",
                UserId = 20001,
                UserName = "testNormal"
            };
            var manageUser = new UserModel()
            {
                DisplayName = "李四",
                ProviderId = 10001,
                Password = "123456",
                Role = EnumUserRole.Manage,
                SubjectId = "1",
                UserId = 20001,
                UserName = "testManage"
            };
            var supperManageUser = new UserModel()
            {
                DisplayName = "dotNET博士",
                ProviderId = 10001,
                Password = "123456",
                Role = EnumUserRole.SupperManage,
                SubjectId = "1",
                UserId = 20001,
                UserName = "testSupperManage"
            };
            var list = new List<UserModel>() {
         normalUser,
         manageUser,
         supperManageUser
     };
            return list?.Where(item => item.UserName.Equals(userName))?.FirstOrDefault();
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(OAuthConfig.UserApiName,OAuthConfig.UserApiName),
            };
        }
        public static IEnumerable<ApiResource> GetApiResourceRoles()
        {
            return new List<ApiResource>
            {
                new ApiResource(OAuthConfig.UserApiName,OAuthConfig.UserApiName,new List<string>(){JwtClaimTypes.Role }),
            };
        }

        public static List<string> GetWeiXinOpenIdTestUsers()
        {
            return new List<string>() { "WX123456" };
        }
    }

    public class OAuthConfig
    {
        /// <summary>
        /// 过期秒数
        /// </summary>
        public const int ExpireIn = 3600;

        public static string UserApiName = "user_api";

        public static string UserApiClientId = "user_clientid";

        public static string UserApiSecret = "user_secret";
    }
    public class ParamConstants
    {
        public const string OpenId = "openid";
        public const string UnionId = "unionid";
        public const string UserName = "user_name";
    }
    public class UserModel
    {
        public string DisplayName { get; set; }
        public int ProviderId { get; set; }
        public string Password { get; set; }
        public EnumUserRole Role { get; set; }
        public string SubjectId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
    public enum EnumUserRole
    {
        Normal,
        Manage,
        SupperManage
    }
    public enum EnumUserClaim
    {
        UserId,
        DisplayName,
        Email,
        ProviderId
    }
}
