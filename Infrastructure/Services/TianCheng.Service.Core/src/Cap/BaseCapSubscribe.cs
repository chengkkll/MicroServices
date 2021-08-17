using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 消息处理的基类
    /// </summary>
    public class BaseCapSubscribe : ICapSubscribe, IServiceRegister
    {
        #region 构造方法
        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseCapSubscribe()
        {
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(GetType());
        }
        #endregion
    }
}
