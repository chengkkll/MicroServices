using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 字符串为Id的Token信息
    /// </summary>
    public class StringTokenInfo : TokenBase, IStringIdToken
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
