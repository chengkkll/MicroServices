using Newtonsoft.Json;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 功能点模块
    /// </summary>
    public class FunctionModuleView : NameViewModel
    {
        /// <summary>
        /// 模块序号
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }
        /// <summary>
        /// 模块编码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 功能点分组（Control）
        /// </summary>
        [JsonProperty("group")]
        public List<FunctionGroupView> FunctionGroups { get; set; } = new List<FunctionGroupView>();
    }
}
