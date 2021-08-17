using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIntBusinessModel
    {
        #region 新增信息
        /// <summary>
        /// 创建人ID
        /// </summary>
        int CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        string CreaterName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateDate { get; set; }
        #endregion

        #region 修改信息
        /// <summary>
        /// 更新人ID
        /// </summary>
        int UpdaterId { get; set; }
        /// <summary>
        /// 更新人名称
        /// </summary>
        string UpdaterName { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateDate { get; set; }
        #endregion

        #region 逻辑删除
        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        bool IsDelete { get; set; }
        #endregion
    }
}
