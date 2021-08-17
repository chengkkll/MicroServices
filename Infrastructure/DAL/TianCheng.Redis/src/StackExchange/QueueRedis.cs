//using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Redis
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
        public long Push(string key, string val)
        {
            return Do(db => db.ListRightPush(GetFormatKey(key), val));
        }

        /// <summary>
        /// 取出数据        [对应 LPOP]
        /// </summary>
        /// <param name="key">键</param>
        public string Pop(string key)
        {
            return Do(db => db.ListLeftPop(GetFormatKey(key)));
        }
    }
}
