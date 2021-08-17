using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 工作流操作类型
    /// </summary>
    public enum WorkflowOperationType
    {
        /// <summary>
        /// 开始
        /// </summary>
        Start = 1,
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
