using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// TianChengConfigureOptions 的扩展方法
    /// </summary>
    static public class TianChengConfigureOptionsExt
    {
        #region Auth Jwt
        /// <summary>
        /// 注册AuthJwt的Key
        /// </summary>
        static private readonly string AuthJwt = "AuthJwt";

        /// <summary>
        /// 判断是否已注册AuthJwt
        /// </summary>
        /// <returns></returns>
        static public bool HasAuthJwt(this TianChengConfigureOptions options)
        {
            return options.HasRegister(AuthJwt);
        }
        /// <summary>
        /// 注册Jwt权限服务
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sectionName">consul配置节点名称</param>
        /// <remarks>如果使用AddTianChengService，则无需在这里引用</remarks>
        static public void AddAuthJwt(this TianChengConfigureOptions options, string sectionName = "Token")
        {
            // 注册 AuthJwt
            AuthRegister.ConfigureServices(sectionName);
            // 标记操作完成
            options.Register(AuthJwt);
        }

        /// <summary>
        /// 使用Jwt权限服务
        /// </summary>
        /// <param name="options"></param>
        /// <remarks>如果使用AddTianChengService，则无需在这里引用</remarks>
        static public void UseAuthJwt(this TianChengConfigureOptions options)
        {
            if (options.HasAuthJwt())
            {
                // 使用 AuthJwt （在请求管道中增加处理）
                AuthRegister.ConfigurePipeline(options.App);
            }
        }
        #endregion
    }
}
