using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 读取Consul的配置信息 - 用于主机启动时静态读取
    /// </summary>
    static public class ConsulConfiguration
    {
        /// <summary>
        /// 读取Consul的配置信息
        /// </summary>
        /// <returns></returns>
        static public ConsulOptions LoadConsulOptions()
        {
            return LoadConsulOptions(ServiceLoader.Configuration);
        }
        /// <summary>
        /// 读取Consul的配置信息
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        static public ConsulOptions LoadConsulOptions(this IConfigurationBuilder configurationBuilder, string sectionName = "Consul")
        {
            var tempConfig = configurationBuilder.Build();
            return LoadConsulOptions(tempConfig, sectionName);
        }

        /// <summary>
        /// 读取Consul的配置信息
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        static public ConsulOptions LoadConsulOptions(this IConfiguration configuration, string sectionName = "Consul")
        {
            ConsulOptions options = new ConsulOptions();
            // 读取配置信息
            configuration.GetSection(sectionName).Bind(options);

            // 完善地址新
            if (string.IsNullOrWhiteSpace(options.Address))
            {
                options.Address = ConsulDefaultAddress;
                Serilog.Log.Logger.Warning("请配置Consul的Address节点来指定Consul的连接地址。当前尝试使用{ConsulUrl}地址连接。", options.Address);
            }

            // 完善当前服务的名称
            if (string.IsNullOrWhiteSpace(options.ServiceName))
            {
                options.ServiceName = GetAppName();
                Serilog.Log.Logger.Warning("请配置Consul的ServiceName节点来指定当前服务的名称。当前尝试使用{ServiceName}名称连接。", options.ServiceName);
            }

            // 完善当前服务的健康地址
            if (string.IsNullOrWhiteSpace(options.HealthCheck))
            {
                options.HealthCheck = ConsulDefaultHealth;
                Serilog.Log.Logger.Warning("请配置Consul的HealthCheck节点来指定当前服务的健康测试地址。当前尝试使用{HealthCheck}地址连接。", options.HealthCheck);
            }

            return options;
        }

        /// <summary>
        /// Consul作为配置中心的默认地址
        /// </summary>
        private static readonly string ConsulDefaultAddress = "http://127.0.0.1:8500";
        /// <summary>
        /// Consul测试服务健康的地址
        /// </summary>
        private static readonly string ConsulDefaultHealth = "/Health";
        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <returns></returns>
        static private string GetAppName()
        {
            return ServiceLoader.GetEnviroment().ApplicationName;
        }
    }
}
