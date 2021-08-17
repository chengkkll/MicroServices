using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 角色的更新信息
    /// </summary>
    public class RoleUpdateView
    {
        /// <summary>
        /// 新角色信息
        /// </summary>
        public RoleView NewRole { get; set; }
        /// <summary>
        /// 旧角色信息
        /// </summary>
        public RoleView OldRole { get; set; }

        /// <summary>
        /// 操作账号信息
        /// </summary>
        public StringTokenInfo LogonInfo { get; set; }
    }
}
