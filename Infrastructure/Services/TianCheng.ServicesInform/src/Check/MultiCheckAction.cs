using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.ConsulHelper;

namespace TianCheng.ServicesInform
{

    /// <summary>
    /// 多个检测的判断
    /// </summary>
    public class MultiCheckAction : IMultiCheckAction
    {
        /// <summary>
        /// 根据命令获取所有需要扩展的服务信息
        /// </summary>
        /// <param name="postCommandName"></param>
        /// <returns></returns>
        private List<MultiCheckAttribute> GetExecuteService(string postCommandName)
        {
            // 根据postCommandName读取Consul，并获取需要操作的微服务及方法名称
            return GetMultiCheckValue(postCommandName) ?? new List<MultiCheckAttribute>();
        }
        /// <summary>
        /// 根据命令获取所有需要扩展的服务信息
        /// </summary>
        /// <param name="postCommandName"></param>
        /// <returns></returns>
        private List<MultiCheckAttribute> GetMultiCheckValue(string postCommandName)
        {
            return ConsulConfigurationHelper.GetObject<List<MultiCheckAttribute>>(MultiCheckAttribute.GetKey(postCommandName));
        }

        /// <summary>
        /// 调用所有注册的服务
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="postObject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool MultiCheck(string commandName, object postObject, out string message)
        {
            message = string.Empty;
            var service = ServiceLoader.GetService<IRestfullApiCall>();

            foreach (var loader in GetExecuteService(commandName))
            {
                try
                {
                    var result = service.Call(loader.HttpMethod, loader.CurrentProject, $"{loader.RouteController}/{loader.RouteMethod}", postObject: postObject);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        if (result.StartsWith("{") && result.EndsWith("}"))
                        {
                            var resObj = result.JsonToObject<CheckResult>();

                            if (resObj != null && resObj.Result == false)
                            {
                                message = resObj.Message;
                                return false;
                            }
                        }
                        else if (result.IndexOf("\r\n") > 0)
                        {
                            var info = result.Substring(0, result.IndexOf("\r\n")).Split(":");
                            if (info != null && info.Length == 2)
                            {
                                message = info[1];
                                return false;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return true;
        }
    }
}
