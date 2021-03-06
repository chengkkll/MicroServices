using Newtonsoft.Json;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 角色查看信息
    /// </summary>
    public class RoleView : NameViewModel
    {
        /// <summary>
        /// 描述信息
        /// </summary>
        [JsonProperty("desc")]
        public string Desc { get; set; }
        /// <summary>
        /// 当前角色登录后的默认页面
        /// </summary>
        [JsonProperty("page")]
        public string DefaultPage { get; set; }

        /// <summary>
        /// 包含菜单列表
        /// </summary>
        [JsonProperty("menu")]
        public List<MenuMainView> PagePower { get; set; }

        /// <summary>
        /// 包含功能点列表
        /// </summary>
        [JsonProperty("functions")]
        public List<FunctionView> FunctionPower { get; set; } = new List<FunctionView>();

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [JsonProperty("updateDate")]
        public string UpdateDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [JsonProperty("updateUser")]
        public string UpdaterName { get; set; }

        /// <summary>
        /// 是否为系统级别数据
        /// </summary>
        [JsonProperty("isSystem")]
        public bool IsSystem { get; set; }
    }
}
