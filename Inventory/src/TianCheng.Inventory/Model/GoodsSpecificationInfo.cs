using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Inventory.Model
{

    /// <summary>
    /// 商品规格
    /// </summary>
    public class GoodsSpecificationInfo : MongoBusinessModel
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string Images { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        #region 品牌信息
        /// <summary>
        /// 品牌信息
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
        #endregion

        #region PV
        /// <summary>
        /// 是否使用PV值
        /// </summary>
        public bool HasPV { get; set; }

        /// <summary>
        /// PV值
        /// </summary>
        public double PV { get; set; }
        #endregion

        #region 价格
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 原始价格
        /// </summary>
        public decimal OrigPrice { get; set; }
        #endregion

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
    }
}
