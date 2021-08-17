using Consul;
using System.Collections.Generic;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// 通过负载均衡获取
    /// </summary>
    public interface ILoadBalancer
    {
        /// <summary>
        /// 根据策略获取一个服务器信息
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        ServiceEntry Resolve(IList<ServiceEntry> services);
    }
}
