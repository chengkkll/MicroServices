using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// Id类型为string的业务基类
    /// </summary>
    public class StringBusinessModel : StringIdModel
    {
        #region 新增信息
        /// <summary>
        /// 创建人ID
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
        /// 更新人ID
        /// </summary>
        public string UpdaterId { get; set; }
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
        /// 业务流程状态
        /// </summary>
        public ProcessState ProcessState { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? ReleaseDate { get; set; }
        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsDelete { get; set; }
        #endregion
    }
}
