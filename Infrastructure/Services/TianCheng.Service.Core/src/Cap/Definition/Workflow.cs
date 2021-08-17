using System;
using System.Collections.Generic;
using System.Text;

namespace Cap
{
    /// <summary>
    /// 工作流   [消息队列]
    /// </summary>
    public struct Workflow
    {
        /// <summary>
        /// 开始审批流程
        /// </summary>
        public const string Start = "Cap.Workflow.Start";
        /// <summary>
        /// 审核处理
        /// </summary>
        public const string Audit = "Cap.Workflow.Audit";
        /// <summary>
        /// 审核的回调方法
        /// </summary>
        public const string SetBusinessData = "Cap.Workflow.SetBusinessData.";
    }
}
