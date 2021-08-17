using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Inventory.Model
{
    /// <summary>
    /// 库存明细信息
    /// </summary>
    public class InventoryDetailInfo : MongoBusinessModel
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
        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 自动记录的操作信息
        /// </summary>
        public string KeepTrack { get; set; }
    }
}
