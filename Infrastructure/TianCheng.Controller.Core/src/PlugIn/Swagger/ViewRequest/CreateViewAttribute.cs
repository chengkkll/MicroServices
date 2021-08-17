using System;
using System.Collections.Generic;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 用于标识请求体是否为创建的查看对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public class CreateViewAttribute : Attribute
    {
        private static readonly Dictionary<string, string> CreateSchemaDict = new Dictionary<string, string>();

        /// <summary>
        /// 增加引用id
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        static public void Append(string key, string id)
        {
            if (!CreateSchemaDict.ContainsKey(key))
            {
                CreateSchemaDict.Add(key, id);
            }
        }

        /// <summary>
        /// 获取所有需要设置的查看对象
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, string> Load()
        {
            return CreateSchemaDict;
        }
    }
}
