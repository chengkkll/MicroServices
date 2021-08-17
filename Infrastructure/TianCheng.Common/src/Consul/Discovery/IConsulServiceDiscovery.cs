using Consul;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TianCheng.Common
{
    /// <summary>
    /// 服务发现
    /// </summary>
    public interface IConsulServiceDiscovery
    {
        /// <summary>
        /// 异步获取一个服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="loadBalancer"></param>
        /// <returns></returns>
        Task<ServiceEntry> GetServicesAsync(string serviceName, ILoadBalancer loadBalancer);
        /// <summary>
        /// 同步获取一个服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="loadBalancer"></param>
        /// <returns></returns>
        ServiceEntry GetServices(string serviceName, ILoadBalancer loadBalancer);

        /// <summary>
        /// 通过Get方式调用一个服务的接口，并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="path"></param>
        /// <param name="balancer"></param>
        /// <returns></returns>
        T GetApi<T>(string serviceName, string path, ILoadBalancer balancer = null);

        /// <summary>
        /// 通过Post方式调用一个服务的接口，并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="balancer"></param>
        /// <returns></returns>
        T PostApi<T>(string serviceName, string path, object data, ILoadBalancer balancer = null);
    }
}
