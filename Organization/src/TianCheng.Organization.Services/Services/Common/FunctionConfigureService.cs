using Microsoft.Extensions.Options;
using TianCheng.Common;
using TianCheng.Organization.Model;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 功能点配置信息（用于读取配置信息）
    /// </summary>
    public class FunctionConfigureService : BaseConfigureService<FunctionModuleConfig>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="monitor"></param>
        public FunctionConfigureService(IOptionsMonitor<FunctionModuleConfig> monitor) : base(monitor)
        {

        }
    }
}
