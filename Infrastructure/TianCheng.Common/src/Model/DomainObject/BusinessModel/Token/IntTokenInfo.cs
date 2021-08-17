using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// int为Id的Token信息
    /// </summary>
    public class IntTokenInfo : TokenBase, IIntIdToken
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
