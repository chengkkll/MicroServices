using Newtonsoft.Json;

namespace TianCheng.Common
{
    /// <summary>
    /// 查询条件基类
    /// </summary>
    public class QueryObject
    {
        /// <summary>
        /// 排序规则
        /// </summary>
        [JsonProperty("sort")]
        public QuerySort Sort { get; set; } = new QuerySort() { Property = "date", IsAsc = false };

        /// <summary>
        /// 分页信息
        /// </summary>
        [JsonProperty("page")]
        public QueryPagination Page { get; set; } = new QueryPagination();

        /// <summary>
        /// 登录信息
        /// </summary>
        [JsonIgnore]
        public TokenBase Token { get; set; } = new TokenBase();
    }
}
