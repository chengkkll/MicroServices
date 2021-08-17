using TianCheng.Common;

namespace TianCheng.Inventory.Model
{
    /// <summary>
    /// 分类的商品列表
    /// </summary>
    public class CategoryGoodsInfo : MongoBusinessModel
    {
        /// <summary>
        /// 主分类Id
        /// </summary>
        public string MainId { get; set; }
        /// <summary>
        /// 主分类名称
        /// </summary>
        public string MainName { get; set; }

        /// <summary>
        /// 子分类Id
        /// </summary>
        public string SubId { get; set; }
        /// <summary>
        /// 子类名称
        /// </summary>
        public string SubName { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
    }
}
