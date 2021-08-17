using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace TianCheng.Common
{
    /// <summary>
    /// 对控制台的配置
    /// </summary>
    static public class ConsoleConfiguration
    {
        /// <summary>
        /// 设置控制台的配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="configure"></param>
        /// <param name="configKeyArray">
        /// 定义读取的配置节点名称列表。
        /// <para>默认的配置节点按优先级顺序有：</para> 
        /// <para>ServicesConfig/important.json</para> 
        /// <para>ServicesConfig/{AppName}/custom.json</para> 
        /// <para>ServicesConfig/{AppName}/{EnvironmentName}.appsettings.json</para> 
        /// <para>ServicesConfig/{AppName}/appsettings.json</para> 
        /// <para>ServicesConfig/common.json</para> 
        /// </param>
        /// <returns></returns>
        static public IConfigurationRoot TianChengConfiguration(this ConfigurationBuilder config, Action<TianChengConsoleOptions> configure = null, params string[] configKeyArray)
        {
            // 初始化配置
            TianChengConsoleOptions co = new TianChengConsoleOptions(config)
            {
                ConfigKeyList = configKeyArray.ToList()
            };
            // 运行配置
            if (configure != null)
            {
                // 执行根据用户指定的配置
                configure.Invoke(co);
            }
            else
            {
                // 默认配置
                co.AddJsonFile();
            }
            // 返回build后的配置信息
            IConfigurationRoot cr = config.Build();
            ServiceLoader.Configuration = cr;
            return cr;
        }
    }
}
