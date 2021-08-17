using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.ServicesInform;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// WebApi中使用ServiceInform
    /// </summary>
    static public class DIRegister
    {
        /// <summary>
        /// WebApi中使用ServiceInform
        /// </summary>
        /// <param name="services"></param>
        /// <param name="currentMicroServiceName"></param>
        static public void AddServicesInform(this IServiceCollection services, string currentMicroServiceName)
        {
            ServiceLoader.Services = services;

            TianCheng.ConsulHelper.ConsulRegister.ConfigureServices();
            ServicesInformRegister.ConfigureServices(currentMicroServiceName);
        }

        /// <summary>
        /// 为管道中增加TianCheng
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static public IApplicationBuilder AddTianChengService(this IApplicationBuilder app)
        {
            return TianCheng.ConsulHelper.ConsulRegister.ConfigurePipeline(app);
        }
    }
}
