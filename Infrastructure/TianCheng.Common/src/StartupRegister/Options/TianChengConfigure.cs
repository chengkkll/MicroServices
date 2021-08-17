using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 设置TianCheng管道
    /// </summary>
    static public class TianChengConfigure
    {
        /// <summary>
        /// 为管道中增加TianCheng
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public IApplicationBuilder UseTianChengCommon(this IApplicationBuilder app, Action<TianChengConfigureOptions> options = null)
        {
            ServiceLoader.Instance = app.ApplicationServices;
            TianChengConfigureOptions co = TianChengConfigureOptions.Instance.SetApp(app);
            if (options != null)
            {
                // 使用指定配置
                options.Invoke(co);
            }
            else
            {
                co.UseSerilog();
            }
            return app;
        }
    }
}
