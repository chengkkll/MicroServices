using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// 权限处理示例
    /// </summary>
    public class AuthService : IAuthService<LoginView>
    {
        /// <summary>
        /// 获取用户是否有指定的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        public bool HasPower(string userId, string powerCode)
        {
            // 获取配置信息
            TokenConfigure tokenConfigure = ServiceLoader.GetService<TokenConfigureService>().Options;
            string serviceName = tokenConfigure.PowerCheckServiceName;
            string restfulPath = tokenConfigure.PowerCheckApiPath.Replace("{{userId}}", userId).Replace("{{powerCode}}", powerCode);
            // 加载对应服务并发送请求
            var loader = ServiceLoader.GetService<IConsulServiceDiscovery>();
            try
            {
                return loader.GetApi<bool>(serviceName, restfulPath);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public TokenResult Login(LoginView loginInfo)
        {
            // 1、判断输入的账号密码是否符合要求
            // 2、根据输入的账号密码获取账号数据
            // 3、写账号信息到缓存中
            // 4、根据账号数据生成token
            return JwtBuilder.BuildToken(loginInfo.Account, new List<Claim>
            {
                ClaimHelper.NewUserId(Guid.NewGuid().ToString()),
                ClaimHelper.NewUserName(loginInfo.Account),
                ClaimHelper.NewAuthorizationService(this)
            });
        }
    }

    public class PowerCacheDict
    {
        private static readonly Dictionary<string, List<string>> Cache = new Dictionary<string, List<string>>();

        public void Set(string account, List<string> powerList)
        {
            if (Cache.ContainsKey(account))
            {
                Cache.Remove(account);
            }
            Cache.Add(account, powerList);
        }

        public bool HasPower(string account, string powerCode)
        {
            return Cache.ContainsKey(account) && Cache[account].Contains(powerCode);
        }
    }
}
