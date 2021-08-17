using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 分类树信息
    /// </summary>
    public class CategoryTreeView
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子类树
        /// </summary>
        public List<CategorySubTreeView> SubCategory { get; set; } = new List<CategorySubTreeView>();
    }
}
