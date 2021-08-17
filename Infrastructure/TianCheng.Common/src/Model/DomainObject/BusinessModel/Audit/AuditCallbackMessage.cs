namespace TianCheng.Common
{
    /// <summary>
    /// 审核回调对象
    /// </summary>
    public class AuditCallbackMessage
    {
        /// <summary>
        /// 业务数据Id
        /// </summary>
        public int DataId { get; set; }

        /// <summary>
        /// 业务编码
        /// </summary>
        public string BusinessCode { get; set; }

        /// <summary>
        /// 审核人id列表
        /// </summary>
        public string AuditIds { get; set; }

        /// <summary>
        /// 抄送人id列表
        /// </summary>
        public string CopyIds { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditState State { get; set; }

        /// <summary>
        /// 催办的状态
        /// </summary>
        public bool UpdateUrge { get; set; }
    }
}
