using System;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 登录历史记录
    /// </summary>
    public class LoginHistoryInfo : MongoIdModel, ICreateStringModel
    {
        /// <summary>
        /// 登录用户的ID
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// 登录用户
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属部门ID
        /// </summary>
        public string DepartmentID { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 请求IP地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 请求IP地址推断城市
        /// </summary>
        public string IpAddressCityName { get; set; }

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

    }
}
