using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using TianCheng.Common;
using TianCheng.DAL;
using System.Linq;
using System.Collections.Generic;

namespace TianCheng.Redis
{
    /// <summary>
    /// Redis基础操作
    /// </summary>
    public abstract class BaseRedis : IDBOperationRegister
    {
        #region 构造及连接处理
        /// <summary>
        /// 连接串
        /// </summary>
        static private readonly DBConnectionOptions ConnOptions;
        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseRedis()
        {
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(this.GetType());
        }
        /// <summary>
        /// 静态构造方法
        /// </summary>
        static BaseRedis()
        {
            ConnectionProvider connectionProvider = ServiceLoader.GetService<ConnectionProvider>();
            ConnOptions = connectionProvider.RedisDefault;
        }
        /// <summary>
        /// 懒加载数据库连接
        /// </summary>
        private static Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(ConnOptions.ConnectionString);
        });

        /// <summary>
        /// 获取一个数据库操作对象
        /// </summary>
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return LazyConnection.Value;
            }
        }
        #endregion

        #region 数据库连接
        /// <summary>
        /// 指定操作的数据库序号
        /// </summary>
        protected virtual int DBIndex
        {
            get { return 0; }
        }

        /// <summary>
        /// 执行数据操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T Do<T>(Func<IDatabase, T> func)
        {
            var client = Connection;
            var db = client.GetDatabase(DBIndex);
            return func(db);
        }

        /// <summary>
        /// 执行数据操作
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected void Do(Action<IDatabase> operation)
        {
            var client = Connection;
            var db = client.GetDatabase(DBIndex);
            operation(db);
        }

        protected T Command<T>(Func<IServer, T> func)
        {
            var client = Connection;
            var server = client.GetServer(ConnOptions.Database, ConnOptions.Port);
            return func(server);
        }

        #endregion

        #region 数据库相关
        /// <summary>
        /// 清除当前操作的数据库
        /// </summary>
        public void Clear()
        {
            Command(server => { server.FlushDatabase(DBIndex); return true; });
            //var client = Connection;
            //client.GetServer(ConnOptions.Database, ConnOptions.Port).FlushDatabase(DBIndex);
        }

        /// <summary>
        /// 模糊查询Key
        /// </summary>
        /// <param name="pattern"></param>
        /// <remarks>
        ///     h?llo 匹配 hello, hallo 和 hxllo
        ///     h* llo 匹配 hllo 和 heeeello
        ///     h[ae] llo 匹配 hello 和 hallo, 但是不匹配 hillo
        ///     h[^e]llo 匹配 hallo, hbllo, … 但是不匹配 hello
        ///     h[a - b] llo 匹配 hallo 和 hbllo
        /// </remarks>
        /// <returns></returns>
        public List<string> DBKeys(string pattern)
        {
            List<string> result = new List<string>();
            foreach (var item in Command(server => server.Keys(DBIndex, pattern)))
            {
                result.Add(item);
            }
            return result;
        }
        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            return Do(db => db.KeyDelete(key));
        }
        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Delete(List<string> keys)
        {
            RedisKey[] rk = new RedisKey[keys.Count];
            for (int i = 0; i < keys.Count; i++)
            {
                rk.SetValue(keys[i], i);
            }
            return Do(db => db.KeyDelete(rk));
        }

        /// <summary>
        /// 删除匹配项的Key
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public long DeleteLikeKey(string pattern)
        {
            var keys = Command(server => server.Keys(DBIndex, pattern));
            return Do(db => db.KeyDelete(keys.ToArray()));
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

        //protected RedisKey GetRedisKey(string key)
        //{
        //    return new RedisKey(string.Format(KeyFormat, key));
        //}
        #endregion
    }
}
