using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// id类型为string的登录信息
    /// </summary>
    public interface IStringIdToken
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }
    }
}
