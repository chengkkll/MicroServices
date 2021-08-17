using System;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 部门信息  
    /// </summary>
    public class DepartmentInfo : MongoIdModel, ICreateStringModel, IUpdateStringModel, IDeleteStringModel
    {
        #region 部门基本信息
        /// <summary>
        /// 部门编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 排序的序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门描述
        /// </summary>
        public string Desc { get; set; }
        #endregion

        #region 上级信息
        /// <summary>
        /// 上级部门Id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 上级部门名称
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 上级部门ID列表
        /// </summary>
        public List<string> ParentsIds { get; set; } = new List<string>();
        /// <summary>
        /// 涉及到部门的ID列表
        /// </summary>
        public List<string> RelatedIds { get; set; } = new List<string>();
        #endregion

        #region 主管信息
        /// <summary>
        /// 部门主管ID
        /// </summary>
        public string ManageId { get; set; }
        /// <summary>
        /// 部门主管名称
        /// </summary>
        public string ManageName { get; set; }
        #endregion

        #region 扩展
        /// <summary>
        /// 扩展ID 用于部门信息的扩展
        /// </summary>
        public string ExtId { get; set; }
        #endregion

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
