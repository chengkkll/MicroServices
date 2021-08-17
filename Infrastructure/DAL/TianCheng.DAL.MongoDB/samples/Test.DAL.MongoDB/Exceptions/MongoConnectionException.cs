using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// 数据库连接异常
    /// </summary>
    public class MongoConnectionException : Exception
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="ex">捕获的异常</param>
        /// <param name="connectionString">数据库连接字符串</param>
        public MongoConnectionException(Exception ex, string connectionString)
        {
            if (ex != null || ex.InnerException is TimeoutException)
            {
                MongoLog.Logger.Fatal(ex, "数据库链接超时。链接字符串：{ConnectionString}", connectionString);
            }
            else
            {
                MongoLog.Logger.Fatal(ex, "数据库链接错误。链接字符串：{ConnectionString}", connectionString);
            }
        }
    }
}
