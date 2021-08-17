using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 获取Cap的配置信息
    /// </summary>
    static public class CapConfiguration
    {
        /// <summary>
        /// 读取Cap的配置信息
        /// </summary>
        /// <returns></returns>
        static public CapOptions LoadCapOptions(string sectionName = "Consul")
        {
            return LoadCapOptions(ServiceLoader.Configuration, sectionName);
        }
        /// <summary>
        /// 读取Cap的配置信息
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        static public CapOptions LoadCapOptions(this IConfigurationBuilder configurationBuilder, string sectionName = "Consul")
        {
            return LoadCapOptions(configurationBuilder.Build(), sectionName);
        }

        /// <summary>
        /// 读取Cap的配置信息
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        static public CapOptions LoadCapOptions(this IConfiguration configuration, string sectionName = "Consul")
        {
            CapOptions options = new CapOptions();
            // 读取配置信息
            configuration.GetSection(sectionName).Bind(options);

            if (options == null)
            {
                options = new CapOptions();
            }

            // 完善Consul对应的配置
            // 从命令行参数中读取配置信息
            Uri currentUri = AppEnvironment.AppUri;
            ConsulOptions consul = configuration.LoadConsulOptions();

            // 配置Consul的信息
            if (string.IsNullOrWhiteSpace(options.Consul.DiscoveryServerHostName) || options.Consul.DiscoveryServerPort <= 0)
            {
                Uri consulUri = new Uri(consul.Address);
                options.Consul.DiscoveryServerHostName = consulUri.Host;
                options.Consul.DiscoveryServerPort = consulUri.Port;
            }

            if (currentUri.Scheme == "https")
            {
                string urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
                if (!string.IsNullOrWhiteSpace(urls))
                {
                    foreach (string url in urls.Split(";"))
                    {
                        if (url.StartsWith("http://"))
                        {
                            currentUri = new Uri(url);
                            break;
                        }
                    }
                }
            }

            options.Consul.CurrentScheme = currentUri.Scheme;
            options.Consul.CurrentNodeHostName = currentUri.Host;
            options.Consul.CurrentNodePort = currentUri.Port;
            options.Consul.NodeId = GetServiceId(consul.ServiceName, currentUri);
            options.Consul.NodeName = consul.ServiceName;

            return options;
        }


        /// <summary>
        /// 获取当前运行微服务的ID
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string GetServiceId(string serviceName, Uri uri)
        {
            return $"{serviceName}_{uri.Host.Replace(".", "_")}__{uri.Port}";
        }

    }
}
