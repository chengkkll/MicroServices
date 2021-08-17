using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 更新信息接口
    /// </summary>
    public interface IUpdateIntModel
    {
        #region 更新信息
        /// <summary>
        /// 更新人Id
        /// </summary>
        public int UpdaterId { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdaterName { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        #endregion
    }
}
