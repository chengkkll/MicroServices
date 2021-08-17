using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.ServicesInform
{
    /// <summary>
    /// 服务通知的实现类
    /// </summary>
    public class ServicesInform
    {
        /// <summary>
        /// 根据命令调用所有需要扩展的服务信息
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="postObject"></param>
        /// <param name="message">返回第一出现问题的错误</param>
        /// <returns></returns>
        static public bool MultiCheck(string commandName, object postObject, out string message)
        {
            var service = ServiceLoader.GetService<IMultiCheckAction>();
            return service.MultiCheck(commandName, postObject, out message);
        }
    }
}
