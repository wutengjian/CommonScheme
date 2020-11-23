using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CommonScheme.NetCore.SysAuthority.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtLoginController : ControllerBase
    {
        private readonly JwtConfig jwtModel = null;
        public JwtLoginController(IOptions<JwtConfig> _jwtModel)//注入jwt配置参数
        {
            jwtModel = _jwtModel.Value;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns> 
        [Route("/api/JwtLogin/Login")]
        [HttpPost]
        public async Task<string> Login(LoginDto dto)
        {
            //一波验证逻辑。。。。。。

            //下面代码自行封装
            var claims = new List<Claim>();
            claims.AddRange(new[]
            {
                new Claim("UserName", dto.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, dto.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            DateTime now = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtModel.Issuer,
                audience: jwtModel.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(jwtModel.Expiration)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtModel.SecurityKey)), SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
        [Route("/api/JwtLogin/GetLogin")]
        [HttpGet]
        public async Task<string> GetLogin()
        {
            //一波验证逻辑。。。。。。
            LoginDto dto = new LoginDto() {UserName="Jianny", Password="123456" };
            //下面代码自行封装
            var claims = new List<Claim>();
            claims.AddRange(new[]
            {
                new Claim("UserName", dto.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, dto.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            DateTime now = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtModel.Issuer,
                audience: jwtModel.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(jwtModel.Expiration)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtModel.SecurityKey)), SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
