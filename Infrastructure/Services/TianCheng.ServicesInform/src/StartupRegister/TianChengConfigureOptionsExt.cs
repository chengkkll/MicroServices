using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.ConsulHelper;

namespace TianCheng.ServicesInform
{
    /// <summary>
    /// TianChengConfigureOptions 的扩展方法
    /// </summary>
    static public class TianChengConfigureOptionsExt
    {
        #region Services Inform
        /// <summary>
        /// 注册ServicesInform的Key
        /// </summary>
        static private readonly string ServicesInform = "ServicesInform";

        /// <summary>
        /// 判断是否已注册ServicesInform
        /// </summary>
        /// <returns></returns>
        static public bool HasServicesInform(this TianChengConfigureOptions options)
        {
            return options.HasRegister(ServicesInform);
        }
        /// <summary>
        /// 注册数据库操作服务
        /// </summary>
        /// <param name="options"></param>
        /// <param name="currentMicroServiceName">当前项目的微服务名称</param>
        /// <param name="sectionName">consul配置节点名称</param>
        /// <remarks>如果使用AddTianChengService，则无需在这里引用</remarks>
        static public void AddServicesInform(this TianChengConfigureOptions options, string currentMicroServiceName, string sectionName = "consul")
        {
            // 如果没有注册Consul对应的信息，先注册Consul
            if (!options.HasConsul())
            {
                options.AddConsul(sectionName);
            }
            // 注册 ServicesInform
            ServicesInformRegister.ConfigureServices(currentMicroServiceName);
            // 标记注册完成
            options.Register(ServicesInform);
        }
        #endregion
    }
}
