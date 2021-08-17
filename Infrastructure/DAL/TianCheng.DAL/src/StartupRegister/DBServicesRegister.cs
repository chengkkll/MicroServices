using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.DAL
{
    /// <summary>
    /// TianChengConfigureOptions 的扩展方法
    /// </summary>
    static public class DBServicesRegister
    {
        #region Db Services
        /// <summary>
        /// 注册DB Services的Key
        /// </summary>
        static private readonly string DBServices = "DBServices";

        /// <summary>
        /// 判断是否已注册DB服务
        /// </summary>
        /// <returns></returns>
        static public bool HasDBServices(this TianChengConfigureOptions options)
        {
            return options.HasRegister(DBServices);
        }

        /// <summary>
        /// 注册数据库操作服务
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sectionName">数据库配置节点名称</param>
        /// <remarks>如果使用AddTianChengService，则无需在这里引用</remarks>
        static public void AddDbServices(this TianChengConfigureOptions options, string sectionName = "DBConnection")
        {
            // 增加对象转换的注册
            if (!options.HasAutoMapper())
            {
                options.AddAutoMapper();
            }

            // 注册数据库配置节点信息
            ServiceLoader.Services.Configure<List<DBConnectionOptions>>(ServiceLoader.Configuration.GetSection(sectionName));
            ServiceLoader.Services.AddSingleton<ConnectionProvider>();

            // 注册数据库操作
            foreach (Type type in AssemblyHelper.GetTypeByInterface<IDBOperationRegister>())
            {
                if (!type.IsInterface && !type.IsAbstract)
                {
                    ServiceLoader.Services.AddSingleton(type);
                }
            }

            // 标记注册完成
            options.Register(DBServices);
        }

        /// <summary>
        /// 使用Db服务
        /// </summary>
        /// <param name="options"></param>
        static public void UseDbServices(this TianChengConfigureOptions options)
        {
            if (options.HasDBServices())
            {
                ServiceLoader.GetService<ConnectionProvider>();
            }
        }
        #endregion
    }
}
