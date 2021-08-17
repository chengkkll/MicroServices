using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Inventory.Model
{
    /// <summary>
    /// 子类信息
    /// </summary>
    public class CategorySubInfo : MongoBusinessModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 主分类Id
        /// </summary>
        public string MainId { get; set; }
        /// <summary>
        /// 主分类名称
        /// </summary>
        public string MainName { get; set; }
    }
}
