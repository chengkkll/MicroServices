using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// JWTToken生成类
    /// </summary>
    public class JwtBuilder
    {
        /// <summary>
        /// 获取基于JWT的Token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static TokenResult BuildToken(string username, List<Claim> claims)
        {
            TokenConfigure tokenConfigure = ServiceLoader.GetService<TokenConfigureService>().Options;

            var now = DateTime.UtcNow;
            // 完善Claim信息
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, username));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64));
            // 生成Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    issuer: tokenConfigure.Issuer,
                    audience: tokenConfigure.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(tokenConfigure.Expiration),
                    signingCredentials: tokenConfigure.SigningCredentials
                ));
            // 设置返回值
            return new TokenResult
            {
                Status = true,
                Token = encodedJwt,
                Expires = now.Add(tokenConfigure.Expiration),
                Scheme = tokenConfigure.Scheme
            };
        }
    }
}
