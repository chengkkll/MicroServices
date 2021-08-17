using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Redis
{
    /// <summary>
    /// 字符串缓存处理
    /// </summary>
    public class StringRedis : BaseRedis
    {
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public bool Set(string key, string val)
        {
            return Do(db => db.StringSet(GetFormatKey(key), val));
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Do(db => db.StringGet(GetFormatKey(key)));
        }
    }

    /// <summary>
    /// 可指定类型的键值缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringRedis<T> : StringRedis where T : new()
    {
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public bool SetObject(string key, T val)
        {
            return Set(key, val.ToJson());
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetObject(string key)
        {
            string json = Get(key);
            return json.JsonToObject<T>();
        }
    }
}
