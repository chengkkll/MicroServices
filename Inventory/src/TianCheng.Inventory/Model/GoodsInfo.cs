using System;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Inventory.Model
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class GoodsInfo : MongoIdModel, ICreateStringModel, IUpdateStringModel, IDeleteStringModel
    {
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

        #region 新增信息
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreaterName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        #endregion 

        #region 修改信息
        /// <summary>
        /// 修改人Id
        /// </summary>
        public string UpdaterId { get; set; }
        /// <summary>
        /// 更新人名称
        /// </summary>
        public string UpdaterName { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        #endregion 

        #region 逻辑删除
        /// <summary>
        /// 是否已逻辑删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        public string DeleteId { get; set; }
        /// <summary>
        /// 删除人名称
        /// </summary>
        public string DeleteName { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteDate { get; set; }
        #endregion
    }
}
