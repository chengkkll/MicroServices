using Newtonsoft.Json;

namespace TianCheng.Common
{
    /// <summary>
    /// 分页对象
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PagedResultPagination
    {
        /// <summary>
        /// 当前页号
        /// </summary>
        [JsonProperty("index")]
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页最多显示的数据条数
        /// </summary>
        [JsonProperty("max")]
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总条数
        /// </summary>
        [JsonProperty("records")]
        public int TotalRecords { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        [JsonProperty("total")]
        public int TotalPage { get; set; }
    }
}
