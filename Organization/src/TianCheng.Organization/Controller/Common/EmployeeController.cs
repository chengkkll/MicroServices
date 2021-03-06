using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Service.Core;
using TianCheng.Organization.Model;
using TianCheng.Organization.Services;
using Microsoft.Extensions.Logging;

namespace TianCheng.Organization.Controller
{
    /// <summary>
    /// 员工管理
    /// </summary>
    [Produces("application/json")]
    [Route("Employee")]
    public class EmployeeController : MongoController<EmployeeService>
    {
        #region 构造方法
        private readonly RoleService _roleService;
        private readonly DepartmentService _departmentService;

        /// <summary> 
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        /// <param name="roleService"></param>        
        /// <param name="departmentService"></param>   
        public EmployeeController(EmployeeService service, RoleService roleService, DepartmentService departmentService) : base(service)
        {
            _roleService = roleService;
            _departmentService = departmentService;
        }
        #endregion

        #region 获取当前用户权限信息
        /// <summary>
        /// 获取当前用户权限信息
        /// </summary>
        /// <returns></returns>
        /// <power>登录</power>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.Employee.Power")]

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Power")]
        [HttpGet("Power")]
        [SwaggerOperation(Tags = new[] { "系统管理-登录验证" })]
        public LogonPowerView Power()
        {
            var LogonInfo = this.GetStringTokenInfo();
            LogonPowerView view = new LogonPowerView()
            {
                Id = LogonInfo.EmployeeId,
                Name = LogonInfo.Name,
                Role = new RoleSimpleView() { Id = LogonInfo.RoleId }
            };
            var role = _roleService.SingleById<RoleView>(view.Role.Id);
            if (role != null)
            {
                view.Role.Name = role.Name;
                view.Role.DefaultPage = role.DefaultPage;
                view.Menu = role.PagePower;
                view.Functions = role.FunctionPower;
            }
            view.Department = new NameViewModel() { Id = LogonInfo.DepartmentId };
            if (!string.IsNullOrEmpty(view.Department.Id))
            {
                var dep = _departmentService.SingleById(view.Department.Id);
                if (dep != null)
                {
                    view.Department.Name = dep.Name;
                }
            }
            return view;
        }
        #endregion

        #region 新增修改数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="view">请求体中放置新增对象的信息</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Create")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPost("")]
        public ResultView Create([FromBody][CreateView] EmployeeView view)
        {
            return Service.Create(view, this);
        }

        /// <summary>
        /// 修改q
        /// </summary>
        /// <param name="view">请求体中带入修改对象的信息</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Update")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPut("")]
        public ResultView Update([FromBody][UpdateView] EmployeeView view)
        {
            return Service.Update(view, this);
        }

        ///// <summary>
        ///// 修改昵称信息
        ///// </summary>
        ///// <param name="view">请求体中带入修改对象的信息</param>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.Employee.SetNickname")]
        //[SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        //[HttpPatch("SetNickname")]
        //public ResultView SetNickname([FromBody]EmployeeView view)
        //{
        //    var info = _Service.SearchById(view.Id);
        //    info.Nickname = view.Nickname;
        //    return _Service.Update(info, LogonInfo);
        //}

        ///// <summary>
        ///// 修改职位信息
        ///// </summary>
        ///// <param name="view">请求体中带入修改对象的信息</param>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.Employee.SetPosition")]
        //[SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        //[HttpPatch("SetPosition")]
        //public ResultView SetPosition([FromBody]EmployeeView view)
        //{
        //    var info = _Service.SearchById(view.Id);
        //    info.Position = view.Position;            
        //    return _Service.Update(info, LogonInfo);
        //}
        #endregion

