namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 基础的员工信息
    /// </summary>
    public class BaseEmployeeView
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所在部门Id
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 所在部门名称
        /// </summary>
        public string DepartmentName { get; set; }
    }
}
