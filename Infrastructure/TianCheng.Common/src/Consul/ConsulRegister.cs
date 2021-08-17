using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TianCheng.Common;

namespace TianCheng.Common
{
    /// <summary>
    /// Consul注册
    /// </summary>
    public class ConsulRegister
    {
        static private bool ServicesRegister = false;
        static private bool ConfigureRegister = false;
        private static Uri ServiceUri;
        private static string ServiceId;
        /// <summary>
        /// 设置Consul的相关配置
        /// </summary>
        /// <param name="sectionName"></param>
        static public void ConfigureServices(string sectionName = "Consul")
        {
            if (ServicesRegister)
            {
                return;
            }
            ServicesRegister = true;
            // 增加健康检测
            ServiceLoader.Services.AddHealthChecks();
            // 注册Consul相关的配置
            ServiceLoader.Services.Configure<ConsulOptions>(ServiceLoader.Configuration.GetSection(sectionName));
            // 注册配置读取的服务
            ServiceLoader.Services.AddSingleton<IConsulConfigurationService, ConsulConfigurationService>();  // 配置信息
            ServiceLoader.Services.AddSingleton<IConsulServiceDiscovery, ConsulServiceDiscovery>();          // 获取服务器连接信息
            ServiceLoader.Services.AddSingleton<ILoadBalancer, RandomLoadBalancer>();                        // 设置默认的负载均衡为随机模式
            ServiceLoader.Services.AddSingleton<IRestfullApiCall, RestfulApiCall>();                         // 获取http微服务
        }

        //static public List<string> OrayIpList = new List<string> { "172.16.2.95", "172.16.0.114", "172.16.1.195" };
        /// <summary>
        /// 获取本地Ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp(ConsulOptions options)
        {
            // 获取本机使用的所有IP地址
            var addressList = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList;
            var ips = addressList.Where(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(address => address.ToString()).ToArray();
            if (ips.Length == 1)
            {
                return ips.First();
            }
            // 如果配置了蒲公英的ip地址。优先使用蒲公英地址作为本地IP
            var service = ServiceLoader.GetService<IConsulConfigurationService>();
            var OrayIpList = options.OrayIpList;
            if (OrayIpList != null && OrayIpList.Count > 0)
            {
                // 如果ip信息中包含蒲公英ip段列表信息的就优先使用
                string ip = ips.Where(ip => OrayIpList.Any(o => ip.Contains(o))).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    return ip;
                }
            }
            // 使用本机Ip地址
            return ips.Where(address => !address.EndsWith(".1")).FirstOrDefault() ?? ips.FirstOrDefault();
        }

        //public static string GetAddress()
        //{
        //    // 获取当前服务地址和端口
        //    var features = ServiceLoader.ApplicationBuilder.Properties["server.Features"] as FeatureCollection;
        //    var address = features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
        //    if (string.IsNullOrWhiteSpace(address))
        //    {
        //        throw new ArgumentException("如果通过 dotnet 命令启动请指定参数：--urls");
        //    }
        //    if (address.Contains("localhost") || address.Contains("127.0.0.1"))
        //    {
        //        string ip = GetLocalIp();
        //        address = address.Replace("localhost", ip).Replace("127.0.0.1", ip);
        //    }
        //    return address;
        //}

        /// <summary>
        /// 配置请求管道信息
        /// </summary>
        /// <param name="app"></param>
        static public IApplicationBuilder ConfigurePipeline(IApplicationBuilder app)
        {
            if (ConfigureRegister && !ServicesRegister)
            {
                return app;
            }
            ConfigureRegister = true;
            var service = ServiceLoader.GetService<IConsulConfigurationService>();
            ConsulOptions options = service.Options;
            app.UseHealthChecks(options.HealthCheck);

            // 获取主机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ConsulRegistration");

            var consulClient = new ConsulClient(configuration =>
            {
                if (!string.IsNullOrWhiteSpace(options.Address))
                {
                    // 服务注册的地址，集群中任意一个地址
                    configuration.Address = new Uri(options.Address);
                }
            });

            // 获取当前服务地址和端口
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("如果通过 dotnet 命令启动请指定参数：--urls");
            }
            if (address.Contains("localhost") || address.Contains("127.0.0.1"))
            {
                string ip = GetLocalIp(options);
                address = address.Replace("localhost", ip).Replace("127.0.0.1", ip);
            }

            var uri = new Uri(address);
            // 服务Id必须保证唯一
            ServiceId = GetServiceId(options.ServiceName, uri);

            // 节点服务注册对象
            var registration = GetRegistrationInfo(options);

            // 注册服务
            try
            {
                consulClient.Agent.ServiceRegister(registration).Wait();
            }
            catch (Exception ex)
            {
                var log = ServiceLoader.GetService<ILogger<ConsulRegister>>();
                log.LogError(ex, "无法连接consul，请确认consul是否启动或配置是否正确");
            }

            // 应用程序终止时，注销服务
            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogError($"服务停止。已注销服务：{ServiceId}");
                consulClient.Agent.ServiceDeregister(ServiceId).Wait();
            });

            return app;
        }
        /// <summary>
        /// 根据配置信息组织需要注册到Consul的服务信息
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        static private AgentServiceRegistration GetRegistrationInfo(ConsulOptions options)
        {
            return new AgentServiceRegistration()
            {
                ID = ServiceId,
                Name = options.ServiceName,
                Address = $"{ServiceUri.Host}",
                Port = ServiceUri.Port,
                Tags = new[] { ServiceUri.Scheme },
                Check = new AgentServiceCheck
                {
                    // 注册超时
                    Timeout = TimeSpan.FromSeconds(5),
                    // 服务停止多久后注销服务
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    // 健康检查地址
                    HTTP = $"{ServiceUri.Scheme}://{ServiceUri.Host}:{ServiceUri.Port}{options.HealthCheck}",
                    TLSSkipVerify = true,
                    // 健康检查时间间隔
                    Interval = TimeSpan.FromSeconds(10)
                }
            };
        }
        /// <summary>
        /// 重新注册
        /// </summary>
        /// <param name="options"></param>
        static internal void ReRegister(ConsulOptions options)
        {
            var consulClient = new ConsulClient(configuration =>
            {
                if (!string.IsNullOrWhiteSpace(options.Address))
                {
                    // 服务注册的地址，集群中任意一个地址
                    configuration.Address = new Uri(options.Address);
                }
            });
            consulClient.Agent.ServiceDeregister(ServiceId).Wait();

            // 节点服务注册对象
            var registration = GetRegistrationInfo(options);
            // 注册节点
            consulClient.Agent.ServiceRegister(registration).Wait();
        }

        /// <summary>
        /// 获取当前运行微服务的ID
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string GetServiceId(string serviceName, Uri uri)
        {
            ServiceUri = uri;
            ServiceId = $"{serviceName}_{uri.Host.Replace(".", "_")}__{uri.Port}";
            return ServiceId;
        }
    }
}
