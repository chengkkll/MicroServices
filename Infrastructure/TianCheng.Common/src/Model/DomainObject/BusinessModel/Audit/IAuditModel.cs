using System;

namespace TianCheng.Common
{
    /// <summary>
    /// 审核对象需要的属性接口
    /// </summary>
    public interface IAuditModel
    {
        /// <summary>
        /// 审批人Id列表
        /// </summary>
        /// <remarks></remarks>
        public string AuditIds { get; set; }

        /// <summary>
        /// 抄送人Id列表
        /// </summary>
        /// <remarks></remarks>
        public string CopyIds { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        /// <remarks>数据状态</remarks>
        public AuditState AuditState { get; set; }

        /// <summary>
        /// 是否催办过
        /// </summary>
        public bool IsUrge { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }
    }
}
