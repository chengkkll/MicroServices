using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// Consul配置信息
    /// </summary>
    public class ConsulOptions
    {
        /// <summary>
        /// 连接的Consul地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthCheck { get; set; }

        /// <summary>
        /// 使用蒲公英时的Ip列表
        /// Consul会优先使用本地蒲公英Ip地址，主要是开发时使用。
        /// </summary>
        public List<string> OrayIpList { get; set; } = new List<string>();
        ///// <summary>
        ///// 默认配置信息
        ///// </summary>
        //static public ConsulOptions Default
        //{
        //    get
        //    {
        //        return ConsulConfiguration.LoadConsulOptions();
        //    }
        //}
    }
}
