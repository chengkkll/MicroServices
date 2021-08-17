using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 按名称查询
    /// </summary>
    public class QueryName : QueryObject
    {
        /// <summary>
        /// 按名称模糊查询
        /// </summary>
        public string LikeName { get; set; }
        /// <summary>
        /// 按名称完全匹配查询
        /// </summary>
        public string EqualsName { get; set; }
    }
}
