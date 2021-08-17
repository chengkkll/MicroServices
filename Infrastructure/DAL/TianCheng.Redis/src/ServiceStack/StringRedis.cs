//using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Redis.ServiceStack
{
    /// <summary>
    /// 字符串缓存处理
    /// </summary>
    public class StringRedis : StringRedis<string>
    {

    }

    /// <summary>
    /// 可指定类型的键值缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringRedis<T> : BaseRedis
    {
        /// <summary>
        /// 根据Key格式来获取key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetFormatKey(string key)
        {
            return string.Format(KeyFormat, key);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public bool Set(string key, T val)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.Set<T>(GetFormatKey(key), val);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get(string key)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.Get<T>(GetFormatKey(key));
        }
    }
}
