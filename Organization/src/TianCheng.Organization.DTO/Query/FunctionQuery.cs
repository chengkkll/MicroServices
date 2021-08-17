using Newtonsoft.Json;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 功能点的查询条件
    /// </summary>
    public class FunctionQuery : QueryObject
    {
        /// <summary>
        /// 按名称模糊查询
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
