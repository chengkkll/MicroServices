using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// 分析AuthAction特性
    /// </summary>
    static public class AuthActionAnalyze
    {
        /// <summary>
        /// 本地缓存的解析结果
        /// </summary>
        private static readonly IDictionary<string, string> AnalyzeResult = new Dictionary<string, string>();

        /// <summary>
        /// 解析
        /// </summary>
        static public void Analyze()
        {
            // 清空本地缓存的数据
            AnalyzeResult.Clear();
            // 遍历所有可用程序集下的所有类型中的方法，将所有具有AuthAction特性的方法都加入本地缓存字典。
            foreach (var assembly in AssemblyHelper.GetAssemblyList())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods())
                    {
                        var attr = method.GetCustomAttribute<AuthActionAttribute>();
                        if (attr != null)
                        {
                            //Console.WriteLine($"assembly:{assembly.FullName}   type:{type.Name}    method:{method.Name}  code:{attr.ActionCode}");
                            string key = GetKey(type.Name, method.Name);
                            string val = attr.ActionCode;
                            if (!AnalyzeResult.ContainsKey(key))
                            {
                                AnalyzeResult.Add(key, val);
                            }
                            // todo ： 路由定位时不清楚如何有效获取重载形式的方法，所以先不对参数做操作，暂时需要不同的ActionCode对应不同的方法名称
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取缓存的key
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        static private string GetKey(string controller, string action)
        {
            return $"{controller.ToLower().Replace("controller", "")}/{action.ToLower()}";
        }

        /// <summary>
        /// 根据路由地址获取权限编码
        /// </summary>
        /// <param name="routing"></param>
        /// <returns></returns>
        static public string GetPowerCode(RouteEndpoint routing)
        {
            if (routing == null)
            {
                return string.Empty;
            }
            string controller = string.Empty;
            string action = string.Empty;
            if (routing.RoutePattern.Defaults.TryGetValue("controller", out object contr))
            {
                controller = contr.ToString();
            }
            if (routing.RoutePattern.Defaults.TryGetValue("action", out object acti))
            {
                action = acti.ToString();
            }

            string key = GetKey(controller, action);
            if (AnalyzeResult.ContainsKey(key))
            {
                return AnalyzeResult[key];
            }
            return string.Empty;
        }
    }

}
