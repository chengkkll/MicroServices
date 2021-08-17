using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using TianCheng.Common;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// TianChengConfigureOptions 扩展方法
    /// </summary>
    static public class CorsRegister
    {
        #region Cors
        /// <summary>
        /// 注册Cors时使用的Key
        /// </summary>
        private static readonly string CorsKey = "Cors";
        /// <summary>
        /// 判断是否已注册HasCors
        /// </summary>
        /// <returns></returns>
        static public bool HasCors(this TianChengConfigureOptions options)
        {
            return options.HasRegister(CorsKey);
        }

        static private readonly string DefaultCorsPolicyName = "AllowAll";
        /// <summary>
        /// 使用Cors
        /// </summary>
        static public void AddHasCors(this TianChengConfigureOptions options, Action<CorsPolicyBuilder> configurePolicy = null)
        {
            if (options.HasCors())
            {
                return;
            }
            // 注册Cors相关服务
            if (configurePolicy == null)
            {
                ServiceLoader.Services.AddCors(confg =>
                    confg.AddPolicy(DefaultCorsPolicyName,
                        p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            }
            else
            {
                ServiceLoader.Services.AddCors(confg => confg.AddPolicy(DefaultCorsPolicyName, configurePolicy));
            }

            // 标记注册完成
            options.Register(CorsKey);
        }

        /// <summary>
        /// 使用Cors
        /// </summary>
        /// <param name="options"></param>
        /// <param name="policyName"></param>
        static public void UseCors(this TianChengConfigureOptions options, string policyName = "")
        {
            if (options.HasCors())
            {
                if (string.IsNullOrWhiteSpace(policyName))
                {
                    policyName = DefaultCorsPolicyName;
                }
                options.App.UseCors(policyName);
            }
        }
        #endregion
    }
}
