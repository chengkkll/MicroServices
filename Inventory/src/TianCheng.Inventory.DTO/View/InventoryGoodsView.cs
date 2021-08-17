using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 商品库存信息
    /// </summary>
    public class InventoryGoodsView
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
