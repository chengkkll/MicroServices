using System;
using Microsoft.Extensions.DependencyInjection;
using TianCheng.Common;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// TianChengConfigureOptions 的扩展方法
    /// </summary>
    static public class MongoRegiester
    {
        #region Db Services
        /// <summary>
        /// 注册DB Services的Key
        /// </summary>
        static private readonly string MongoAccess = "MongoAccess";

        /// <summary>
        /// 判断是否已注册DB服务
        /// </summary>
        /// <returns></returns>
        static public bool HasMongoAccess(this TianChengConfigureOptions options)
        {
            return options.HasRegister(MongoAccess);
        }

        /// <summary>
        /// 注册数据库操作服务
        /// </summary>
        /// <param name="options"></param>
        /// <remarks>如果使用AddTianChengService，则无需在这里引用</remarks>
        static public void AddMongoAccess(this TianChengConfigureOptions options)
        {
            if (!options.HasDBServices())
            {
                options.AddDbServices();
            }

            // 标记注册完成
            options.Register(MongoAccess);
        }

        /// <summary>
        /// 使用Db服务
        /// </summary>
        /// <param name="options"></param>
        static public void UseMongoAccess(this TianChengConfigureOptions options)
        {
            if (options.HasMongoAccess())
            {

            }
        }
        #endregion
    }
}
