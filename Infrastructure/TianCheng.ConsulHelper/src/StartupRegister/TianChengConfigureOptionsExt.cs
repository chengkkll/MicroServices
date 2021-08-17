using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// TianChengConfigureOptions 扩展方法
    /// </summary>
    static public class TianChengConfigureOptionsExt
    {
        #region Consul
        /// <summary>
        /// 注册Consul的Key
        /// </summary>
        static private readonly string ConsulKey = "Consul";

        /// <summary>
        /// 判断是否已注册Consul
        /// </summary>
        /// <returns></returns>
        static public bool HasConsul(this TianChengConfigureOptions options)
        {
            return options.HasRegister(ConsulKey);
        }

        /// <summary>
        /// 注册Consul
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sectionName">配置consul节点的名称</param>
        /// <remarks>如果使用AddTianChengService，则无需在这里引用</remarks>
        static public void AddConsul(this TianChengConfigureOptions options, string sectionName = "Consul")
        {
            // 注册Consul相关服务
            ConsulRegister.ConfigureServices(sectionName);
            // 标记注册完成
            options.Register(ConsulKey);
        }

        /// <summary>
        /// 使用Consul
        /// </summary>
        static public void UseConsul(this TianChengConfigureOptions options)
        {
            if (options.HasConsul())
            {
                // 配置请求管道信息
                ConsulRegister.ConfigurePipeline(options.App);
            }
        }
        #endregion
    }
}
