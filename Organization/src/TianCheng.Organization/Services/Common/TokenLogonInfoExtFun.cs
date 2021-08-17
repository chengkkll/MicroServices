using System;
using TianCheng.Common;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 角色操作的扩展方法
    /// </summary>
    static public class TokenLogonInfoExtFun
    {
        /// <summary>
        /// 判断当前用户是否为管理员
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        static public bool IsAdminRole(this TokenBase logonInfo)
        {
            RoleService roleService = ServiceLoader.GetService<RoleService>();
            if (roleService == null)
            {
                return false;
            }
            var roleInfo = roleService.SingleById(logonInfo.RoleId);
            if (roleInfo != null && (roleInfo.Name.Contains("管理员") || roleInfo.IsAdmin))
            {
                return true;
            }
            return false;
        }
    }
}
