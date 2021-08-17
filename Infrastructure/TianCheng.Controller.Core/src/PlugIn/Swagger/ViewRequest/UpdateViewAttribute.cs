using System;
using System.Collections.Generic;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 用于标识请求体是否为修改的查看对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public class UpdateViewAttribute : Attribute
    {
        private static readonly Dictionary<string, string> UpdateSchemaDict = new Dictionary<string, string>();

        /// <summary>
        /// 增加引用id
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        static public void Append(string key, string id)
        {
            if (!UpdateSchemaDict.ContainsKey(key))
            {
                UpdateSchemaDict.Add(key, id);
            }
        }

        /// <summary>
        /// 获取所有需要设置的查看对象
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, string> Load()
        {
            return UpdateSchemaDict;
        }
    }
}
