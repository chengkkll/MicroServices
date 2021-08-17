using Consul;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianCheng.Common;

namespace TianCheng.Common
{
    /// <summary>
    /// 服务发现
    /// </summary>
    public class ConsulServiceDiscovery : IConsulServiceDiscovery
    {
        private readonly ConsulClient Client;
        private readonly ILoadBalancer Balancer;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ConsulServiceDiscovery()
        {
            Client = new ConsulClient(consulConfig =>
            {
                var service = ServiceLoader.GetService<IConsulConfigurationService>();
                ConsulOptions options = service.Options;
                consulConfig.Address = new Uri(options.Address);
            });
            Balancer = ServiceLoader.GetService<ILoadBalancer>();
        }
        /// <summary>
        /// 根据serviceName获取服务地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="balancer"></param>
        /// <returns></returns>
        public ServiceEntry GetServices(string serviceName, ILoadBalancer balancer = null)
        {
            return GetServicesAsync(serviceName, balancer).Result;
        }
        /// <summary>
        /// 根据serviceName获取服务地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="balancer"></param>
        /// <returns></returns>
        public async Task<ServiceEntry> GetServicesAsync(string serviceName, ILoadBalancer balancer = null)
        {
            if (balancer == null)
            {
                balancer = Balancer;
            }
            try
            {
                var queryResult = await Client.Health.Service(serviceName, "", true);
                if (queryResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = queryResult.Response;
                    // 检查
                    if (data == null || data.Length == 0)
                    {
                        throw new EmptyServiceEntryException();
                    }
                    return balancer.Resolve(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取{serviceName}服务地址错误：{ex.Message}");
                throw;
            }
            throw new EmptyServiceEntryException();
        }

        /// <summary>
        /// Get方式调用Api接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="path"></param>
        /// <param name="balancer"></param>
        /// <returns></returns>
        public T GetApi<T>(string serviceName, string path, ILoadBalancer balancer = null)
        {
            // 获取服务对象
            var service = GetServices(serviceName, balancer);
            // 组织请求的api地址
            string Host = service.ServiceHostAddress();
            if (service.Service.Tags.Contains("http"))
            {
                Host = "http://" + Host;
            }
            else
            {
                Host = "https://" + Host;
            }
            return Host.AppendPathSegment(path).GetJsonAsync<T>().Result;
        }

        /// <summary>
        /// Post方式调用Api接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="data">post请求数据</param>
        /// <param name="path"></param>
        /// <param name="balancer"></param>
        /// <returns></returns>
        public T PostApi<T>(string serviceName, string path, object data, ILoadBalancer balancer = null)
        {
            // 获取服务对象
            var service = GetServices(serviceName, balancer);
            // 组织请求的api地址
            string Host = service.ServiceHostAddress();
            if (service.Service.Tags.Contains("http"))
            {
                Host = "http://" + Host;
            }
            else
            {
                Host = "https://" + Host;
            }
            return Host.AppendPathSegment(path).PostJsonAsync(data).ReceiveJson<T>().Result;
        }
    }
}