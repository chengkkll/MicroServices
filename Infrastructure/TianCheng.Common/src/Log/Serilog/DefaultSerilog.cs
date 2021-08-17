using Serilog;
using Serilog.Formatting.Compact;

namespace TianCheng.Common
{
    /// <summary>
    /// 配置Serilog默认的日志对象
    /// </summary>
    static public class DefaultSerilog
    {
        /// <summary>
        /// 默认的日志文件
        /// </summary>
        static readonly string FileFormat = "Logs/logger-{Date}.log";

        /// <summary>
        /// 设置日志操作对象
        /// </summary>
        /// <param name="sectionName">默认配置所在的节点名称</param>
        static public void Init(string sectionName = "Serilog")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(sectionName))
                {
                    sectionName = "Serilog";
                }

                var con = new LoggerConfiguration().ReadFrom.Configuration(ServiceLoader.Configuration, sectionName);
                Log.Logger = con.CreateLogger();
            }
            catch
            {
                Log.Logger = new LoggerConfiguration()
                                .Enrich.FromLogContext()
                                .Enrich.WithThreadId()
                                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                                .WriteTo.Debug()
                                .WriteTo.RollingFile(formatter: new CompactJsonFormatter(), FileFormat)
                                .CreateLogger();
            }
        }
    }
}

