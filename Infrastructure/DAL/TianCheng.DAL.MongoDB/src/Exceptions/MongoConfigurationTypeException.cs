using System;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// 数据库访问对象配置异常
    /// </summary>
    public class MongoConfigurationTypeException : Exception
    {
        /// <summary>
        /// 异常的消息内容
        /// </summary>
        private const string ExceptionMessage = "数据库访问对象配置异常，请检查DBMapping特性和数据库配置";
        /// <summary>
        /// 日志的异常模板
        /// </summary>
        private const string MessageTemplate = "指定的对象{TypeName}没有对应的数据库操作服务，请检查DBMapping特性和数据库配置";

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="typeName">异常信息</param>
        public MongoConfigurationTypeException(string typeName) : base(ExceptionMessage)
        {
            MongoLog.Logger.Fatal(MessageTemplate, typeName);
        }
    }
}
