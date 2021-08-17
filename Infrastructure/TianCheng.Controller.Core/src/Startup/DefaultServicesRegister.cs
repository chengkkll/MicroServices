using Microsoft.Extensions.Configuration;
using System;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.DAL;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;
using TianCheng.DAL.NpgSqlByEF;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 配置TianCheng使用组件
    /// </summary>
    static public class DefaultServicesRegister
    {
        /// <summary>
        /// 配置使用TianCheng
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <param name="routePrefix"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public IServiceCollection AddTianCheng(this IServiceCollection services, IConfiguration Configuration, string routePrefix = "api", Action<TianChengConfigureOptions> options = null)
        {
            ServiceLoader.Services = services;
            ServiceLoader.Configuration = Configuration;

            TianChengConfigureOptions cs = TianChengConfigureOptions.Instance;
            if (options != null)
            {
                // 使用指定配置
                options.Invoke(cs);
            }
            else
            {
                // 使用默认配置
                cs.AddSerilog();
                cs.AddAutoMapper();
                cs.AddCap();
                cs.AddControllerSetting(routePrefix);
                cs.AddHasCors();
                cs.AddConsul();
                cs.AddDbServices();
                cs.AddNpgSqlAccess();
                cs.AddBusinessServices();
                cs.AddAuthJwt();
                cs.AddSwagger();
            }
            return services;
        }

        /// <summary>
        /// 网关使用的默认配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <param name="routePrefix"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public IServiceCollection AddGateway(this IServiceCollection services,
           IConfiguration Configuration,
           string routePrefix = "api",
           Action<TianChengConfigureOptions> options = null)
        {
            ServiceLoader.Services = services;
            ServiceLoader.Configuration = Configuration;

            TianChengConfigureOptions cs = TianChengConfigureOptions.Instance;
            if (options != null)
            {
                // 使用指定配置
                options.Invoke(cs);
            }
            else
            {
                // 使用默认配置
                cs.AddSerilog();
                cs.AddAutoMapper();
                cs.AddControllerSetting(routePrefix);
                cs.AddHasCors();
                //cs.AddConsul();
                cs.AddBusinessServices();
                cs.AddAuthJwt();
                cs.AddSwagger();
            }
            return services;
        }
    }
}
