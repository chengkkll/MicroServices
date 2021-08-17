using TianCheng.Common;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// Consul配置信息接口
    /// </summary>
    public interface IConsulConfigurationService
    {
        /// <summary>
        /// 获取Consul配置信息
        /// </summary>
        ConsulOptions Options { get; }
    }
}
