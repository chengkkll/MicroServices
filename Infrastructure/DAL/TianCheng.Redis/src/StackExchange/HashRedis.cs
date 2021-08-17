using Microsoft.Extensions.Logging;
using StackExchange.Redis;
//using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Redis
{
    /// <summary>
    /// Hash缓存的通用处理
    /// </summary>
    public class HashRedis : BaseRedis
    {
        /// <summary>
        /// 添加/修改值      [对应 HSET]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public bool Set(string key, string field, string val)
        {
            // 入参检查
            if (string.IsNullOrWhiteSpace(key))
            {
                Log.LogError("Reids Hash的Set操作错误。\r\n给定的key不能为空：[{TypeName}]", this.GetType().FullName);
            }
            if (string.IsNullOrWhiteSpace(field))
            {
                Log.LogError("Reids Hash的Set操作错误。\r\n给定的field不能为空：[{TypeName}]", this.GetType().FullName);
            }

            return Do(db => db.HashSet(GetFormatKey(key), field, val));
        }
        /// <summary>
        /// 添加/修改值      [对应 HSET]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldVal"></param>
        public void Set(string key, IDictionary<string, string> fieldVal)
        {
            // 入参检查
            if (string.IsNullOrWhiteSpace(key))
            {
                Log.LogError("Reids Hash的Set操作错误。\r\n给定的key不能为空：[{TypeName}]", this.GetType().FullName);
            }

            Do(db => db.HashSet(GetFormatKey(key), fieldVal.ToHashEntry()));
        }

        /// <summary>
        /// 获取键值下字段对应的值  [对应 HGET]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Get(string key, string field)
        {
            return Do(db => db.HashGet(GetFormatKey(key), field));
        }

        /// <summary>
        /// 获取键值下的所有字段与值的信息  [对应 HGETALL]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetFieldsValues(string key)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in Do(db => db.HashGetAll(GetFormatKey(key))))
            {
                if (!result.ContainsKey(item.Name))
                {
                    result.Add(item.Name, item.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取键值下的所有字段名  [对应 HKEYS]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFields(string key)
        {
            return Do(db => db.HashKeys(GetFormatKey(key)).ToStringArray());
        }

        /// <summary>
        /// 获取键值下所有值的信息  [对应 HVALS]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> GetValues(string key)
        {
            return Do(db => db.HashValues(GetFormatKey(key)).ToStringArray());
        }


        /// <summary>
        /// 获取键值下某个字段信息  [对应 HDEL]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool Del(string key, string field)
        {
            return Do(db => db.HashDelete(GetFormatKey(key), field));
        }

        /// <summary>
        /// 获取键值下某个字段是否存在 [对应 HEXISTS]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool Exists(string key, string field)
        {
            return Do(db => db.HashExists(GetFormatKey(key), field));
        }

        /// <summary>
        /// 获取键值下的字段数量 [对应 HLEN]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Length(string key)
        {
            return Do(db => db.HashLength(GetFormatKey(key)));
        }
    }
}
