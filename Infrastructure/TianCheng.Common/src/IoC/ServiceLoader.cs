using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TianCheng.Common
{
    /// <summary>
    /// 获取服务对象
    /// </summary>
    public static class ServiceLoader
    {
        /// <summary>
        /// 
        /// </summary>
        public static IApplicationBuilder ApplicationBuilder { get; set; }
        /// <summary>
        /// 获取IServiceProvider
        /// </summary>
        public static IServiceProvider Instance { get; set; }
        /// <summary>
        /// 获取IServiceCollection
        /// </summary>
        public static IServiceCollection Services { get; set; }

        private static IConfiguration configuration = null;
        /// <summary>
        /// 获取Configuration
        /// </summary>
        public static IConfiguration Configuration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = TianCheng.Common.Configuration.BuildConfiguration();
                }
                return configuration;
            }
            set
            {
                configuration = value;
            }
        }
        /// <summary>
        /// 获取系统的环境变量
        /// </summary>
        static public IHostingEnvironment Environment
        {
            get
            {
                return GetService<IHostingEnvironment>();
            }
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return Instance.GetService<T>();
        }

        /// <summary>
        /// 根据接口获取服务
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetService<T>(string typeName)
        {
            foreach (var service in Instance.GetService<IEnumerable<T>>())
            {
                if (service.GetType().FullName.Contains(typeName))
                {
                    return service;
                }
            }
            return default;
        }

        /// <summary>
        /// 根据类型名称获取服务
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object GetService(string typeName)
        {
            foreach (var ser in Services)
            {
                if (typeName.Contains(ser.ServiceType.Name))
                {
                    return Instance.GetService(ser.ServiceType);
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IHostingEnvironment GetEnviroment()
        {
            return Instance.GetService<IHostingEnvironment>();
        }

    }
}
