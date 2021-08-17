using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 业务信息的查看对象
    /// </summary>
    public class BusinessView : IdView
    {
        #region 新增信息
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
        /// 更新人名称
        /// </summary>
        public string UpdaterName { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        #endregion

        #region 业务流程信息
        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsDelete { get; set; }
        #endregion
    }
}
