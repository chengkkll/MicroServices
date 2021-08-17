using System;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.DAL;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 设置TianCheng管道
    /// </summary>
    static public class DefaultConfigureRegister
    {
        /// <summary>
        /// 为管道中增加TianCheng
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public IApplicationBuilder UseTianCheng(this IApplicationBuilder app, Action<TianChengConfigureOptions> options = null)
        {
            ServiceLoader.ApplicationBuilder = app;
            ServiceLoader.Instance = app.ApplicationServices;
            TianChengConfigureOptions co = TianChengConfigureOptions.Instance.SetApp(app);
            if (options != null)
            {
                // 使用指定配置
                options.Invoke(co);
            }
            else
            {
                // 使用默认配置
                co.UseSerilog();
                co.UseCors();
                co.UseControllerSetting();
                co.UseDbServices();
                co.UseAuthJwt();
                co.UseConsul();
                co.UseSwagger();
                co.UseCap();
            }
            return app;
        }

        /// <summary>
        /// 为网关管道中增加TianCheng
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public IApplicationBuilder UseGateway(this IApplicationBuilder app, Action<TianChengConfigureOptions> options = null)
        {
            ServiceLoader.ApplicationBuilder = app;
            ServiceLoader.Instance = app.ApplicationServices;
            TianChengConfigureOptions co = TianChengConfigureOptions.Instance.SetApp(app);
            if (options != null)
            {
                // 使用指定配置
                options.Invoke(co);
            }
            else
            {
                // 使用默认配置
                co.UseSerilog();
                co.UseCors();
                co.UseControllerSetting();
                co.UseAuthJwt();
                //co.UseConsul();
                co.UseSwagger();
            }
            return app;
        }
    }
}
