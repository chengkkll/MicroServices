using Microsoft.Extensions.Hosting;
using System;
using TianCheng.Common;

namespace TianCheng.ConsulHelper
{
    ///// <summary>
    ///// Consul配置信息
    ///// </summary>
    //public class ConsulOptions
    //{
    //    /// <summary>
    //    /// 连接的Consul地址
    //    /// </summary>
    //    public string Address { get; set; }

    //    /// <summary>
    //    /// 服务名称
    //    /// </summary>
    //    public string ServiceName { get; set; }

    //    /// <summary>
    //    /// 健康检查地址
    //    /// </summary>
    //    public string HealthCheck { get; set; }

    //    /// <summary>
    //    /// 默认配置信息
    //    /// </summary>
    //    static public ConsulOptions Default
    //    {
    //        get
    //        {
    //            return new ConsulOptions
    //            {
    //                Address = TianChengOptions.ConsulDefaultAddress,
    //                ServiceName = TianCheng.Common.ServiceLoader.GetEnviroment().ApplicationName,
    //                HealthCheck = "/Health"
    //            };
    //        }
    //    }
    //}
}
