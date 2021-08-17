using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 按商品规格的库存
    /// </summary>
    public class InventorySpecificationView
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        #region 规格
        /// <summary>
        /// 规格1名称
        /// </summary>
        public string Specification1Name { get; set; }
        /// <summary>
        /// 规格1名称
        /// </summary>
        public string Specification2Name { get; set; }
        /// <summary>
        /// 规格1名称
        /// </summary>
        public string Specification3Name { get; set; }
        /// <summary>
        /// 规格1
        /// </summary>
        public string Specification1Value { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        public string Specification2Value { get; set; }
        /// <summary>
        /// 规格3
        /// </summary>
        public string Specification3Value { get; set; }
        #endregion

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
