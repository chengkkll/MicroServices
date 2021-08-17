using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 日志操作
    /// </summary>
    /// <remarks>固定配置，课用于全局记录日志</remarks>
    public class GlobalLog
    {
        /// <summary>
        /// 默认的日志文件
        /// </summary>
        static readonly string FileFormat = "logs/global.log";

        /// <summary>
        /// 日志操作
        /// </summary>
        static public ILogger Logger { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        static GlobalLog()
        {
            Logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .Enrich.WithThreadId()
                         .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                         .WriteTo.Debug()
                         .WriteTo.File(formatter: new CompactJsonFormatter(), FileFormat)
                         .CreateLogger();
        }
    }
}
