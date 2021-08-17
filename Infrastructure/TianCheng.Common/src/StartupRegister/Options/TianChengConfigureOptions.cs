using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class TianChengConfigureOptions
    {
        #region 构造方法
        /// <summary>
        /// 使用单例模式
        /// </summary>
        private static TianChengConfigureOptions instance = null;
        /// <summary>
        /// 获取配置实例
        /// </summary>
        public static TianChengConfigureOptions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TianChengConfigureOptions();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        #endregion

        #region IApplicationBuilder
        /// <summary>
        /// App
        /// </summary>
        public IApplicationBuilder App { get; private set; }

        /// <summary>
        /// 设置 IApplicationBuilder 
        /// </summary>
        /// <param name="app"></param>
        public TianChengConfigureOptions SetApp(IApplicationBuilder app)
        {
            App = app;
            return this;
        }
        #endregion

        #region 判断指定中间件是否使用
        /// <summary>
        /// 中间件使用字典
        /// </summary>
        private static readonly IDictionary<string, bool> RegisterDict = new Dictionary<string, bool>();
        /// <summary>
        /// 设置使用的中间件名称
        /// </summary>
        /// <param name="middlewareName"></param>
        public void Register(string middlewareName)
        {
            if (RegisterDict.ContainsKey(middlewareName))
            {
                RegisterDict.Remove(middlewareName);
            }
            RegisterDict.Add(middlewareName, true);
        }
        /// <summary>
        /// 判断中间件是否使用
        /// </summary>
        /// <param name="middlewareName"></param>
        /// <returns></returns>
        public bool HasRegister(string middlewareName)
        {
            return RegisterDict.ContainsKey(middlewareName);
        }
        #endregion
    }

}
