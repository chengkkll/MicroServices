using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// Consul配置接口
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
