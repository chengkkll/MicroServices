using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class RoleInfo : MongoBusinessModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 当前角色登录后的默认页面
        /// </summary>
        public string DefaultPage { get; set; }
        /// <summary>
        /// 包含菜单列表
        /// </summary>
        public List<MenuMainView> PagePower { get; set; } = new List<MenuMainView>();
        /// <summary>
        /// 包含功能点列表
        /// </summary>
        public List<FunctionView> FunctionPower { get; set; } = new List<FunctionView>();
        /// <summary>
        /// 是否为系统级别数据
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 是否为管理员角色
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
