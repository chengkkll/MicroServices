using Microsoft.Extensions.Logging;
//using ServiceStack.Redis;
using StackExchange.Redis;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Redis.ServiceStack
{
    /// <summary>
    /// Redis基础操作
    /// </summary>
    public abstract class BaseRedis : IDBOperationRegister
    {
        #region 构造方法
        /// <summary>
        /// 连接串
        /// </summary>
        protected readonly DBConnectionOptions Connection;
        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseRedis()
        {
            ConnectionProvider connectionProvider = ServiceLoader.GetService<ConnectionProvider>();
            Connection = connectionProvider.RedisDefault;

            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(this.GetType());
        }
        #endregion

        #region 数据库相关
        /// <summary>
        /// 指定操作的数据库序号
        /// </summary>
        protected virtual long DBIndex
        {
            get { return 0; }
        }

        /// <summary>
        /// 清除当前操作的数据库
        /// </summary>
        public void Clear()
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;
            client.FlushDb();
        }
        #endregion

        #region 工具方法
        /// <summary>
        /// 将一个字符串转成字符数组
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected byte[] GetByteArray(string val)
        {
            return System.Text.Encoding.UTF8.GetBytes(val);
        }

        /// <summary>
        /// 将一个字符数组转成字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected string GetString(byte[] val)
        {
            return System.Text.Encoding.UTF8.GetString(val);
        }
        #endregion

        #region Key相关操作
        /// <summary>
        /// Key的格式
        /// </summary>
        protected virtual string KeyFormat
        {
            get { return "string:default:{0}"; }
        }

        /// <summary>
        /// 根据Key格式来获取key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetFormatKey(string key)
        {
            return string.Format(KeyFormat, key);
        }
        #endregion
    }
}
