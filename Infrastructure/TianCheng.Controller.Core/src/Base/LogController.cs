using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TianCheng.Common;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 包含日志的Controller
    /// </summary>
    [ApiController]
    public class LogController : ControllerBase
    {
        #region 构造方法
        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;
        /// <summary>
        /// 构造方法
        /// </summary>
        public LogController()
        {
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(GetType());
        }
        #endregion
    }
}
