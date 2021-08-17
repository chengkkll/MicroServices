using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// TianChengConfigureOptions 扩展方法
    /// </summary>
    static public class AutoMapperOptionsExt
    {
        #region AutoMapper
        /// <summary>
        /// 注册AutoMapper时使用的Key
        /// </summary>
        private static readonly string AutoMapperKey = "AutoMapper";
        /// <summary>
        /// 判断是否已注册AutoMapper
        /// </summary>
        /// <returns></returns>
        static public bool HasAutoMapper(this TianChengConfigureOptions options)
        {
            return options.HasRegister(AutoMapperKey);
        }
        /// <summary>
        /// 使用AutoMapper
        /// </summary>
        static public void AddAutoMapper(this TianChengConfigureOptions options)
        {
            if (options.HasAutoMapper())
            {
                return;
            }
            // 注册AutoMapper相关服务
            AutoMapperExtension.RegisterAutoMapper();
            // 标记注册完成
            options.Register(AutoMapperKey);
        }
        #endregion
    }
}
