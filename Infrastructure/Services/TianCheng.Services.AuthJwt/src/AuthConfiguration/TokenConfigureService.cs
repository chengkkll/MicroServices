using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// token 配置信息的读取服务
    /// </summary>
    public class TokenConfigureService : BaseConfigureService<TokenConfigure>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="monitor"></param>
        public TokenConfigureService(IOptionsMonitor<TokenConfigure> monitor) : base(monitor)
        {

        }

        /// <summary>
        /// 组装配置信息
        /// </summary>
        /// <param name="optons"></param>
        /// <returns></returns>
        protected override TokenConfigure Assembling(TokenConfigure optons)
        {
            if (string.IsNullOrWhiteSpace(optons.SecretKey))
            {
                TianCheng.Common.GlobalLog.Logger.Warning("请配置Token的信息");
                optons.SecretKey = Guid.NewGuid().ToString();
            }
            var keyByteArray = System.Text.Encoding.ASCII.GetBytes(optons.SecretKey);
            optons.SecurityKey = new SymmetricSecurityKey(keyByteArray);
            optons.SigningCredentials = new SigningCredentials(optons.SecurityKey, SecurityAlgorithms.HmacSha256);
            // 设置验证的配置信息
            optons.ValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,                    // 是否验证签发者的key
                IssuerSigningKey = optons.SecurityKey,              // 签发者的key
                // Validate the JWT Issuer (iss 发行人) claim
                ValidateIssuer = true,                              // 是否验证签发者
                ValidIssuer = optons.Issuer,                        // 签发者名称
                // Validate the JWT Audience (aud 订阅人) claim
                ValidateAudience = true,                            // 是否验证订阅人 （客户端）
                ValidAudience = optons.Audience,                    // 订阅人名称
                // Validate the token expiry
                ValidateLifetime = true,                            // 是否验证失效时间
                ClockSkew = TimeSpan.Zero,                          // 失效偏离时间
                RequireExpirationTime = true,                       // token是否必须具有有过期值
            };
            return optons;
        }
    }
}
