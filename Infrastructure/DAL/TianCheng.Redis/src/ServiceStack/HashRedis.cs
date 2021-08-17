using Microsoft.Extensions.Logging;
//using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Redis.ServiceStack
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
        public void Set(string key, string field, string val)
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

            // 创建操作对象
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            //设置值
            client.Hashes[GetFormatKey(key)].Add(field, val);
        }

        /// <summary>
        /// 获取键值下的所有字段与值的信息  [对应 HGETALL]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetAll(string key)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.Hashes[GetFormatKey(key)];
        }

        /// <summary>
        /// 获取键值下的所有字段名  [对应 HKEYS]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFields(string key)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.Hashes[GetFormatKey(key)].Keys;
        }

        /// <summary>
        /// 获取键值下所有值的信息  [对应 HVALS]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> GetValues(string key)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.Hashes[GetFormatKey(key)].Values;
        }

        /// <summary>
        /// 获取键值下字段对应的值  [对应 HGET]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Get(string key, string field)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.Hashes[GetFormatKey(key)][field];
        }

        /// <summary>
        /// 获取键值下某个字段信息  [对应 HDEL]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool Del(string key, string field)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.Hashes[GetFormatKey(key)].Remove(field);
        }

        /// <summary>
        /// 获取键值下某个字段是否存在 [对应 HEXISTS]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool Exists(string key, string field)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.HExists(GetFormatKey(key), GetByteArray(field)) == 1;
            // 还有两种方式获取，有时间测测性能做做比较
            //return client.HashContainsEntry(GetHashKey(key), field);
            //return client.Hashes[GetHashKey(key)].ContainsKey(field);
        }

        /// <summary>
        /// 获取键值下的字段数量 [对应 HLEN]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Length(string key)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;

            return client.HLen(GetFormatKey(key));
        }
    }
}
