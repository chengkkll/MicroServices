using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 带部门信息的登录
    /// </summary>
    public class DepartmentLoginView : LoginView
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }
    }
}
