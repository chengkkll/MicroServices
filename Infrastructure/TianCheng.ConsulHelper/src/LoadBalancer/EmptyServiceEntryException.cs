using System;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// 负载均衡时，没有找到可用的服务器
    /// </summary>
    public class EmptyServiceEntryException : AggregateException
    {
        /// <summary>
        /// 异常信息
        /// </summary>
        static private readonly string EmptyMessage = "负载均衡时，没有找到可用的服务器";
        /// <summary>
        /// 构造方法
        /// </summary>
        public EmptyServiceEntryException() : base(EmptyMessage)
        {
            Console.WriteLine(EmptyMessage);
        }
    }
}
