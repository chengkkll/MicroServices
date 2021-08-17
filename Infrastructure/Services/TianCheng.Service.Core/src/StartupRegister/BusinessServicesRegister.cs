using MediatR;
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
    static public class BusinessServicesRegister
    {
        #region Regist Business Services
        /// <summary>
        /// 注册Business Services 使用的Key
        /// </summary>
        static private readonly string BusinessServices = "BusinessServices";

        /// <summary>
        /// 判断是否已注册BusinessServices
        /// </summary>
        /// <returns></returns>
        static public bool HasBusinessServices(this TianChengConfigureOptions options)
        {
            return options.HasRegister(BusinessServices);
        }

        /// <summary>
        /// 注册 BusinessServices
        /// </summary>
        /// <param name="options"></param>
        static public void AddBusinessServices(this TianChengConfigureOptions options)
        {
            // 注册所有服务
            foreach (Type type in AssemblyHelper.GetTypeByInterface<IServiceRegister>())
            {
                if (type.GetTypeInfo().IsClass)
                {
                    ServiceLoader.Services.AddSingleton(type);
                }
            }

            // 注册所有服务的扩展--> 事件的多播委托
            ServiceExtOption.Load(ServiceLoader.Services);

            // 注册列表/过滤条件的显示项操作服务
            ServiceLoader.Services.AddSingleton<IShowItemService, ShowItemServiceRedis>();
            ServiceLoader.Services.AddSingleton<ISearchQueryService, SearchQueryServiceRedis>();

            // 注册MediatR
            ServiceLoader.Services.AddMediatR(AssemblyHelper.GetAssemblyList().ToArray());

            // 标记注册完成
            options.Register(BusinessServices);
        }
        #endregion
    }
}
