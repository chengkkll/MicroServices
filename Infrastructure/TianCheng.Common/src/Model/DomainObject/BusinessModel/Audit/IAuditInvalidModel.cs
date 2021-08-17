using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 审核作废需要的属性接口
    /// </summary>
    public interface IAuditInvalidModel : IAuditModel
    {
        /// <summary>
        /// 作废状态
        /// </summary>
        public AuditState AuditInvalidState { get; set; }
        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime? InvalidDate { get; set; }
        /// <summary>
        /// 作废原因
        /// </summary>
        public string InvalidReason { get; set; }
    }
}
