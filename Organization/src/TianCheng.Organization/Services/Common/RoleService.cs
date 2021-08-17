using System;
using System.Collections.Generic;
using System.Linq;
using TianCheng.Common;
using TianCheng.Service.Core;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 角色管理    [ Service ]
    /// </summary>
    public class RoleService : MongoBusinessService<RoleDAL, RoleInfo, RoleQuery>,
        IDeleteService<RoleInfo, RoleDAL>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public RoleService(RoleDAL dal) : base(dal)
        {

        }
        #endregion

        //#region 查询方法
        ///// <summary>
        ///// 获取一个以角色名称为key的字典信息
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<string, RoleInfo> GetNameDict()
        //{
        //    Dictionary<string, RoleInfo> dict = new Dictionary<string, RoleInfo>();
        //    foreach (RoleInfo info in Dal.Queryable().Where(e => e.IsDelete == false))
        //    {
        //        if (!dict.ContainsKey(info.Name))
        //        {
        //            dict.Add(info.Name, info);
        //        }
        //    }
        //    return dict;
        //}
        //#endregion

        #region 删除方法
        /// <summary>
        /// 删除时的检查
        /// </summary>
        /// <param name="info"></param>
        protected override void DeleteRemoveCheck(RoleInfo info)
        {
            //如果角色下有员工信息，不允许删除
            EmployeeService employeeService = ServiceLoader.GetService<EmployeeService>();
            if (employeeService.CountByRoleId(info.Id.ToString()) > 0)
            {
                throw ApiException.BadRequest("角色下有员工信息，不允许删除");
            }
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化管理员角色信息
        /// </summary>
        public void InitAdmin()
        {
            // 清除已有角色
            Dal.Drop();
            // 创建一个角色
            RoleInfo admin = new RoleInfo()
            {
                Name = "系统管理员",
                Desc = "系统默认系统管理员",
                CreateDate = DateTime.Now,
                CreaterId = "",
                CreaterName = "系统初始化",
                UpdateDate = DateTime.Now,
                UpdaterId = "",
                UpdaterName = "系统初始化",
                //ProcessState = ProcessState.Edit,
                IsSystem = true,
                IsAdmin = true,
                FunctionPower = ServiceLoader.GetService<FunctionService>().SearchFunction(),
                PagePower = ServiceLoader.GetService<SystemMenuService>().SearchMenuTree()
            };
            // 持久化保存角色信息
            Dal.Save(admin);
            // 将管理员角色设置到管理员用户
            ServiceLoader.GetService<EmployeeService>().ResetSystemAdminRole(admin);
        }
        ///// <summary>
        ///// 更新管理员的角色信息
        ///// </summary>
        //public void UpdateAdminRole()
        //{
        //    // 获取管理员账号
        //    List<RoleInfo> data = Dal.Queryable().Where(e => e.Name.Contains("管理员") && e.IsSystem).ToList();
        //    // 如果管理员账号不存在，初始化后返回
        //    if (data.Count == 0)
        //    {
        //        InitAdmin();
        //        return;
        //    }

        //    // 获取设置的功能点与菜单信息。
        //    var functionPower = ServiceLoader.GetService<FunctionService>().SearchFunction();
        //    var pagePower = ServiceLoader.GetService<SystemMenuService>().SearchMenuTree();
        //    // 更新数据
        //    foreach (RoleInfo role in data)
        //    {
        //        role.FunctionPower = functionPower;
        //        role.PagePower = pagePower;
        //        role.UpdateDate = DateTime.Now;
        //        Dal.UpdateObject(role);
        //    }
        //}
        #endregion

        ///// <summary>
        ///// 菜单修改后的事件处理
        ///// </summary>
        ///// <param name="menuInfo"></param>
        ///// <param name="oldMenuInfo"></param>
        ///// <param name="logonInfo"></param>
        //internal static void OnMenuUpdated(MenuMainInfo menuInfo, MenuMainInfo oldMenuInfo, TokenLogonInfo logonInfo)
        //{
        //    if (menuInfo.Name == oldMenuInfo.Name && menuInfo.Icon == oldMenuInfo.Icon && menuInfo.Link == oldMenuInfo.Link && menuInfo.Index == oldMenuInfo.Index)
        //    {
        //        return;
        //    }
        //    string menuId = menuInfo.Id.ToString();
        //    RoleDAL roleDAL = ServiceLoader.GetService<RoleDAL>();
        //    foreach (var role in roleDAL.Queryable().Where(e => e.PagePower.Any(p => p.Id == menuId)))
        //    {
        //        var menu = role.PagePower.Where(e => e.Id == menuId).FirstOrDefault();
        //        menu.Name = menuInfo.Name;
        //        menu.Icon = menuInfo.Icon;
        //        menu.Link = menuInfo.Link;
        //        menu.Index = menuInfo.Index;
        //        roleDAL.UpdateObject(role);
        //    }
        //}
    }
}
