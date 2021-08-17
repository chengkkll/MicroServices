using MediatR;
using System.Collections.Generic;
using System.Linq;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 部门的查询条件
    /// </summary>
    public class DepartmentQuery : QueryObject
    {
        /// <summary>
        /// 按名称模糊查询
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 按上级部门ID查询
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 只查询根部门
        /// </summary>
        public bool? OnlyRoot { get; set; }
        ///// <summary>
        ///// 按部门主管ID查询
        ///// </summary>
        //public string ManageId { get; set; }

        ///// <summary>
        ///// 按部门主管的名称模糊查询
        ///// </summary>
        //public string ManageName { get; set; }
    }
}
