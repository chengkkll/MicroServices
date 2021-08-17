using Newtonsoft.Json;

namespace TianCheng.Common
{
    /// <summary>
    /// 用于给下拉列表显示数据的对象结构
    /// </summary>
    public class SelectView
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
