using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.ServicesInform
{
    /// <summary>
    /// ServicesInform的WebApi注册
    /// </summary>
    static public class ServicesInformRegister
    {
        /// <summary>
        /// 设置Consul的相关配置
        /// </summary>
        /// <param name="currentMicroServiceName"></param>
        static public void ConfigureServices(string currentMicroServiceName)
        {

            // 注册配置读取的服务
            ServiceLoader.Services.AddSingleton<IMultiCheckAction, MultiCheckAction>();                 // 多个检测

            // 遍历本项目中，需要扩展检查服务，并注册。
            // 遍历本项目中，能够扩展其他服务的功能。并注册。
            ConfigureServicesExtensions.AddInform(currentMicroServiceName);
        }
    }
}
