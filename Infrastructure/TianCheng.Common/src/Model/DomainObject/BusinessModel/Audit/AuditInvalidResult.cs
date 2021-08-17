namespace TianCheng.Common
{
    /// <summary>
    /// 审核作废信息
    /// </summary>
    public class AuditInvalidResult : AuditResult
    {
        /// <summary>
        /// 作废原因
        /// </summary>
        public string InvalidReason { get; set; }
    }
}
