using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// Swagger配置信息扩展方法
    /// </summary>
    static public class SwaggerDocInfoExt
    {
        /// <summary>
        /// 获取当前运行项目对应的Swagger配置
        /// </summary>
        /// <param name="swaggerConfigDict"></param>
        /// <returns></returns>
        static public SwaggerDocInfo Current(this Dictionary<string, SwaggerDocInfo> swaggerConfigDict)
        {
            return swaggerConfigDict.Values.ToList().Current();
        }

        /// <summary>
        /// 获取当前运行项目对应的Swagger配置
        /// </summary>
        /// <param name="swaggerConfigList"></param>
        /// <returns></returns>
        static public SwaggerDocInfo Current(this List<SwaggerDocInfo> swaggerConfigList)
        {
            if (swaggerConfigList.Count == 0)
            {
                return SwaggerDocInfo.Default;
            }
            if (swaggerConfigList.Count == 1)
            {
                return swaggerConfigList.FirstOrDefault();
            }

            string currentNs = Assembly.GetEntryAssembly().GetName().Name;
            foreach (var conf in swaggerConfigList)
            {
                if (conf.Namespace.StartsWith(currentNs))
                {
                    return conf;
                }
            }
            foreach (var conf in swaggerConfigList)
            {
                if (conf.Namespace.Contains(currentNs))
                {
                    return conf;
                }
            }
            return swaggerConfigList.FirstOrDefault();
        }

        /// <summary>
        /// 将列表按Code转成一个字典对象
        /// </summary>
        /// <param name="swaggerConfigList"></param>
        /// <returns></returns>
        static public Dictionary<string, SwaggerDocInfo> ToDict(this List<SwaggerDocInfo> swaggerConfigList)
        {
            if (swaggerConfigList == null || swaggerConfigList.Count == 0)
            {
                //throw new System.Exception("请配置swagger信息");
                //DefaultControllerLog.Logger.Error("请配置swagger信息");
                //DefaultControllerLog.Logger.Information(@"swagger默认节点名称为swagger,且为数组形式。\r\n示例：" +
                //                                        "\"swagger\": [" +
                //                                        "  {" +
                //                                        "      \"Code\": \"WebAppDemo1\"" +
                //                                        "      \"Namespace\": \"WebAppDemo1\"," +
                //                                        "      \"Version\": \"v1\"," +
                //                                        "      \"Title\": \"测试系统接口文档\"," +
                //                                        "      \"Description\": \"RESTful API for WebAppDemo1\"," +
                //                                        "      \"TermsOfService\": \"\"," +
                //                                        "      \"ContactName\": \"cheng_kkll\"," +
                //                                        "      \"ContactEmail\": \"cheng_kkll@163.com\"," +
                //                                        "      \"ContactUrl\": \"\"," +
                //                                        "      \"LicenseName\": \"\"," +
                //                                        "      \"LicenseUrl\": \"\"," +
                //                                        "      \"DocumentTitle\": \"WebAppDemo1 接口说明\"," +
                //                                        "      \"EndpointUrl\": \"https://localhost:5001/swagger/WebAppDemo1/swagger.json\"," +
                //                                        "      \"EndpointDesc\": \"WebAppDemo1 API V1\"," +
                //                                        "      \"OrderBy\": []" +
                //                                        "   }" +
                //                                        "]");
                return new Dictionary<string, SwaggerDocInfo>();
            }
            // 初始化字典
            Dictionary<string, SwaggerDocInfo> configDict = new Dictionary<string, SwaggerDocInfo>();
            // 配置信息字典
            for (int index = 0; index < swaggerConfigList.Count; index++)
            {
                var doc = swaggerConfigList.ElementAt(index);
                if (string.IsNullOrWhiteSpace(doc.Code))
                {
                    doc.Code = index == 0 ? "default" : $"code{index + 1}";
                }
                if (!configDict.ContainsKey(doc.Code))
                {
                    configDict.Add(doc.Code, doc);
                }
            }
            return configDict;
        }
    }
}
