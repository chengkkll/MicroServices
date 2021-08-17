using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 读取配置信息
    /// </summary>
    static public class Configuration
    {
        /// <summary>
        /// 在配置中获取Section对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static private IConfigurationSection GetSection(string key)
        {
            try
            {
                return ServiceLoader.Configuration.GetSection(key);
            }
            catch
            {
                GlobalLog.Logger.Warning("无法找到指定的配置信息,{Section}", key);
                return null;
            }
        }

        /// <summary>
        /// 获取指定的键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public string Get(string key)
        {
            var section = GetSection(key);
            if (section == null || section.Value == null)
            {
                return string.Empty;
            }
            return section.Value;
        }
        /// <summary>
        /// 获取指定的键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        static public T Get<T>(string key)
        {
            var options = GetSection(key).Get<T>();
            if (options == null)
            {
                return default;
            }
            return options;
        }

        /// <summary>
        /// 创建一份配置文件信息
        /// </summary>
        /// <returns></returns>
        public static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ServiceLoader.Environment.EnvironmentName}.json", optional: true)
                .Build();
        }

        /// <summary>
        /// 根据环境变量组合appsettings.json
        /// 服务器的系统环境变量中需要添加：ASPNETCORE_ENVIRONMENT=Production
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static void Appsettings(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddJsonFile("appsettings.json", optional: false)
                   .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                   .AddEnvironmentVariables();
        }
    }
}
