using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TianCheng.Common
{
    /// <summary>
    /// 读取Consul的配置信息 - 以服务的形式读取数据
    /// </summary>
    public class ConsulConfigurationService : BaseConfigureService<ConsulOptions>, IConsulConfigurationService
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="monitor"></param>
        public ConsulConfigurationService(IOptionsMonitor<ConsulOptions> monitor) : base(monitor)
        {
        }
    }
}
