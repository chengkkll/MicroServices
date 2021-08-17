using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 消息队列信息
    /// </summary>
    public class CapMQOptions
    {
        /// <summary>
        /// 队列类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VirtualHost { get; set; }
    }
}
