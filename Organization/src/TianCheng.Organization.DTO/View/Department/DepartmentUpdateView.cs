using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 部门的更新信息
    /// </summary>
    public class DepartmentUpdateView
    {
        /// <summary>
        /// 新部门信息
        /// </summary>
        public DepartmentView NewDepartment { get; set; }
        /// <summary>
        /// 旧部门信息
        /// </summary>
        public DepartmentView OldDepartment { get; set; }

        /// <summary>
        /// 操作账号信息
        /// </summary>
        public StringTokenInfo LogonInfo { get; set; }
    }
}
