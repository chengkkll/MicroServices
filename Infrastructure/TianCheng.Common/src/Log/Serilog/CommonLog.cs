using Serilog.Formatting.Compact;
using TianCheng.Common;

namespace Serilog
{
    /// <summary>
    /// 增加一个通用的日志处理
    /// </summary>
    static public class CommonLog
    {
        /// <summary>
        /// 默认的日志文件
        /// </summary>
        private static readonly string FileFormat = "Logs/common-{Date}.log";

        /// <summary>
        /// 日志操作
        /// </summary>
        static public ILogger Logger { get; private set; }

        /// <summary>
        /// 设置日志操作对象
        /// </summary>
        /// <param name="sectionName">默认配置所在的节点名称</param>
        static public void Init(string sectionName = "SerilogCommon")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(sectionName))
                {
                    sectionName = "SerilogCommon";
                }

                var con = new LoggerConfiguration().ReadFrom.Configuration(ServiceLoader.Configuration, sectionName).Enrich.WithRequestInfo();
                Logger = con.CreateLogger();
            }
            catch
            {
                Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .Enrich.WithThreadId()
                            .Enrich.WithRequestInfo()
                            //.WriteTo.Elasticsearch(Configuration.GetConnectionString("ElasticSearch"), $"logstash-{ApplicationHelper.ApplicationName.ToLower()}")
                            .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                            .WriteTo.Debug()
                            .WriteTo.RollingFile(formatter: new CompactJsonFormatter(), FileFormat)
                            .CreateLogger();
            }
        }
    }
}