        #region 数据删除
        /// <summary>
        /// 设置离职    
        /// </summary>
        /// <param name="id">要逻辑删除的对象id</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Delete")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpDelete("Delete/{id}")]
        public ResultView Delete(string id)
        {
            return Service.Delete(id, this);
        }
        /// <summary>
        /// 设置在职    
        /// </summary>
        /// <param name="id">要逻辑删除的对象id</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Delete")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("UnDelete/{id}")]
        public ResultView UnDelete(string id)
        {
            return Service.Undelete(id, this);
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">要逻辑删除的对象id</param>
        /// <power>粉碎数据</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Remove")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpDelete("Remove/{id}")]
        public Task<ResultView> Remove(string id)
        {
            return Service.Remove(id, this);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 根据ID获取一条用户信息    
        /// </summary>
        /// <param name="id">查询的用户ID</param>
        /// <power>详情</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Single")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("{id}")]
        public EmployeeView SearchById(string id)
        {
            return Service.SingleById<EmployeeView>(id);
        }
        /// <summary>
        /// 获取当前用户信息    
        /// </summary>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Load")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("")]
        public EmployeeView Load()
        {
            return Service.SingleById<EmployeeView>(this.GetStringTokenInfo().EmployeeId);
        }

        /// <summary>
        /// 根据条件获取有分页信息的查询列表    
        /// </summary>
        /// <remarks> 
        /// 
        ///     排序规则包含： 
        /// 
        ///         name            : 按名称排列          
        ///         department.name : 按部门名称排列
        ///         role.name       : 按角色名称倒序排列
        ///         state           : 按状态正序排列
        ///         updateDate      : 更新时间排列     为默认排序
        ///         
        /// </remarks>
        /// <param name="queryInfo">查询信息。（包含分页信息、查询条件、排序条件）
        /// 排序规则参见上面的描述
        /// </param>
        /// <power>查询</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Search")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPost("Search")]
        public PagedResult<EmployeeView> SearchPage([FromBody] EmployeeQuery queryInfo)
        {
            var LogonInfo = this.GetStringTokenInfo();
            var role = _roleService.SingleById(LogonInfo.RoleId);
            if (!role.Name.Contains("管理员"))
            {
                //非管理员只能查看自己部门下的用户
                queryInfo.RootDepartment = LogonInfo.DepartmentId;
            }

            return Service.FilterPage<EmployeeView>(queryInfo, this);
        }

        /// <summary>
        /// 根据条件查询数据列表  无分页效果    
        /// </summary>
        /// <remarks> 
        /// 
        ///     排序规则包含： 
        /// 
        ///         name            : 按名称排列          
        ///         department.name : 按部门名称排列
        ///         role.name       : 按角色名称倒序排列
        ///         state           : 按状态正序排列
        ///         updateDate      : 更新时间排列     为默认排序
        ///         
        /// </remarks>
        /// <param name="queryInfo">查询信息。（包含查询条件、排序条件）
        /// 排序规则参见上面的描述
        /// </param>
        /// <power>查询</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Search")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPost("List")]
        public IEnumerable<EmployeeView> SearchFilter([FromBody] EmployeeQuery queryInfo)
        {
            return Service.Filter<EmployeeView>(queryInfo, this);
        }

        /// <summary>
        /// 为下拉列表提供数据 - 获取所有的员工列表      
        /// </summary>
        /// <power>列表选择</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("Select")]
        public IEnumerable<SelectView> Select()
        {
            EmployeeQuery query = new EmployeeQuery() { Page = QueryPagination.DefaultObject };
            query.Sort.Property = "name";
            return Service.Select(query);
        }
        /// <summary>
        /// 为下拉列表提供数据 - 获取所有的员工列表      
        /// </summary>
        /// <power>列表选择</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("SelectView")]
        public IEnumerable<EmployeeSelectView> SelectDepartment()
        {
            //List<EmployeeSelectView> viewList = new List<EmployeeSelectView>();
            //foreach (var info in Service.SearchQueryable().OrderBy(e => e.Name).ToList())
            //{
            //    viewList.Add(new EmployeeSelectView
            //    {
            //        Id = info.Id.ToString(),
            //        Name = info.Name,
            //        Code = info.Code,
            //        DepartmentId = info.Department.Id,
            //        DepartmentName = info.Department.Name,
            //        RoleId = info.Role.Id,
            //        RoleName = info.Role.Name,
            //        IsDelete = info.IsDelete
            //    });
            //}
            //return viewList;

            return Service.Filter<EmployeeSelectView>(new EmployeeQuery() { Sort = new QuerySort { Property = "Name" } });
        }
        /// <summary>
        /// 为下拉列表提供数据 - 获取某部门下的员工列表（不显示已离职的员工）   
        /// </summary>
        /// <param name="depId">部门id</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("SelectByDep/{depId}")]
        public IEnumerable<SelectView> SelectByDepartment(string depId)
        {
            EmployeeQuery query = new EmployeeQuery() { DepartmentId = depId, Page = QueryPagination.DefaultObject, HasDelete = 0 };
            query.Sort.Property = "name";
            return Service.Select(query);
        }
        /// <summary>
        /// 为下拉列表提供数据 - 获取某部门下的员工列表（显示已离职的员工）   
        /// </summary>
        /// <param name="depId">部门id</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("SelectByDep/{depId}/All")]
        public IEnumerable<SelectView> SelectByDepartmentAll(string depId)
        {
            EmployeeQuery query = new EmployeeQuery() { DepartmentId = depId, Page = QueryPagination.DefaultObject, HasDelete = 1 };
            query.Sort.Property = "name";
            return Service.Select(query);
        }
        /// <summary>
        /// 为下拉列表提供数据 - 获取某部门下的员工列表   
        /// </summary>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("SelectMyDepartment")]
        public IEnumerable<SelectView> SelectByMyDepartment()
        {
            EmployeeQuery query = new EmployeeQuery() { DepartmentId = this.GetStringTokenInfo().DepartmentId, Page = QueryPagination.DefaultObject };
            query.Sort.Property = "name";
            return Service.Select(query);
        }

        /// <summary>
        /// 获取所有可用的员工列表，按部门分组
        /// </summary>
        /// <returns></returns>
        /// <power>列表选择</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("SelectGroupByDepartment")]
        public List<EmployeeGroupByDepartment> GetEmployeeByDepartment()
        {
            return Service.GetEmployeeByDepartment();
        }
        /// <summary>
        /// 获取所有可用的员工列表，按部门分组
        /// </summary>
        /// <returns></returns>
        /// <power>列表选择</power>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("SelectGroupByDepartment/My")]
        public List<EmployeeGroupByDepartment> GetMyEmployeeByDepartment()
        {
            return Service.GetEmployeeByDepartment(this.GetStringTokenInfo());
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改其他用户密码
        /// </summary>
        /// <param name="view">用户id</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.UpdatePasswordOther")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("UpPwd/Other")]
        public ResultView UpdatePasswordOther([FromBody] UpdatePasswordView view)
        {
            return Service.UpdatePassword(view.Id, view.OldPwd, view.NewPwd);
        }

        /// <summary>
        /// 修改当前用户密码
        /// </summary>
        /// <param name="view">用户id</param>

        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.UpdatePasswordMe")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("UpPwd/Me")]
        public ResultView UpdatePasswordMe([FromBody] UpdatePasswordMeView view)
        {
            Service.UpdatePassword(this.GetStringTokenInfo().EmployeeId, view.OldPwd, view.NewPwd);
            return ResultView.Success("密码修改成功");
        }
        #endregion

        #region 员工状态控制
        /// <summary>
        /// 禁止某些员工登录系统，主要用于员工出差，或暂时不允许登录
        /// </summary>
        /// <param name="id">用户id</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Disable")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("Disable/{id}")]
        public ResultView Disable(string id)
        {
            return Service.SetDisable(id);
        }

        /// <summary>
        /// 恢复员工禁止登录系统的状态
        /// </summary>
        /// <param name="id">用户id</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Enable")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("Enable/{id}")]
        public ResultView Enable(string id)
        {
            return Service.SetEnable(id);
        }
        /// <summary>
        /// 解锁用户的登录状态 - 用户连续多次由于密码错误而登录失败时，将会为用户设置登录锁状态，本功能用于解除这种登录锁的状态
        /// </summary>
        /// <param name="id">用户id</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Unlock")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("Unlock/{id}")]
        public ResultView Unlock(string id)
        {
            return Service.SetUnlock(id);
        }
        #endregion

        /// <summary>
        /// 按属性修改用户信息
        /// </summary>
        /// <remarks> 
        /// 
        ///     property包含（不区分大小写）： 
        /// 
        ///         Position      : 职位          
        ///         Name          : 名称 
        ///         Nickname      : 昵称
        ///         Mobile        : 手机电话
        ///         Telephone     : 座机电话
        ///         
        /// </remarks>
        /// <param name="view"></param>
        /// <returns></returns>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Update")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("SetProperty")]
        public ResultView SetProperty([FromBody] SetPropertyView view)
        {
            if (string.IsNullOrWhiteSpace(view.PropertyName))
            {
                ApiException.ThrowBadRequest("属性名不能为空");
            }
            var LogonInfo = this.GetStringTokenInfo();
            var info = Service.SingleById(LogonInfo.EmployeeId);
            switch (view.PropertyName.ToLower())
            {
                case "position": { info.Position = view.PropertyValue; break; }
                case "name": { info.Name = view.PropertyValue; break; }
                case "nickname": { info.Nickname = view.PropertyValue; break; }
                case "mobile": { info.Mobile = view.PropertyValue; break; }
                case "telephone": { info.Telephone = view.PropertyValue; break; }
            }
            Service.Update(info, LogonInfo);
            return ResultView.Success("信息修改成功");
        }

        /// <summary>
        /// 更新所有用户的密码
        /// </summary>
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpPatch("UpdateAllPassword")]
        public ResultView UpdateAllPassword()
        {
            Service.UpdateAllPassword("123456");
            return ResultView.Success();
        }

        /// <summary>
        /// 更新所有用户的密码
        /// </summary>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Employee.Power")]
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("Test")]
        public string Test()
        {
            //string res = Service.GetShowItem<EmployeeView, EmployeeQuery>(this.GetStringTokenInfo());
            //Service.Test();
            //Service.ClearOccupy("abc");
            return Service.GetSearchQuery(this);
            //return ResultView.Success(this.GetEmployeeStringId());
        }

        //#region 忘记密码

        //#endregion
    }
}
