using TianCheng.Common;

namespace TianCheng.Inventory.Model
{
    /// <summary>
    /// 商品库存信息
    /// </summary>
    public class InventoryGoodsInfo : MongoBusinessModel
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
