using Consul;
using System.Collections.Generic;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// 轮寻服务器信息
    /// </summary>
    public class RoundRobinLoadBalancer : ILoadBalancer
    {
        private readonly object _lock = new object();
        private int _index = 0;

        /// <summary>
        /// 轮寻策略获取一个服务器信息
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public ServiceEntry Resolve(IList<ServiceEntry> services)
        {
            // 使用lock控制并发
            lock (_lock)
            {
                if (_index >= services.Count)
                {
                    _index = 0;
                }
                return services[_index++];
            }
        }
    }
}
