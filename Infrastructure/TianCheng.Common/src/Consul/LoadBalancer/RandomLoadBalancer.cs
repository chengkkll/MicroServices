using Consul;
using System;
using System.Collections.Generic;

namespace TianCheng.Common
{
    /// <summary>
    /// 随机获取
    /// </summary>
    public class RandomLoadBalancer : ILoadBalancer
    {
        static private readonly Random random = new Random();
        /// <summary>
        /// 根据随机策略获取一个服务器信息
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public ServiceEntry Resolve(IList<ServiceEntry> services)
        {
            // 检查
            if (services == null || services.Count == 0)
            {
                throw new EmptyServiceEntryException();
            }
            int count = services.Count;

            if (count == 1)
            {
                return services[0];
            }
            // 随机获取一项
            return services[random.Next(count)];
        }
    }
}
