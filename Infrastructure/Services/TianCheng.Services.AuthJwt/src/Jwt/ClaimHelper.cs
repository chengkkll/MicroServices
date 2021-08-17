using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// Jwt的Claim操作
    /// </summary>
    public class ClaimHelper
    {
        static private readonly string UserId = "id";
        static private readonly string UserName = "name";
        static private readonly string AuthorizationService = "auth";
        private const string DefaultAuthServiceName = "AuthService";

        #region User Id
        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        static public string GetUserId(ClaimsIdentity identity)
        {
            return GetClaim(identity, UserId);
        }
        /// <summary>
        /// 创建一个UserId的Claim
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public Claim NewUserId(string id)
        {
            return NewClaim(UserId, id);
        }
        #endregion

        #region User Name
        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        static public string GetUserName(ClaimsIdentity identity)
        {
            return GetClaim(identity, UserName);
        }
        /// <summary>
        /// 创建一个UserName的Claim
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static public Claim NewUserName(string name)
        {
            return NewClaim(UserName, name);
        }
        #endregion

        #region Authorization Service
        /// <summary>
        /// 根据Token获取判定权限的服务
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        static public string GetAuthorizationService(ClaimsIdentity identity)
        {
            string serviceName = GetClaim(identity, AuthorizationService);
            return string.IsNullOrWhiteSpace(serviceName) ? DefaultAuthServiceName : serviceName;
        }
        /// <summary>
        /// 指定为解析Token时使用的服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        static public Claim NewAuthorizationService(string serviceName = DefaultAuthServiceName)
        {
            return NewClaim(AuthorizationService, serviceName);
        }

        /// <summary>
        /// 将当前AuthService指定为解析Token时使用的服务
        /// </summary>
        /// <param name="authService"></param>
        /// <returns></returns>
        static public Claim NewAuthorizationService(object authService)
        {
            return NewAuthorizationService(authService.GetType().FullName);
        }
        #endregion

        #region 过期时间
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        static public Claim NewExpiration(double expiration = -1)
        {
            // 在配置中获取过期时间
            if (expiration < 0)
            {
                // todo : ServiceLoader.Configuration.GetSection("");
                expiration = 5000;
            }

            // 生成过期时间的Claim
            return NewClaim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(expiration).ToString());
        }
        #endregion

        #region Core
        /// <summary>
        /// 获取一个Claim数据
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static private string GetClaim(ClaimsIdentity identity, string key)
        {
            if (identity == null || identity.Claims == null)
            {
                return string.Empty;
            }
            var claim = identity.Claims.Where(e => e.Type == key).FirstOrDefault();
            return claim == null ? string.Empty : claim.Value;
        }

        /// <summary>
        /// 获取一个Claim
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static private Claim NewClaim(string key, string val)
        {
            return new Claim(key, val);
        }
        #endregion
    }

}
