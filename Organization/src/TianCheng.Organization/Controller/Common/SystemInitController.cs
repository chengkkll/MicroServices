using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Organization.Services;
using TianCheng.Service.Core;

namespace TianCheng.Organization.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("System")]
    public class SystemInitController : LogController
    {
        #region 构造方法
        private readonly EmployeeService employeeService;
        private readonly FunctionService functionService;
        private readonly SystemMenuService menuService;
        private readonly RoleService roleService;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="eService"></param>
        /// <param name="fService"></param>
        /// <param name="mService"></param>
        /// <param name="rService"></param>
        public SystemInitController(EmployeeService eService, FunctionService fService, SystemMenuService mService, RoleService rService)
        {
            employeeService = eService;
            functionService = fService;
            menuService = mService;
            roleService = rService;
        }
        #endregion

        /// <summary>
        /// 重置数据库
        /// </summary>
        /// <remarks>清除已有数据，重置默认数据。【重置数据包括：菜单、功能点、角色、用户】 </remarks>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.System.Init")]
        [SwaggerOperation(Tags = new[] { "系统管理-系统设置" })]
        [HttpPost("ResetDB")]
        public void ResetDB()
        {
            //初始化系统功能点列表
            functionService.InitOrganization();
            //初始化用户信息
            employeeService.InitAdmin();
            //初始化系统菜单
            menuService.Init();
            //初始化角色信息
            roleService.InitAdmin();
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <remarks>无需登录的初始化数据库，必须系统中无用户信息时才可执行</remarks>
        [SwaggerOperation(Tags = new[] { "系统管理-系统设置" })]
        [HttpPost("InitDB")]
        public void InitDB()
        {
            if (employeeService.Count() > 0)
            {
                ApiException.ThrowBadRequest("系统中拥有用户信息，无密码初始化失败。您可以通过管理员账号登陆后再初始化");
            }
            ResetDB();
        }
    }
}
