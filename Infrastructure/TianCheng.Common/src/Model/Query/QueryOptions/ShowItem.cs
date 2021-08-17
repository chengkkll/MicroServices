using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace TianCheng.Common
{
    /// <summary>
    /// 显示项信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    [JsonObject(MemberSerialization.OptIn)]
    public class ShowItem<T, Q>
        where Q : QueryObject, new()
    {
        /// <summary>
        /// 查询条件显示项
        /// </summary>
        [JsonProperty("query")]
        public Dictionary<string, bool> Query { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// 查询结果显示项
        /// </summary>
        [JsonProperty("result")]
        public Dictionary<string, bool> Result { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// 获取默认的显示项
        /// </summary>
        /// <returns></returns>
        public ShowItem<T, Q> Default()
        {
            ShowItem<T, Q> show = new ShowItem<T, Q>();
            // 看所有属性
            foreach (var p in typeof(T).GetProperties())
            {
                if (!show.Result.ContainsKey(p.Name))
                {
                    show.Result.Add(p.Name, true);
                }
            }
            // 不看基类属性
            foreach (var p in typeof(Q).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
            {
                if (!show.Query.ContainsKey(p.Name))
                {
                    show.Query.Add(p.Name, true);
                }
            }
            return show;
        }
    }
}
