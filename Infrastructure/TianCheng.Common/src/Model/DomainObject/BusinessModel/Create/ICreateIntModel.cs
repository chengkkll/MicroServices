using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 创建信息接口
    /// </summary>
    public interface ICreateIntModel
    {
        #region 新增信息
        /// <summary>
        /// 创建人Id
        /// </summary>
        public int CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreaterName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        #endregion
    }
}
