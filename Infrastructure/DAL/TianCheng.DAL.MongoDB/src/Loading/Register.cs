using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using Serilog;
using System;
using TianCheng.Common;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// 为MongoDB操作注册全局信息
    /// </summary>
    public class Register
    {
        static private bool IsRegister = false;
        /// <summary>
        /// 注册UTC时间转换
        /// </summary>
        static public void Init()
        {
            if (IsRegister) return;
            try
            {
                // 注册MongoDB的UTC时间转换操作
                BsonSerializer.RegisterSerializer(typeof(DateTime), new MongoDateTimeSerializer());
                IsRegister = true;
            }
            catch (Exception ex)
            {
                var Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(new Register().GetType());
                Log.LogError(ex, "注册MongoDB的UTC时间转换时出错");
            }
        }
    }
}
