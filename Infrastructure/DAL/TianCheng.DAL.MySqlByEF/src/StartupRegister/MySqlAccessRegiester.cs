using Microsoft.Extensions.DependencyInjection;
using System;
using TianCheng.Common;

namespace TianCheng.DAL.MySqlByEF
{
    /// <summary>
    /// TianChengConfigureOptions 的扩展方法
    /// </summary>
    static public class MySqlAccessRegiester
    {
        #region Db Services
        /// <summary>
        /// 注册DB Services的Key
        /// </summary>
        static private readonly string MySqlAccess = "MySqlAccess";

        /// <summary>
        /// 判断是否已注册DB服务
        /// </summary>
        /// <returns></returns>
        static public bool HasMySqlAccess(this TianChengConfigureOptions options)
        {
            return options.HasRegister(MySqlAccess);
        }

        /// <summary>
        /// 注册数据库操作服务
        /// </summary>
        /// <param name="options"></param>
        /// <remarks>如果使用AddTianChengService，则无需在这里引用</remarks>
        static public void AddMySqlAccess(this TianChengConfigureOptions options)
        {
            if (!options.HasDBServices())
            {
                options.AddDbServices();
            }
            // 注册数据库操作上下文对象
            ServiceLoader.Services.AddDbContext<MysqlContext>();
            foreach (Type type in AssemblyHelper.GetTypeByBaseClassName("MysqlContext"))
            {
                if (!type.IsInterface && !type.IsAbstract)
                {
                    ServiceLoader.Services.AddSingleton(type);
                }
            }

            // 标记注册完成
            options.Register(MySqlAccess);
        }

        /// <summary>
        /// 使用Db服务
        /// </summary>
        /// <param name="options"></param>
        static public void UseDbServices(this TianChengConfigureOptions options)
        {
            if (options.HasMySqlAccess())
            {

            }
        }
        #endregion
    }
}
