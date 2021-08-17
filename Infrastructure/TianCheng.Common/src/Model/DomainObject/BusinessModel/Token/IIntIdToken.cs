using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// id类型为int的登录信息
    /// </summary>
    public interface IIntIdToken
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentId { get; set; }
    }
}
