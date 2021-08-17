using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// Consul 的配置信息读取
    /// </summary>
    static public class ConsulConfigurationHelper
    {
        static public Uri ConnUri { get; set; }
        /// <summary>
        /// 根据 key 读取节点值
        /// </summary> 
        /// <param name="key"></param>
        /// <returns></returns>
        static public T GetObject<T>(string key) where T : new()
        {
            return GetValue(key).JsonToObject<T>();
        }

        /// <summary>
        /// 根据 key 读取节点的字符串值
        /// </summary> 
        /// <param name="key"></param>
        /// <returns></returns>
        static public string GetValue(string key)
        {
            using ConsulClient client = new ConsulClient();
            var val = client.KV.Get(key).Result;
            if (val.Response != null)
            {
                return Encoding.UTF8.GetString(val.Response.Value);
            }
            return string.Empty;
        }

        static public void Set(string key, string val, bool overwrite = true)
        {
            KVPair pair = new KVPair(key)
            {
                Value = Encoding.UTF8.GetBytes(val)
            };
            using ConsulClient client = new ConsulClient(consulConfig =>
            {
                if (ConnUri != null)
                {
                    consulConfig.Address = ConnUri;
                }
            });

            if (!overwrite)
            {
                var result = client.KV.Get(key).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    return;
                }
            }

            _ = client.KV.Put(pair).Result;
        }
    }
}
