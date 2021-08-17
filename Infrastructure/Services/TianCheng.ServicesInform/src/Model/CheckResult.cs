using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.ServicesInform
{
    /// <summary>
    /// 检查的返回结果
    /// </summary>
    public class CheckResult
    {
        /// <summary>
        /// 检查结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 错误时的消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 一个检查成功的结果
        /// </summary>
        /// <returns></returns>
        static public CheckResult OK()
        {
            return new CheckResult() { Result = true, Message = string.Empty };
        }
        /// <summary>
        /// 获取一个检查失败的结果，并设置失败信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        static public CheckResult Error(string message)
        {
            return new CheckResult() { Result = false, Message = message };
        }
    }
}
