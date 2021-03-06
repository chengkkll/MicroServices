using System.Collections.Generic;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 上级部门信息
    /// </summary>
    public class ParentDepartment
    {
        /// <summary>
        /// 上级部门ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 上级部门名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 本级与所有上级ID列表
        /// </summary>
        public List<string> Ids { get; set; }
    }
}
