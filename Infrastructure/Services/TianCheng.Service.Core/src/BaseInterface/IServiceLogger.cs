using Microsoft.Extensions.Logging;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 包含日志的服务
    /// </summary>

    public interface IServiceLogger
    {
        public ILogger Log { get; }
    }
}
