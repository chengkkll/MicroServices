using Consul;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianCheng.Common;

namespace TianCheng.ConsulHelper
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
        /// <param name="uri"></param>
        public ConsulServiceDiscovery(Uri uri = null)
        {
            Client = new ConsulClient(consulConfig =>
            {
                if (uri != null)
                {
                    consulConfig.Address = uri;
                }
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
            throw new EmptyServiceEntryException();
        }

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
    }
}