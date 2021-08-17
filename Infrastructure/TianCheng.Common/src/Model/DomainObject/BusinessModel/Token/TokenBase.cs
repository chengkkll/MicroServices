using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 基础的Token信息
    /// </summary>
    public class TokenBase
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 获取登录用户Id
        /// </summary>
        public dynamic GetEmployeeId
        {
            get
            {
                return this switch
                {
                    IIntIdToken intToken => intToken.EmployeeId,
                    IStringIdToken strToken => strToken.EmployeeId,
                    _ => string.Empty,
                };
            }
        }

        /// <summary>
        /// 获取登录用户Id 
        /// </summary>
        public string GetEmployeeStringId
        {
            get
            {
                return this switch
                {
                    IIntIdToken intToken => intToken.EmployeeId.ToString(),
                    IStringIdToken strToken => strToken.EmployeeId,
                    _ => string.Empty,
                };
            }
        }

        /// <summary>
        /// 当前用户拥有的功能点列表
        /// </summary>
        public List<string> FunctionPolicyList { get; set; } = new List<string>();

        /// <summary>
        /// 判断当前登录用户是否拥有指定的权限
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public bool HasPolicy(string policy)
        {
            return FunctionPolicyList.Contains(policy);
        }
    }
}
