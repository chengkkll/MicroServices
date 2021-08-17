using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace TianCheng.Common
{
    //public class SerilogLogger : ICommonLog
    //{
    //    #region Core
    //    /// <summary>
    //    /// Serilog的日志处理
    //    /// </summary>
    //    public ILogger Logger
    //    {
    //        get
    //        {
    //            return _Logger;
    //        }
    //    }

    //    /// <summary>
    //    /// 日志操作对象
    //    /// </summary>
    //    static public ILogger _Logger = null;
    //    /// <summary>
    //    /// 默认的日志文件
    //    /// </summary>
    //    static readonly string FileFormat = "Logs/common-{Date}.txt";
    //    /// <summary>
    //    /// 默认的配置节点名称
    //    /// </summary>
    //    static private string SectionName = "Serilog";

    //    /// <summary>
    //    /// 构造方法
    //    /// </summary>
    //    /// <param name="sectionName"></param>
    //    public SerilogLogger(string sectionName)
    //    {
    //        SetLogger(sectionName);
    //    }

    //    /// <summary>
    //    /// 设置日志操作对象
    //    /// </summary>
    //    public void SetLogger(string sectionName)
    //    {
    //        try
    //        {
    //            if (!string.IsNullOrWhiteSpace(sectionName))
    //            {
    //                SectionName = sectionName;
    //            }

    //            var con = new LoggerConfiguration().ReadFrom.Configuration(ServiceLoader.Configuration, SectionName).Enrich.WithRequestInfo();
    //            _Logger = con.CreateLogger();
    //        }
    //        catch
    //        {
    //            _Logger = new LoggerConfiguration()
    //                        .Enrich.FromLogContext()
    //                        .Enrich.WithThreadId()
    //                        .Enrich.WithRequestInfo()
    //                        .WriteTo.Console(Serilog.Events.LogEventLevel.Information, theme: SystemConsoleTheme.Colored)
    //                        .WriteTo.Debug()
    //                        .WriteTo.RollingFile(formatter: new CompactJsonFormatter(), FileFormat)
    //                        .CreateLogger();
    //        }
    //    }
    //    #endregion

    //    #region Verbose
    //    /// <summary>
    //    /// Verbose
    //    /// </summary>
    //    /// <param name="messageTemplate"></param>
    //    public void Verbose(string messageTemplate)
    //    {
    //        Logger.Verbose(messageTemplate);
    //    }
    //    /// <summary>
    //    /// Verbose
    //    /// </summary>
    //    /// <param name="exception"></param>
    //    /// <param name="messageTemplate"></param>
    //    public void Verbose(Exception exception, string messageTemplate)
    //    {
    //        Logger.Verbose(exception, messageTemplate);
    //    }
    //    #endregion

    //    #region Debug
    //    /// <summary>
    //    /// Debug
    //    /// </summary>
    //    /// <param name="messageTemplate"></param>
    //    public void Debug(string messageTemplate)
    //    {
    //        Logger.Debug(messageTemplate);
    //    }
    //    /// <summary>
    //    /// Debug
    //    /// </summary>
    //    /// <param name="exception"></param>
    //    /// <param name="messageTemplate"></param>
    //    public void Debug(Exception exception, string messageTemplate)
    //    {
    //        Logger.Debug(exception, messageTemplate);
    //    }
    //    #endregion

    //    #region Information
    //    /// <summary>
    //    /// Information
    //    /// </summary>
    //    /// <param name="messageTemplate"></param>
    //    public void Information(string messageTemplate)
    //    {
    //        Logger.Information(messageTemplate);
    //    }
    //    /// <summary>
    //    /// Information
    //    /// </summary>
    //    /// <param name="exception"></param>
    //    /// <param name="messageTemplate"></param>
    //    public void Information(Exception exception, string messageTemplate)
    //    {
    //        Logger.Information(exception, messageTemplate);
    //    }
    //    #endregion

    //    #region Warning
    //    /// <summary>
    //    /// Warning
    //    /// </summary>
    //    /// <param name="messageTemplate"></param>
    //    public void Warning(string messageTemplate)
    //    {
    //        Logger.Warning(messageTemplate);
    //    }
    //    /// <summary>
    //    /// Warning
    //    /// </summary>
    //    /// <param name="exception"></param>
    //    /// <param name="messageTemplate"></param>
    //    public void Warning(Exception exception, string messageTemplate)
    //    {
    //        Logger.Warning(exception, messageTemplate);
    //    }
    //    #endregion

    //    #region Error
    //    /// <summary>
    //    /// Error
    //    /// </summary>
    //    /// <param name="messageTemplate"></param>
    //    public void Error(string messageTemplate)
    //    {
    //        Logger.Error(messageTemplate);
    //    }
    //    /// <summary>
    //    /// Error
    //    /// </summary>
    //    /// <param name="exception"></param>
    //    /// <param name="messageTemplate"></param>
    //    public void Error(Exception exception, string messageTemplate)
    //    {
    //        Logger.Error(exception, messageTemplate);
    //    }
    //    #endregion

    //    #region Fatal
    //    /// <summary>
    //    /// Fatal
    //    /// </summary>
    //    /// <param name="messageTemplate"></param>
    //    public void Fatal(string messageTemplate)
    //    {
    //        Logger.Fatal(messageTemplate);
    //    }
    //    /// <summary>
    //    /// Fatal
    //    /// </summary>
    //    /// <param name="exception"></param>
    //    /// <param name="messageTemplate"></param>
    //    public void Fatal(Exception exception, string messageTemplate)
    //    {
    //        Logger.Fatal(exception, messageTemplate);
    //    }
    //    #endregion
    //}
}
