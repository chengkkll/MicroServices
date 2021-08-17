using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 审核的通过方式
    /// </summary>
    public enum ApprovalType
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,
        /// <summary>
        /// 所有人全部通过才可以
        /// </summary>
        Every = 1,
        /// <summary>
        /// 任意一个均可  协同处理
        /// </summary>
        Any = 2,
    }
}
