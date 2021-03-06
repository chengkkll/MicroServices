using Newtonsoft.Json;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 登录后的用户权限信息
    /// </summary>
    public class LogonPowerView : NameViewModel
    {
        /// <summary>
        /// 角色信息
        /// </summary>
        [JsonProperty("role")]
        public RoleSimpleView Role { get; set; }
        /// <summary>
        /// 部门信息
        /// </summary>
        [JsonProperty("department")]
        public NameViewModel Department { get; set; }
        /// <summary>
        /// 菜单信息
        /// </summary>
        [JsonProperty("menu")]
        public List<MenuMainView> Menu { get; set; }
        /// <summary>
        /// 包含的功能点列表
        /// </summary>
        [JsonProperty("functions")]
        public List<FunctionView> Functions { get; set; }
    }
}
