using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.ServicesInform
{
    /// <summary>
    /// 多个检测的判断接口
    /// </summary>
    public interface IMultiCheckAction
    {
        /// <summary>
        /// 调用所有注册的服务
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="postObject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool MultiCheck(string commandName, object postObject, out string message);
    }
}
