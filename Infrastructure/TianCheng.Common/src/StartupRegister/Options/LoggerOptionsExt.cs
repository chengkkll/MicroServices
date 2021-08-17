using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// TianChengConfigureOptions 扩展方法
    /// </summary>
    static public class LoggerOptionsExt
    {
        #region Serilog
        /// <summary>
        /// 注册Serilog时使用的Key
        /// </summary>
        private static readonly string SerilogKey = "Serilog";
        /// <summary>
        /// 判断是否已注册Serilog
        /// </summary>
        /// <returns></returns>
        static public bool HasSerilog(this TianChengConfigureOptions options)
        {
            return options.HasRegister(SerilogKey);
        }
        /// <summary>
        /// 注册Serilog
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sectionName">配置consul节点的名称</param>
        static public void AddSerilog(this TianChengConfigureOptions options, string sectionName = "Serilog")
        {
            if (options.HasSerilog())
            {
                return;
            }
            // 初始化通用日志对象
            Serilog.CommonLog.Init();
            // 初始化默认Serilog日志
            DefaultSerilog.Init(sectionName);
            // 标记注册完成
            options.Register(SerilogKey);
        }

        /// <summary>
        /// 使用Serilog
        /// </summary>
        /// <param name="options"></param>
        static public void UseSerilog(this TianChengConfigureOptions options)
        {
            if (options.HasSerilog())
            {
                options.App.UseSerilogRequestLogging();
            }
        }
        #endregion
    }
}
