using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// Cap 配置信息
    /// </summary>
    public class CapOptions
    {
        /// <summary>
        /// 存储一致性的数据库信息
        /// </summary>
        public CapDBOptions DB { get; set; } = new CapDBOptions();
        /// <summary>
        /// 是否显示UI界面
        /// </summary>
        public bool ShowUI { get; set; } = false;

        /// <summary>
        /// 消息队列信息
        /// </summary>
        public CapMQOptions MQ { get; set; } = new CapMQOptions();
        /// <summary>
        /// Consul信息
        /// </summary>
        public CapConsulOptions Consul { get; set; } = new CapConsulOptions();
    }
}
