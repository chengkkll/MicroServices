using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 简单的商品信息
    /// </summary>
    public class GoodsSimpleView
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

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
        public List<string> Specification1 { get; set; } = new List<string>();
        /// <summary>
        /// 规格2
        /// </summary>
        public List<string> Specification2 { get; set; } = new List<string>();
        /// <summary>
        /// 规格3
        /// </summary>
        public List<string> Specification3 { get; set; } = new List<string>();
        #endregion
    }
}
