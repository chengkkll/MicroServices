using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 禁用信息
    /// </summary>
    public interface IDisabledStringModel : IDisabledModel
    {
        #region 禁用
        /// <summary>
        /// 是否已禁用
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 禁用人Id
        /// </summary>
        public string DisablerId { get; set; }
        /// <summary>
        /// 禁用人名称
        /// </summary>
        public string DisablerName { get; set; }
        /// <summary>
        /// 禁用时间
        /// </summary>
        public DateTime? DisabledDate { get; set; }
        #endregion
    }
}
