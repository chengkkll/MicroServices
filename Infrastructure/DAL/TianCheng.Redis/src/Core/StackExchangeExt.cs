using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Redis
{
    /// <summary>
    /// StackExchange.Redis 的扩展方法
    /// </summary>
    static public class StackExchangeExt
    {
        /// <summary>
        /// 字符串转成RedisValue格式的
        /// </summary>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        static public KeyValuePair<RedisValue, RedisValue> ToRedisPair(this KeyValuePair<string, string> strKeyValue)
        {
            return new KeyValuePair<RedisValue, RedisValue>(strKeyValue.Key, strKeyValue.Value);
        }

        /// <summary>
        /// 转成HashEntry格式
        /// </summary>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        static public HashEntry ToHashEntry(this KeyValuePair<string, string> strKeyValue)
        {
            return new HashEntry(strKeyValue.Key, strKeyValue.Value);
        }

        /// <summary>
        /// 转成HashEntry格式
        /// </summary>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        static public HashEntry[] ToHashEntry(this ICollection<KeyValuePair<string, string>> kvList)
        {
            HashEntry[] result = new HashEntry[kvList.Count];
            IEnumerator<KeyValuePair<string, string>> enumerator = kvList.GetEnumerator();

            for (int i = 0; i < kvList.Count; i++)
            {
                if (enumerator.MoveNext())
                {
                    result[i] = enumerator.Current.ToHashEntry();
                }
            }

            return result;
        }
    }
}
