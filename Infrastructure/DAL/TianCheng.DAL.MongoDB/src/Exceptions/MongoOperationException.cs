using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// 数据库操作错误
    /// </summary>
    public class MongoOperationException : Exception
    {
        /// <summary>
        /// 异常的消息内容
        /// </summary>
        private const string ExceptionMessage = "操作异常终止";

        /// <summary>
        /// 日志的异常模板
        /// </summary>
        private const string MessageTemplate = "操作异常终止,{Message}";

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="ex">捕获的异常</param>
        /// <param name="message">异常信息</param>
        /// <param name="logger">异常信息</param>
        public MongoOperationException(Exception ex, string message = "", ILogger logger = null) : base(ExceptionMessage, ex)
        {
            if (logger == null)
            {
                logger = MongoLog.Logger;
            }
            logger.Error(ex, MessageTemplate, message);
        }
    }
}
