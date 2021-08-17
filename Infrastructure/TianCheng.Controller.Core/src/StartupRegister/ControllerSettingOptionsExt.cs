using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using TianCheng.Common;
using TianCheng.Controller.Core.PlugIn.RoutePrefix;
using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// TianChengConfigureOptions 扩展方法
    /// </summary>
    static public class ControllerSettingOptionsExt
    {
        #region ControllerSetting
        /// <summary>
        /// 注册ControllerSetting时使用的Key
        /// </summary>
        private static readonly string ControllerSettingKey = "ControllerSetting";
        /// <summary>
        /// 判断是否已注册HasControllerSetting
        /// </summary>
        /// <returns></returns>
        static public bool HasControllerSetting(this TianChengConfigureOptions options)
        {
            return options.HasRegister(ControllerSettingKey);
        }
        /// <summary>
        /// 使用ControllerSetting
        /// </summary>
        /// <param name="options"></param>
        /// <param name="routePrefix">路由前置信息</param>
        static public void AddControllerSetting(this TianChengConfigureOptions options, string routePrefix = "")
        {
            if (options.HasControllerSetting())
            {
                return;
            }

            ServiceLoader.Services.AddOptions();

            // 序列化操作
            ServiceLoader.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                // 修改属性名称的序列化方式，首字母小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // 增加异常过滤处理
            ServiceLoader.Services.AddMvc(options =>
            {
                options.Filters.Add(typeof(TianCheng.Controller.Core.WebApiExceptionFilterAttribute));    // 增加异常处理
                if (!string.IsNullOrWhiteSpace(routePrefix))
                {
                    options.SetRoutePrefix(routePrefix);
                }
            });

            // 标记注册完成
            options.Register(ControllerSettingKey);

            // 如果使用Cap 完善Cap的Web配置
            if (options.HasCap())
            {
                ServiceLoader.Services.AddCap(x =>
                {
                    // 设置UI显示
                    x.UseDashboard();
                    // 配置Consul信息，可以切换服务进行查看
                    var capOptions = CapConfiguration.LoadCapOptions();
                    x.UseDiscovery(d =>
                    {
                        d.DiscoveryServerHostName = capOptions.Consul.DiscoveryServerHostName;
                        d.DiscoveryServerPort = capOptions.Consul.DiscoveryServerPort;
                        d.CurrentNodeHostName = capOptions.Consul.CurrentNodeHostName;
                        d.CurrentNodePort = capOptions.Consul.CurrentNodePort;
                        d.NodeId = capOptions.Consul.NodeId + "_CAP";
                        d.NodeName = capOptions.Consul.NodeName + "_CAP";
                    });
                });
            }
        }

        /// <summary>
        /// 使用ControllerSetting
        /// </summary>
        /// <param name="options"></param>
        static public void UseControllerSetting(this TianChengConfigureOptions options)
        {
            if (options.HasControllerSetting())
            {
                var env = ServiceLoader.GetService<IWebHostEnvironment>();
                if (env.IsDevelopment())
                {
                    options.App.UseDeveloperExceptionPage();
                }

                options.App.UseRouting();
            }
            //if (options.HasCap())
            //{
            //    options.App.UseCap();
            //}
        }
        #endregion
    }
}
