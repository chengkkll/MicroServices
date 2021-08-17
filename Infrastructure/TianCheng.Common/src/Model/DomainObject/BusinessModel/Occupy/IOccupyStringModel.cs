using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 数据占用信息
    /// </summary>
    public interface IOccupyStringModel : IOccupyModel
    {
        #region 占用信息
        /// <summary>
        /// 占用人Id
        /// </summary>
        public string OccupierId { get; set; }
        /// <summary>
        /// 占用人名称
        /// </summary>
        public string OccupierName { get; set; }
        /// <summary>
        /// 占用时间
        /// </summary>
        public DateTime? OccupyDate { get; set; }
        #endregion
    }
}
