using Serilog;
using Serilog.Formatting.Compact;
using System.Net;
using TianCheng.Common;

namespace TianCheng.DAL.NpgSqlByEF
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class NpgSqlLog
    {
        #region 默认配置信息
        /// <summary>
        /// Mongo的配置节点名称
        /// </summary>
        private static readonly string SectionName = "SerilogNpgSql";
        /// <summary>
        /// 默认的日志配置节点
        /// </summary>
        private static readonly string DefaultSectionName = "Serilog";
        /// <summary>
        /// 默认的日志文件
        /// </summary>
        private static readonly string FileFormat = "Logs/npgs-{Date}.txt";
        ///// <summary>
        ///// 记录日志的表名
        ///// </summary>
        //private static readonly string LogTableName = "system_log";
        /// <summary>
        /// 默认的发送邮箱账号
        /// </summary>
        private static readonly ICredentialsByHost NetworkCredential = new NetworkCredential("tianchengok2019@163.com", "1234qwer");
        /// <summary>
        /// 接受邮件账号
        /// </summary>
        private static readonly string ToEmail = "17814198@qq.com";
        #endregion

        /// <summary>
        /// 日志操作
        /// </summary>
        public static ILogger Logger { get; private set; }

        /// <summary>
        /// 初始化日志操作对象
        /// </summary>
        static NpgSqlLog()
        {
            try
            {
                // 检查配置是否存在，决定使用哪个配置，优先使用Mongo的日志配置
                string section = SectionName;
                var conf = ServiceLoader.Configuration.GetSection($"{section}:WriteTo:0:Name");
                if (conf == null || string.IsNullOrWhiteSpace(conf.Value))
                {
                    section = DefaultSectionName;
                }
                // 创建日志的配置
                LoggerConfiguration logConf = new LoggerConfiguration().ReadFrom.Configuration(ServiceLoader.Configuration, section).Enrich.WithRequestInfo();
                Logger = logConf.CreateLogger();
            }
            catch
            {
                Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .Enrich.WithThreadId()
                            .Enrich.WithRequestInfo()
                            //.WriteTo.Elasticsearch(Configuration.GetConnectionString("ElasticSearch"), $"logstash-{ApplicationHelper.ApplicationName.ToLower()}")
                            .WriteTo.Console(Serilog.Events.LogEventLevel.Warning)
                            .WriteTo.RollingFile(formatter: new CompactJsonFormatter(), FileFormat)
                            // 发送邮件
                            .WriteTo.Email(
                                fromEmail: "tianchengok2019@163.com",
                                toEmail: ToEmail,
                                mailServer: "smtp.163.com",
                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                                networkCredential: NetworkCredential,
                                outputTemplate: "[{Level}] {NewLine}{Message} {NewLine}{Exception}",
                                mailSubject: "系统错误-提醒邮件")
                            .CreateLogger();
            }
        }
    }
}
