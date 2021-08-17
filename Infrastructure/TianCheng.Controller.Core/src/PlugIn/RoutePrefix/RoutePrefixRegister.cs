using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Controller.Core.PlugIn.RoutePrefix
{
    /// <summary>
    /// 注册路由前缀信息
    /// </summary>
    public static class RoutePrefixRegister
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="routeAttribute"></param>
        public static void SetRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // 添加自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }

        /// <summary>
        /// 指定路由的前缀
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="routePrefix"></param>
        public static void SetRoutePrefix(this MvcOptions opts, string routePrefix)
        {
            // 添加自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConvention(new RouteAttribute(routePrefix)));
        }
    }
}
