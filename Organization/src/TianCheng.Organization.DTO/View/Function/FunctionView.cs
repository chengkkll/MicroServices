using Newtonsoft.Json;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 功能点对象 查看对象
    /// </summary>
    public class FunctionView
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }
        /// <summary>
        /// 功能点标识名      可以理解为唯一标识功能点的ID值
        /// </summary>
        [JsonProperty("policy")]
        public string Policy { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
