using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// 数据库操作错误
    /// </summary>
    public class NpgOperationException : Exception
    {
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// 数据库操作错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public NpgOperationException(Exception ex, string message) : base(message, ex)
        {
            _logger = Serilog.Log.ForContext<NpgOperationException>();

            _logger.Error(ex, "{Messages}", message);
        }

        /// <summary>
        /// 数据库操作错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public NpgOperationException(Exception ex, string message, object model) : base(message, ex)
        {
            _logger = Serilog.Log.ForContext<NpgOperationException>();
            _logger.Error(ex, "{Messages} {@Model}", message, model);
        }
    }
}
