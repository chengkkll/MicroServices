namespace TianCheng.Common
{
    /// <summary>
    /// 审核结果  用于终端与服务端等等审核信息传递
    /// </summary>
    public class AuditResult
    {
        /// <summary>
        /// 审核的业务数据id
        /// </summary>
        public int DataId { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool Approval { get; set; }
    }
}
