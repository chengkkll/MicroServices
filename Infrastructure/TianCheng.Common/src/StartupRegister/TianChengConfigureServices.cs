using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 配置TianCheng使用组件
    /// </summary>
    static public class TianChengConfigureServices
    {
        /// <summary>
        /// 配置使用TianCheng
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public IServiceCollection AddTianChengCommon(this IServiceCollection services, IConfiguration Configuration, Action<TianChengConfigureOptions> options = null)
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
                cs.AddAutoMapper();
                cs.AddSerilog();
            }
            return services;
        }
    }
}
