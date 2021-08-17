using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 品牌信息
    /// </summary>
    public class BrandView
    {
        /// <summary>
        /// 品牌Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 品牌Logo
        /// </summary>
        public string Logo { get; set; }
    }
}
