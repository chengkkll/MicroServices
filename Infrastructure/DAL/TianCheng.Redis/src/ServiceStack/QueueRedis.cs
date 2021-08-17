//using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Redis.ServiceStack
{
    /// <summary>
    /// 队列的缓存处理
    /// </summary>
    public class QueueRedis : BaseRedis
    {
        /// <summary>
        /// 增加数据        [对应 RPUSH]
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public void Push(string key, string val)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;
            // 入列， 在队列末尾增加
            client.EnqueueItemOnList(GetFormatKey(key), val);
        }

        /// <summary>
        /// 取出数据        [对应 LPOP]
        /// </summary>
        /// <param name="key">键</param>
        public string Pop(string key)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            client.Db = DBIndex;
            // 出列， 在队列前弹出一个数据
            return client.DequeueItemFromList(GetFormatKey(key));
        }
    }
}
