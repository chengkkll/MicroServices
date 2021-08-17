using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    ///  通用的配置读取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// 使用前，需要先注册配置信息，例如：
    ///     ServiceLoader.Services.Configure《T》(ServiceLoader.Configuration.GetSection(sectionName));
    /// </remarks>
    public class BaseConfigureService<T> where T : new()
    {
        /// <summary>
        /// 设置可热更新的配置操作
        /// </summary>
        private readonly IOptionsMonitor<T> Monitor;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="monitor"></param>
        public BaseConfigureService(IOptionsMonitor<T> monitor)
        {
            Monitor = monitor;
            Options = Assembling(Monitor.CurrentValue);
            if (Options == null)
            {
                Options = Assembling(DefaultOptions());
            }
            // 如果配置信息改了。重新更新配置信息
            Monitor.OnChange(options =>
            {
                Options = Assembling(options);
            });
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        public T Options { get; private set; }

        /// <summary>
        /// 默认的配置信息
        /// </summary>
        /// <returns></returns>
        protected virtual T DefaultOptions()
        {
            return new T();
        }
        /// <summary>
        /// 组装配置信息
        /// </summary>
        /// <param name="optons"></param>
        /// <returns></returns>
        protected virtual T Assembling(T optons)
        {
            return optons;
        }
    }
}
