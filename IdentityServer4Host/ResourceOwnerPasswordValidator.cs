using IdentityModel;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4Host
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userName = context.UserName;
                var password = context.Password;
                var claimList = await ValidateUserAsync(userName, password);//验证用户,这么可以到数据库里面验证用户名和密码是否正确
                context.Result = new GrantValidationResult(subject: userName, authenticationMethod: "custom", claims: claimList.ToArray());// 验证账号
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult() { IsError = true, Error = ex.Message };//验证异常结果
            }
        }
        #region Private Method
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<List<Claim>> ValidateUserAsync(string loginName, string password)
        {
            //TODO 这里可以通过用户名和密码到数据库中去验证是否存在，
            // 以及角色相关信息，我这里还是使用内存中已经存在的用户和密码
            var user = ID4Config.GetUsers();

            if (user == null)
                throw new Exception("登录失败，用户名和密码不正确");
            return new List<Claim>() { new Claim(ClaimTypes.Name, $"{loginName}"), };
        }
        #endregion
        public async Task<List<Claim>> ValidateUserByRoleAsync(string loginName, string password)
        {
            var user = ID4Config.GetUserByUserName(loginName);
            if (user == null)
                throw new Exception("登录失败，用户名和密码不正确");
            //实际生产环境需要通过读取数据库的信息并且来声明
            return new List<Claim>()
            {
             new Claim(ClaimTypes.Name, $"{user.UserName}"),
             new Claim(EnumUserClaim.DisplayName.ToString(),user.DisplayName),
             new Claim(EnumUserClaim.UserId.ToString(),user.UserId.ToString()),
             new Claim(EnumUserClaim.ProviderId.ToString(),user.ProviderId.ToString()),
             new Claim(JwtClaimTypes.Role.ToString(),user.Role.ToString())
         };
        }
    }

}
