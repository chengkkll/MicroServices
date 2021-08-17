using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 部门下的员工信息
    /// </summary>
    public class DepartmentEmployeeView
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 部门员工列表
        /// </summary>
        public List<NameViewModel> Employees { get; set; } = new List<NameViewModel>();
    }
}
