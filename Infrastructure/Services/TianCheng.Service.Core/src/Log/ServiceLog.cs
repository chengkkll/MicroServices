using Serilog;
using System.Net;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 服务通用日志
    /// </summary>
    public class ServiceLog
    {
        /// <summary>
        /// 日志操作对象
        /// </summary>
        static private ILogger _Logger = null;
        /// <summary>
        /// 日志操作
        /// </summary>
        static public ILogger Logger
        {
            get
            {
                if (_Logger == null)
                {
                    // 初始化日志对象
                    _Logger = Configuration.CreateLogger();
                }
                return _Logger;
            }
        }
        public static readonly string FileFormat = "Logs/TianCheng.NpgDB-{Date}.txt";

        /// <summary>
        /// 默认的发送邮箱账号
        /// </summary>
        private static readonly ICredentialsByHost NetworkCredential = new NetworkCredential("tianchengok2019@163.com", "1234qwer");
        /// <summary>
        /// 接受邮件账号
        /// </summary>
        static private string toEmail = "17814198@qq.com";

        /// <summary>
        /// 设置接受邮件的账号
        /// </summary>
        static public string ToEmail
        {
            set
            {
                toEmail = value;
                _Logger = Configuration.CreateLogger();
            }
        }

        /// <summary>
        /// 日志的配置信息
        /// </summary>
        private static LoggerConfiguration Configuration
        {
            get
            {
                return new LoggerConfiguration()
                        .WriteTo.Console(Serilog.Events.LogEventLevel.Warning)
                        .WriteTo.Debug()
                        .WriteTo.RollingFile(FileFormat, Serilog.Events.LogEventLevel.Warning)
                        .WriteTo.Email(
                            fromEmail: "tianchengok2019@163.com",
                            toEmail: toEmail,
                            mailServer: "smtp.163.com",
                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal,
                            networkCredential: NetworkCredential,
                            outputTemplate: "[{Level}] {NewLine}{Message} {NewLine}{Exception}",
                            mailSubject: "系统错误-提醒邮件");
            }
        }
    }
}
