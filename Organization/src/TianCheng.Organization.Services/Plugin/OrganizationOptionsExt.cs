using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.Organization.Model;
using TianCheng.Organization.Services;

namespace TianCheng.Organization
{
    /// <summary>
    /// TianChengConfigureOptions 扩展方法
    /// </summary>
    static public class OrganizationOptionsExt
    {
        #region Organization
        /// <summary>
        /// 注册Organization时使用的Key
        /// </summary>
        private static readonly string OrganizationKey = "Organization";
        /// <summary>
        /// 判断是否已注册Organization
        /// </summary>
        /// <returns></returns>
        static public bool HasOrganization(this TianChengConfigureOptions options)
        {
            return options.HasRegister(OrganizationKey);
        }
        /// <summary>
        /// 使用Organization
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sectionName">存储功能节点的配置名称</param>
        static public void AddOrganization(this TianChengConfigureOptions options, string sectionName = "FunctionModule")
        {
            if (options.HasOrganization())
            {
                return;
            }
            // 注册Organization相关服务
            ServiceLoader.Services.Configure<FunctionModuleConfig>(ServiceLoader.Configuration.GetSection(sectionName));
            ServiceLoader.Services.AddSingleton<FunctionConfigureService>();

            // 标记注册完成
            options.Register(OrganizationKey);
        }
        #endregion
    }
}
