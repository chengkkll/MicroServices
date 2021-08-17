using Microsoft.AspNetCore.Authorization;
using System;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// 权限特性
    /// </summary>
    public class AuthActionAttribute : Attribute, IAuthorizeData
    {
        /// <summary>
        /// 模块编码
        /// </summary>
        public string ModuleCode { get; set; }
        /// <summary>
        /// 组编码
        /// </summary>
        public string GroupCode { get; set; }
        /// <summary>
        /// 权限编码
        /// </summary>
        public string ActionCode { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 功能点名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 功能点说明
        /// </summary>
        public string Remark { get; set; }

        #region base
        /// <summary>
        /// 固定策略名称
        /// </summary>
        public string Policy { get; set; } = AuthActionRequirement.PolicyName;
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="actionCode"></param>
        public AuthActionAttribute(string actionCode) => ActionCode = actionCode;
    }
}
