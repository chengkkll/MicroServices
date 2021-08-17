using System;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class RoleInfo : MongoIdModel, ICreateStringModel, IUpdateStringModel, IDeleteStringModel
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

        #region 新增信息
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreaterName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        #endregion 

        #region 修改信息
        /// <summary>
        /// 修改人Id
        /// </summary>
        public string UpdaterId { get; set; }
        /// <summary>
        /// 更新人名称
        /// </summary>
        public string UpdaterName { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        #endregion 

        #region 逻辑删除
        /// <summary>
        /// 是否已逻辑删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        public string DeleteId { get; set; }
        /// <summary>
        /// 删除人名称
        /// </summary>
        public string DeleteName { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteDate { get; set; }
        #endregion
    }
}
