//using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// Token参数配置信息
    /// </summary>
    public class TokenConfigure
    {
        /// <summary>
        /// Scheme
        /// </summary>
        public string Scheme { get; set; } = "Bearer";
        /// <summary>
        /// 是否使用Https
        /// </summary>
        public bool IsHttps { get; set; } = false;
        /// <summary>
        /// 登录Api地址
        /// </summary>
        public string LogonApi { get; set; } = "/api/auth/login";
        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 签发者的key
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SecurityKey SecurityKey { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(24);
        /// <summary>
        /// 签发者的证书
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// 验证的参数
        /// </summary>
        public TokenValidationParameters ValidationParameters { get; set; }

        /// <summary>
        /// 验证权限服务
        /// </summary>
        public string AuthService { get; set; }
        /// <summary>
        /// 验证权限服务名称
        /// </summary>
        public string PowerCheckServiceName { get; set; }
        /// <summary>
        /// 验证权限接口地址
        /// </summary>
        public string PowerCheckApiPath { get; set; }

        /// <summary>
        /// Token的默认配置
        /// </summary>
        static public TokenConfigure Default
        {
            get
            {
                // todo : 返回默认配置
                return new TokenConfigure
                {

                };
            }
        }
    }
}
