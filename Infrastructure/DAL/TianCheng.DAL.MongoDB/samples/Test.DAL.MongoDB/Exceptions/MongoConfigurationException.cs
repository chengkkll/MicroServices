using System;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// 数据库配置异常
    /// </summary>
    public class MongoConfigurationException : Exception
    {
        /// <summary>
        /// 异常的消息内容
        /// </summary>
        private const string ExceptionMessage = "无法找到数据库配置信息";

        /// <summary>
        /// 构造方法
        /// </summary>
        public MongoConfigurationException() : base(ExceptionMessage)
        {
            MongoLog.Logger.Fatal(ExceptionMessage);
        }
    }
}
