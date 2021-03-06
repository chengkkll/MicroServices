namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 权限功能点信息
    /// </summary>
    public class FunctionInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 功能点标识名
        /// </summary>
        public string Policy { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属模块编号
        /// </summary>
        public string ModeuleCode { get; set; }
        /// <summary>
        /// 所属功能组编号
        /// </summary>
        public string GroupCode { get; set; }
    }
}
