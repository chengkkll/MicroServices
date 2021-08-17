using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditState
    {
        /// <summary>
        /// 未定义
        /// </summary>
        None = 0,
        /// <summary>
        /// 待送审
        /// </summary>
        Edit = 1,
        /// <summary>
        /// 审核中
        /// </summary>
        Aduditing = 2,
        /// <summary>
        /// 审核通过
        /// </summary>
        Approved = 4,
        /// <summary>
        /// 已驳回
        /// </summary>
        Rejection = 8,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 16,
        /// <summary>
        /// 撤回
        /// </summary>
        Withdraw = 32,
    }
}
