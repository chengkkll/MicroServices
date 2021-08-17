using TianCheng.Common;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 商品查询
    /// </summary>
    public class GoodsQuery : QueryObject
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
