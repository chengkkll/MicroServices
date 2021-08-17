using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// 数据库连接异常
    /// </summary>
    public class NpgConnectionException : Exception
    {
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="connectionString"></param>
        public NpgConnectionException(Exception ex, string connectionString)
        {
            _logger = Serilog.Log.ForContext<NpgConnectionException>();

            if (ex != null || ex.InnerException is TimeoutException)
            {
                _logger.Error(ex, "{Messages}",$"数据库链接超时。链接字符串：{connectionString}");
            }
            else
            {
                _logger.Error(ex, "{Messages}", $"数据库链接错误。链接字符串：{connectionString}");
            }
        }
    }
}
