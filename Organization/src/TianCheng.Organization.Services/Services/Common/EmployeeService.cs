using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TianCheng.Common;
using TianCheng.Service.Core;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using Microsoft.Extensions.Logging;
using DotNetCore.CAP;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 员工管理    [ Service ]
    /// </summary>
    public class EmployeeService : MongoBusinessService<EmployeeDAL, EmployeeInfo, EmployeeQuery>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public EmployeeService(EmployeeDAL dal) : base(dal)
        {
        }
        #endregion

        #region 查询方法

        /// <summary>
        /// 获取所有可用的员工列表，按部门分组
        /// </summary>
        /// <returns></returns>
        public List<EmployeeGroupByDepartment> GetEmployeeByDepartment(TokenLogonInfo logonInfo = null)
        {
            var query = Dal.Queryable().Where(e => e.IsDelete == false);
            // 如果登录信息不为空，查询当前用户下的所有成员信息
            if (logonInfo != null)
            {
                query = query.Where(e => e.ParentDepartment != null && e.ParentDepartment.Ids.Contains(logonInfo.DepartmentId));
            }
            var employeeList = query.ToList();
            var depList = ServiceLoader.GetService<DepartmentDAL>().Queryable().ToList();
            List<EmployeeGroupByDepartment> result = new List<EmployeeGroupByDepartment>();
            foreach (var item in employeeList.GroupBy(e => e.Department.Id))
            {
                var dep = depList.Where(e => e.IdString == item.Key).FirstOrDefault();
                result.Add(new EmployeeGroupByDepartment()
                {
                    Id = item.Key,
                    Name = dep.Name,
                    ParentId = dep.ParentId,
                    Employees = item.Select(e => { return new SelectView { Id = e.Id.ToString(), Name = e.Name, Code = e.Department != null ? e.Department.Id : "" }; })
                });
            }

            return result;
        }
        #endregion

        #region 新增 / 修改方法
        /// <summary>
        /// 设置角色是否必填
        /// </summary>
        public static bool RequiredRole { get; set; } = true;

        /// <summary>
        /// 设置部门是否必填
        /// </summary>
        public static bool RequiredDepartment { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public static bool CanRepeatLogonAccount { get; set; } = false;
        /// <summary>
        /// 保存的校验
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void SavingCheck(EmployeeInfo info, TokenLogonInfo logonInfo)
        {
            // 处理角色的验证
            if (RequiredRole)
            {
                if (info.Role == null)
                {
                    throw ApiException.BadRequest("请指定用户的角色");
                }
            }

            // 处理部门的验证
            if (RequiredDepartment)
            {
                if (info.Department == null)
                {
                    throw ApiException.BadRequest("请指定用户的部门");
                }

            }

            // 验证登录账号
            if (CanRepeatLogonAccount == false)
            {
                // 检查登录账号是否重复，忽略当前账号id，检查已删除数据。
                if (Dal.HasRepeat(info.Id, "LogonAccount", info.LogonAccount, false))
                {
                    throw ApiException.BadRequest("登陆账号已存在，无法新增用户");
                }
            }
        }

        /// <summary>
        /// 保存的前置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void Saving(EmployeeInfo info, TokenLogonInfo logonInfo)
        {
            //设置员工状态
            info.State = UserState.Enable;
            info.ProcessState = ProcessState.Enable;

            //完善数据
            //1、完善部门信息
            if (info.Department != null && !string.IsNullOrWhiteSpace(info.Department.Id))
            {
                string depId = info.Department.Id;
                DepartmentDAL departmentDAL = ServiceLoader.GetService<DepartmentDAL>();
                DepartmentInfo depInfo = departmentDAL.SingleByStringId(depId);
                if (depInfo == null)
                {
                    throw ApiException.BadRequest("选择的部门信息不存在，请刷新页面再尝试");
                }
                info.Department = new SelectView() { Id = depId, Name = depInfo.Name, Code = depInfo.Code };
                info.ParentDepartment.Id = depInfo.ParentId;
                info.ParentDepartment.Name = depInfo.Name;
                info.ParentDepartment.Ids = depInfo.ParentsIds;
            }

            //2、完善角色信息
            if (info.Role != null && !string.IsNullOrWhiteSpace(info.Role.Id))
            {
                string roleId = info.Role.Id;
                RoleDAL roleDAL = ServiceLoader.GetService<RoleDAL>();
                RoleInfo role = roleDAL.SingleByStringId(roleId);
                if (role == null)
                {
                    throw ApiException.BadRequest("选择的角色信息不存在，请刷新页面再尝试");
                }
                info.Role = new SelectView() { Id = roleId, Name = role.Name, Code = role.Code };
            }
        }
        #endregion

        //#region 角色信息修改时的事件处理
        ///// <summary>
        ///// 角色更新的后置处理
        ///// </summary>
        ///// <param name="roleInfo">新的角色信息</param>
        ///// <param name="oldInfo">旧的角色信息</param>
        ///// <param name="logonInfo"></param>
        //internal static void OnRoleUpdated(RoleInfo roleInfo, RoleInfo oldInfo, TokenLogonInfo logonInfo)
        //{
        //    if (roleInfo.Name != oldInfo.Name)
        //    {
        //        // 更新员工信息
        //        ThreadPool.QueueUserWorkItem(h =>
        //        {
        //            EmployeeDAL dal = ServiceLoader.GetService<EmployeeDAL>();
        //            // 更新部门信息
        //            dal.UpdateRoleInfo(roleInfo);
        //        });
        //    }
        //}
        //#endregion

        #region 初始化
        /// <summary>
        /// 获取系统默认的管理员
        /// </summary>
        /// <returns></returns>
        public EmployeeInfo GetAdminstrator()
        {
            return Dal.Queryable().Where(e => e.IsSystem == true && e.Type == EmployeeType.Administrator).FirstOrDefault();
        }
        /// <summary>
        /// 初始化系统的用户信息  删除已有的用户，设置一个账号密码为a的管理账户
        /// </summary>
        /// <remarks>
        /// 初始化的用户没有角色信息，角色信息在初始化角色时更新管理员用户（方法ResetSystemAdminRole）
        /// </remarks>
        public void InitAdmin()
        {
            //删除已有用户
            Dal.Drop();
            EmployeeInfo emp = new EmployeeInfo()
            {
                LogonAccount = "a",
                LogonPassword = "a",
                Name = "预制管理员",
                IsSystem = true,
                Type = EmployeeType.Administrator,
                ProcessState = ProcessState.Edit,
                State = UserState.Enable,
                CreateDate = DateTime.Now,
                CreaterId = "",
                CreaterName = "系统初始化",
                UpdateDate = DateTime.Now,
                UpdaterId = "",
                UpdaterName = "系统初始化",
                IsDelete = false
            };
            // 持久化用户信息
            Dal.InsertObject(emp);
        }

        /// <summary>
        /// 设置预制管理员的角色信息
        /// </summary>
        /// <param name="role"></param>
        public void ResetSystemAdminRole(RoleInfo role)
        {
            // 查询所有的系统预制的管理员账号，更新其角色信息
            foreach (var emp in Dal.Queryable().Where(e => e.IsSystem == true && e.Type == EmployeeType.Administrator))
            {
                emp.Role = ObjectTran.Tran<SelectView>(role);
                Dal.UpdateObject(emp);
            }
        }
        #endregion

        #region 修改登录密码
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public ResultView UpdatePassword(string id, string oldPwd, string newPwd)
        {

            if (string.IsNullOrWhiteSpace(newPwd.Trim()))
            {
                throw ApiException.BadRequest("请输入新密码");
            }
            var emp = Dal.SingleByStringId(id);
            if (!emp.LogonPassword.Equals(oldPwd))
            {
                throw ApiException.BadRequest("原密码输入错误");
            }
            emp.LogonPassword = newPwd;
            Dal.UpdateObject(emp);
            return ResultView.Success(emp.IdString);
        }

        /// <summary>
        /// 更新所有人的密码
        /// </summary>
        public void UpdateAllPassword(string password = "")
        {
            foreach (var employee in Dal.Queryable())
            {
                employee.LogonPassword = string.IsNullOrWhiteSpace(password) ? Guid.NewGuid().ToString("N").Substring(0, 8) : password;
                Dal.UpdateObject(employee);
            }
        }
        #endregion

        #region 设置员工状态
        /// <summary>
        /// 禁止某些员工登录系统，主要用于员工出差，或暂时不允许登录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultView SetDisable(string id)
        {
            var emp = SingleByStringId(id);
            emp.State = UserState.Disable;
            emp.ProcessState = ProcessState.Disable;
            Dal.UpdateObject(emp);
            return ResultView.Success(id);
        }

        /// <summary>
        /// 恢复某些员工禁止登录系统的状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultView SetEnable(string id)
        {
            var emp = SingleByStringId(id);
            emp.State = UserState.Enable;
            emp.ProcessState = ProcessState.Enable;
            Dal.UpdateObject(emp);
            return ResultView.Success(id);
        }
        /// <summary>
        /// 解锁某些用户的登录状态 - 用户连续多次由于密码错误而登录失败时，将会为用户设置登录锁状态，本功能用于解除这种登录锁的状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultView SetUnlock(string id)
        {
            var emp = SingleByStringId(id);
            emp.State = UserState.Enable;
            emp.ProcessState = ProcessState.Enable;
            Dal.UpdateObject(emp);
            return ResultView.Success(id);
        }
        #endregion

        #region 数量统计
        /// <summary>
        /// 查看某角色下的可用员工的个数
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int CountByRoleId(string roleId)
        {
            return Dal.Queryable().Where(e => e.Role != null && !string.IsNullOrEmpty(e.Role.Id) && e.Role.Id == roleId && e.IsDelete == false && e.State == UserState.Enable).Count();
        }
        /// <summary>
        /// 判断角色是否使用
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool IsUseRoleId(string roleId)
        {
            return Dal.Queryable().Any(e => e.Role != null && !string.IsNullOrEmpty(e.Role.Id) && e.Role.Id == roleId && e.IsDelete == false && e.State == UserState.Enable);
        }
        /// <summary>
        /// 查看某部门下的可用员工的个数
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        internal int CountByDepartmentId(string departmentId)
        {
            return Dal.Queryable().Where(e => e.Department != null && !string.IsNullOrEmpty(e.Department.Id) && e.Department.Id == departmentId && e.IsDelete == false && e.State == UserState.Enable).Count();
        }
        #endregion

        #region 获取登录信息
        /// <summary>
        /// 根据用户ID获取用户登录信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public TokenLogonInfo GetLogonInfo(string employeeId)
        {
            var employee = SingleByStringId(employeeId);
            return new TokenLogonInfo
            {
                Id = employeeId,
                Name = employee.Name,
                DepartmentId = employee.Department.Id,
                DepartmentName = employee.Department.Name,
                RoleId = employee.Role.Id
            };
        }
        /// <summary>
        /// 获取管理员账号的登录信息
        /// </summary>
        /// <returns></returns>
        public TokenLogonInfo GetAdminLogonInfo()
        {
            EmployeeInfo admin = GetAdminstrator();
            return new TokenLogonInfo
            {
                Id = admin.IdString,
                Name = admin.Name,
                RoleId = admin.Role.Id,
                DepartmentId = admin.Department.Id,
                DepartmentName = admin.Department.Name
            };
        }
        #endregion
    }
}
