using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Winton.Extensions.Configuration.Consul;
using TianCheng.Common;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 主机的配置信息
    /// </summary>
    public class TianChengHostOptions
    {
        /// <summary>
        /// web主机信息
        /// </summary>
        private readonly IHostBuilder WebBuilder;
        /// <summary>
        /// 配置中心的配置key列表
        /// </summary>
        public IList<string> ConfigKeyList { get; set; } = new List<string>();
        /// <summary>
        /// 构造方法必须设置web主机
        /// </summary>
        /// <param name="webBuilder"></param>
        public TianChengHostOptions(IHostBuilder webBuilder)
        {
            WebBuilder = webBuilder;
        }
        /// <summary>
        /// 更新配置中的环境变量信息
        /// <para>***  正常不必调用，默认会自动加载  ***</para>
        /// </summary>
        public void AddEnvironment()
        {
            WebBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();
            });
        }
        /// <summary>
        /// 添加命令行配置信息
        /// </summary>
        /// <param name="args">key</param>
        /// <param name="switchMappings">别名转换字典</param>
        public void AddCommandLine(string[] args, IDictionary<string, string> switchMappings = null)
        {
            WebBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddCommandLine(args, switchMappings);
            });
        }
        /// <summary>
        /// 添加Json配置文件
        /// <para>默认会加载appsettings.{EnvironmentName}.json文件，该文件优先级低于指定的json配置文件</para> 
        /// <para>环境变量需要配置 ASPNETCORE_ENVIRONMENT 例如：ASPNETCORE_ENVIRONMENT=Production</para>
        /// </summary>
        /// <param name="files">加载指定的json配置文件，无法找到文件时不报错。如果文件变更会自动加载</param>
        public void AddJsonFile(params string[] files)
        {
            WebBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                string envName = hostingContext.HostingEnvironment.EnvironmentName;
                config.AddJsonFile(envName, files);
            });
        }

        /// <summary>
        /// 添加ini配置文件
        /// </summary>
        /// <param name="files">加载指定的ini配置文件，无法找到文件时不报错。如果文件变更会自动加载</param>
        public void AddInitFile(params string[] files)
        {
            WebBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddIniFile(files);
            });
        }

        /// <summary>
        /// 使用Nacos作为配置中心
        /// </summary>
        public void AddNacos()
        {
            throw new Exception("暂时不支持Nacos");
        }

        ///// <summary>
        ///// 使用Apollo作为配置中心
        ///// 暂时可能不考虑做Apollo的支持。以后遇到再说
        ///// </summary>
        //public void UseApollo()
        //{
        //    throw new Exception("暂时不支持Apollo");
        //}


        /// <summary>
        /// 使用Consul作为配置中心
        /// <para>默认的配置节点按优先级顺序有：</para> 
        /// <para>ServicesConfig/important.json</para> 
        /// <para>ServicesConfig/{AppName}/custom.json</para> 
        /// <para>ServicesConfig/{AppName}/appsettings.{EnvironmentName}.json</para> 
        /// <para>ServicesConfig/{AppName}/appsettings.json</para> 
        /// <para>ServicesConfig/{AppName}/swagger.json</para> 
        /// <para>ServicesConfig/{AppName}/ocelo.json</para> 
        /// <para>ServicesConfig/{AppName}/log.json</para> 
        /// <para>ServicesConfig/log.json</para> 
        /// <para>ServicesConfig/auth.json</para> 
        /// <para>ServicesConfig/common.json</para> 
        /// <para>注：环境变量需要配置 ASPNETCORE_ENVIRONMENT 例如：ASPNETCORE_ENVIRONMENT=Production</para>
        /// </summary>
        public void AddConsul()
        {
            // 当web主机创建完成后，设置配置信息
            WebBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 添加当前项目的配置文件。。可以根据环境变量加载
                string envName = hostingContext.HostingEnvironment.EnvironmentName;
                config.AddConsul(envName, ConfigKeyList);

                hostingContext.Configuration = config.Build();
            });
        }
    }
}
