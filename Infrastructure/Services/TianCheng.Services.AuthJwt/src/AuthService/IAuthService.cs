using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// 权限处理接口
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 判断用户是否有拥有指定的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        public bool HasPower(string userId, string powerCode);
    }
    /// <summary>
    /// 权限处理接口
    /// </summary>
    public interface IAuthService<T> : IAuthService
        where T : LoginView
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public TokenResult Login(T loginInfo);
    }
}
