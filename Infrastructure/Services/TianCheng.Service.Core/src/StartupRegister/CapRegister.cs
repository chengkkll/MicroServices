using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// TianChengConfigureOptions 的扩展方法
    /// </summary>
    static public class CapRegister
    {
        #region Regist Cap Services
        /// <summary>
        /// 注册Cap 使用的Key
        /// </summary>
        static private readonly string CapKey = "CapMQ";

        /// <summary>
        /// 判断是否已注册Cap
        /// </summary>
        /// <returns></returns>
        static public bool HasCap(this TianChengConfigureOptions options)
        {
            return options.HasRegister(CapKey);
        }

        /// <summary>
        /// 注册 CapServices
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sectionName"></param>
        static public void AddCap(this TianChengConfigureOptions options, string sectionName = "Cap")
        {
            // 读取配置信息
            var setting = CapConfiguration.LoadCapOptions(sectionName);
            if (string.IsNullOrWhiteSpace(setting.MQ.Type))
            {
                ServiceLog.Logger.Warning("请配置CAP 的消息队列信息");
                return;
            }

            // 根据配置信息设置Cap
            ServiceLoader.Services.AddCap(x =>
            {
                //if (string.IsNullOrWhiteSpace(setting.DB.Type))
                //{
                //    //ServiceLog.Logger.Warning("请配置CAP 的数据库信息");
                //}
                //else
                //{
                //    string dbType = setting.DB.Type.ToLower();
                //    if (dbType.Contains("mysql"))
                //    {
                //        x.UseMySql(setting.DB.ConnectionString);
                //    }
                //}

                x.UseLiteDBStorage();

                string mqType = setting.MQ.Type.ToLower();
                if (mqType.Contains("rabbit"))
                {
                    x.UseRabbitMQ(options =>
                    {
                        if (!string.IsNullOrWhiteSpace(setting.MQ.HostName))
                        {
                            options.HostName = setting.MQ.HostName;
                        }
                        if (setting.MQ.Port != null)
                        {
                            options.Port = setting.MQ.Port.Value;
                        }
                        if (!string.IsNullOrWhiteSpace(setting.MQ.UserName))
                        {
                            options.UserName = setting.MQ.UserName;
                        }
                        if (!string.IsNullOrWhiteSpace(setting.MQ.Password))
                        {
                            options.Password = setting.MQ.Password;
                        }
                        if (!string.IsNullOrWhiteSpace(setting.MQ.ExchangeName))
                        {
                            options.ExchangeName = setting.MQ.ExchangeName;
                        }
                        if (!string.IsNullOrWhiteSpace(setting.MQ.VirtualHost))
                        {
                            options.VirtualHost = setting.MQ.VirtualHost;
                        }
                    });
                }
                options.Register(CapKey);
            });

        }

        /// <summary>
        /// 使用Cap
        /// </summary>
        /// <param name="options"></param>
        static public void UseCap(this TianChengConfigureOptions options)
        {
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
        #endregion
    }
}
