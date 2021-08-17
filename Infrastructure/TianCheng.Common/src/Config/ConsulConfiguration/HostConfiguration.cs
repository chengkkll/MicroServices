using System;
using System.Linq;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 对主机的配置操作
    /// </summary>
    static public class HostConfiguration
    {
        /// <summary>
        /// 设置主机配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <param name="configKeyArray">
        /// 定义读取的配置节点名称列表。
        /// <para>默认的配置节点按优先级顺序有：</para> 
        /// <para>ServicesConfig/important.json</para> 
        /// <para>ServicesConfig/{AppName}/custom.json</para> 
        /// <para>ServicesConfig/{AppName}/{EnvironmentName}.appsettings.json</para> 
        /// <para>ServicesConfig/{AppName}/appsettings.json</para> 
        /// <para>ServicesConfig/auth.json</para> 
        /// <para>ServicesConfig/common.json</para> 
        /// </param>
        /// <returns></returns>
        static public IHostBuilder TianChengConfiguration(this IHostBuilder builder, Action<TianChengHostOptions> configure = null, params string[] configKeyArray)
        {
            // 初始化配置参数
            TianChengHostOptions ho = new TianChengHostOptions(builder)
            {
                ConfigKeyList = configKeyArray.ToList()
            };
            // 运行配置
            if (configure != null)
            {
                // 执行根据用户指定的配置
                configure.Invoke(ho);
            }
            else
            {
                // 使用默认的配置文件加载
                ho.AddJsonFile();
                // 使用Consul作为配置中心
                ho.AddConsul();
            }

            return builder;
        }
    }
}
