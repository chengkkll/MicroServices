using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;
using Flurl;
using Flurl.Http;
using System.Linq;

namespace TianCheng.Inventory.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DefAuthService : IAuthService<LoginView>, IBusinessService
    {
        /// <summary>
        /// 获取用户是否有指定的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        private bool HasPowerByHttp(string userId, string powerCode)
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
        /// 判断用户是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        public bool HasPower(string userId, string powerCode)
        {
            return HasPowerByHttp(userId, powerCode);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public TokenResult Login(LoginView loginInfo)
        {
            return new TokenResult();
        }
    }
}
