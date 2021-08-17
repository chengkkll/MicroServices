using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Organization.MqBus
{
    /// <summary>
    /// 部门消息队列
    /// </summary>
    public struct DepartmentMq
    {
        /// <summary>
        /// 部门数据更新
        /// </summary>
        public const string Update = "Org.Department.Update";
    }

    /// <summary>
    /// 菜单消息队列
    /// </summary>
    public struct MenuMq
    {
        /// <summary>
        /// 菜单数据更新
        /// </summary>
        public const string Update = "Org.Menu.Update";
    }

    /// <summary>
    /// 员工消息队列
    /// </summary>
    public struct EmployeeMq
    {
        /// <summary>
        /// 员工数据更新
        /// </summary>
        public const string Update = "Org.Employee.Update";
    }


    /// <summary>
    /// 角色消息队列
    /// </summary>
    public struct RoleMq
    {
        /// <summary>
        /// 角色数据更新
        /// </summary>
        public const string Update = "Org.Role.Update";
    }
}
