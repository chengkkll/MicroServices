using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 控制器中获取请求IP地址的信息
    /// </summary>
    static public class ControllerRequestIpExt
    {
        #region 请求地址
        /// <summary>
        /// 获取请求的IP地址
        /// </summary>
        static public string IpAddress(this ControllerBase controller)
        {
            // 获取请求链接的服务
            return controller.HttpContext.Connection.RemoteIpAddress.ToString();

            //IHttpContextAccessor ContextAccessor = ServiceLoader.GetService<IHttpContextAccessor>();

            //if (ContextAccessor != null)
            //{
            //    return ContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            //}
            //return string.Empty;
        }
        #endregion
    }
}
