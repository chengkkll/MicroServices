using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TianCheng.Common;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// TianChengConfigureOptions 扩展方法
    /// </summary>
    static public class HealthRegister
    {
        #region Health
        /// <summary>
        /// 注册Health时使用的Key
        /// </summary>
        private static readonly string HealthKey = "Health";
        /// <summary>
        /// 判断是否已注册HasHealth
        /// </summary>
        /// <returns></returns>
        static public bool HasHealth(this TianChengConfigureOptions options)
        {
            return options.HasRegister(HealthKey);
        }
        /// <summary>
        /// 使用Health
        /// </summary>
        static public void AddHasHealth(this TianChengConfigureOptions options)
        {
            if (options.HasHealth())
            {
                return;
            }
            // 注册Health相关服务
            ServiceLoader.Services.AddHealthChecks();
            // 标记注册完成
            options.Register(HealthKey);
        }

        /// <summary>
        /// 使用Health
        /// </summary>
        /// <param name="options"></param>
        /// <param name="path"></param>
        static public void UseHealth(this TianChengConfigureOptions options, string path = "/health")
        {
            if (options.HasHealth())
            {
                options.App.UseHealthChecks(path);
            }
        }
        #endregion
    }
}
