using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 模块的配置信息，用于读取写在appsettings.json中的信息
    /// </summary>
    public class FunctionModuleConfig
    {
        /// <summary>
        /// 获取模块的命名空间及模块名称
        /// </summary>
        public List<SelectView> ModuleDict { get; set; } = new List<SelectView>();
    }
}
