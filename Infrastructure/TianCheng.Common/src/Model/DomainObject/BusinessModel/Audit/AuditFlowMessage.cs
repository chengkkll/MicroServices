namespace TianCheng.Common
{
    /// <summary>
    /// 审核时发送给消息总线的消息
    /// </summary>
    public class AuditFlowMessage
    {
        /// <summary>
        /// 当前登录用户Id
        /// </summary>
        public int LoginId { get; set; }
        /// <summary>
        /// 当前登录用户名称
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 当前登录用户的部门Id
        /// </summary>
        public string LoginDepId { get; set; }
        /// <summary>
        /// 业务编码
        /// </summary>
        public string BusinessCode { get; set; }
        /// <summary>
        /// 业务数据Id
        /// </summary>
        public int DataId { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public WorkflowOperationType Operation { get; set; }
        /// <summary>
        /// 审核时的参数信息
        /// </summary>
        public string Parameter { get; set; }
    }
}
