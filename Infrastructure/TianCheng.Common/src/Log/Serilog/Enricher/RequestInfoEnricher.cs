using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 为日志信息中增加请求信息
    /// </summary>
    public class RequestInfoEnricher : ILogEventEnricher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="propertyFactory"></param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = ServiceLoader.GetService<IHttpContextAccessor>()?.HttpContext;
            if (null != httpContext)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestIP", httpContext.Connection.RemoteIpAddress.ToString()));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", httpContext.Request.Path));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestReferer", httpContext.Request.Headers["Referer"]));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class EnricherExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrich"></param>
        /// <returns></returns>
        public static LoggerConfiguration WithRequestInfo(this Serilog.Configuration.LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<RequestInfoEnricher>();
        }
    }
}
