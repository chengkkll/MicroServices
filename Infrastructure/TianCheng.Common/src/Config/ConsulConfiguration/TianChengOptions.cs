using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Winton.Extensions.Configuration.Consul;

namespace TianCheng.Common
{
    /// <summary>
    /// 配置参数管理
    /// </summary>
    static public class TianChengOptions
    {
        /// <summary>
        /// Consul作为配置中心时的配置前缀
        /// </summary>
        static public readonly string ConsulPreKey = "ServicesConfig";

        /// <summary>
        /// 获取环境变量（ASPNETCORE_ENVIRONMENT），默认为Production
        /// </summary>
        /// <returns></returns>
        static public string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        }

        #region add json
        /// <summary>
        /// 增加自定义的Json配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="envName"></param>
        /// <param name="files"></param>
        static public void AddJsonFile(this IConfigurationBuilder config, string envName, params string[] files)
        {
            if (string.IsNullOrWhiteSpace(envName))
            {
                envName = GetEnvironmentName();
            }
            config.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{envName}.json", optional: false, reloadOnChange: true);
            foreach (string file in files)
            {
                config.AddJsonFile(file, optional: false, reloadOnChange: true);
            }
        }
        #endregion

        #region add ini 
        /// <summary>
        /// 添加ini配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="files">加载指定的ini配置文件，无法找到文件时不报错。如果文件变更会自动加载</param>
        static public void AddIniFile(this IConfigurationBuilder config, params string[] files)
        {
            // optional : 文件不存在时抛异常 默认false
            // reloadOnChange : 文件变更读取新的文件 默认true
            foreach (string file in files)
            {
                config.AddIniFile(file, optional: false, reloadOnChange: true);
            }
        }
        #endregion

        #region add consul
        /// <summary>
        /// 增加Consul的作为配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="envName"></param>
        /// <param name="ConfigKeyList"></param>
        static public void AddConsul(this IConfigurationBuilder config, string envName, IList<string> ConfigKeyList)
        {
            // 加载consul配置中心配置连接地址
            var consul = config.LoadConsulOptions();
            string consul_url = consul.Address;
            string appName = consul.ServiceName;

            // 添加当前项目的配置文件。。可以根据环境变量加载
            if (ConfigKeyList == null)
            {
                ConfigKeyList = new List<string>();
            }
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/common.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/auth.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/log.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/{appName}/log.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/{appName}/ocelot.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/{appName}/swagger.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/{appName}/appsettings.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/{appName}/appsettings.{envName}.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/{appName}/custom.json");
            ConfigKeyList.Add($"{TianChengOptions.ConsulPreKey}/important.json");

            // 根据配置列表读取并合并配置信息
            foreach (string key in ConfigKeyList)
            {
                config.AddConsul(key, options =>
                {
                    // 连接consul
                    options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consul_url); };
                    // 配置选项
                    options.Optional = true;
                    // 设置柱塞式循环间隔
                    options.PollWaitTime = TimeSpan.FromSeconds(15);
                    // 配置文件更新后重新加载
                    options.ReloadOnChange = true;
                    // 加载配置时有异常忽略
                    options.OnLoadException = exceptionContext =>
                    {
                        exceptionContext.Ignore = true;
                        Serilog.Log.Logger.Error(exceptionContext.Exception, "连接Consul失败，连接地址{ConsulUrl}", consul_url);
                    };
                });
            }
        }
        #endregion
    }
}
