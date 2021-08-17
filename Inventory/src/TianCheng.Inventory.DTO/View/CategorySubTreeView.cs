using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 子类树信息
    /// </summary>
    public class CategorySubTreeView
    {
        /// <summary>
        /// 子类Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 子类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品列表
        /// </summary>
        public List<SelectView> GoodsSelect { get; set; } = new List<SelectView>();
        /// <summary>
        /// 商品规格列表
        /// </summary>
        public List<GoodsSpecificationView> GoodsSpecification { get; set; } = new List<GoodsSpecificationView>();
    }
}
