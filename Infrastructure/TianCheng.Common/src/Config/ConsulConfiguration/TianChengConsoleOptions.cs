using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 控制台程序的配置
    /// </summary>
    public class TianChengConsoleOptions
    {
        #region 构造方法
        /// <summary>
        /// 
        /// </summary>
        readonly IConfigurationBuilder Config;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="configBuilder"></param>
        public TianChengConsoleOptions(IConfigurationBuilder configBuilder)
        {
            Config = configBuilder;
        }
        #endregion

        /// <summary>
        /// 配置中心的配置key列表
        /// </summary>
        public IList<string> ConfigKeyList { get; set; } = new List<string>();
        #region 获取控制台信息
        /// <summary>
        /// 获取环境变量（ASPNETCORE_ENVIRONMENT），默认为Production
        /// </summary>
        /// <returns></returns>
        private string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        }
        #endregion
        /// <summary>
        /// 更新配置中的环境变量信息
        /// <para>***  正常不必调用，默认会自动加载  ***</para>
        /// </summary>
        public void AddEnvironment()
        {
            Config.AddEnvironmentVariables();
        }
        /// <summary>
        /// 添加命令行配置信息
        /// </summary>
        /// <param name="args">key</param>
        /// <param name="switchMappings">别名转换字典</param>
        public void AddCommandLine(string[] args, IDictionary<string, string> switchMappings = null)
        {
            Config.AddCommandLine(args, switchMappings);
        }

        #region json 
        /// <summary>
        /// 添加Json配置文件
        /// <para>默认会加载appsettings.{EnvironmentName}.json文件，该文件优先级低于指定的json配置文件</para> 
        /// <para>环境变量需要配置 ASPNETCORE_ENVIRONMENT 例如：ASPNETCORE_ENVIRONMENT=Production</para>
        /// </summary>
        /// <param name="files">加载指定的json配置文件，无法找到文件时不报错。如果文件变更会自动加载</param>
        public void AddJsonFile(params string[] files)
        {
            Config.AddJsonFile(GetEnvironmentName(), files);
        }
        #endregion

        #region ini
        /// <summary>
        /// 添加ini配置文件
        /// </summary>
        /// <param name="files">加载指定的ini配置文件，无法找到文件时不报错。如果文件变更会自动加载</param>
        public void AddInitFile(params string[] files)
        {
            Config.AddIniFile(files);
        }
        #endregion

        #region Consul
        /// <summary>
        /// 使用Consul作为配置中心
        /// <para>默认的配置节点按优先级顺序有：</para> 
        /// <para>ServicesConfig/important.json</para> 
        /// <para>ServicesConfig/{AppName}/custom.json</para> 
        /// <para>ServicesConfig/{AppName}/{EnvironmentName}.appsettings.json</para> 
        /// <para>ServicesConfig/{AppName}/appsettings.json</para> 
        /// <para>ServicesConfig/ocelo.json</para> 
        /// <para>ServicesConfig/log.json</para> 
        /// <para>ServicesConfig/common.json</para> 
        /// <para>注：环境变量需要配置 ASPNETCORE_ENVIRONMENT 例如：ASPNETCORE_ENVIRONMENT=Production</para>
        /// </summary>
        public void AddConsul()
        {
            Config.AddConsul(GetEnvironmentName(), ConfigKeyList);
        }
        #endregion
    }
}
