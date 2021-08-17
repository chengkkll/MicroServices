using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.Service.Core;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using DotNetCore.CAP;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 菜单      [ Service ]
    /// </summary>
    public class SystemMenuService : MongoBusinessService<MenuDAL, MenuInfo, MenuQuery>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public SystemMenuService(MenuDAL dal) : base(dal)
        {

        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public void Init()
        {
            // 获取管理员账号的登录信息
            StringTokenInfo adminLogonInfo = ServiceLoader.GetService<EmployeeService>().GetAdminLogonInfo();
            // 清除已有的菜单
            Dal.Drop();
            // 添加个人信息主菜单
            MenuInfo mainPersonal = new MenuInfo() { Name = "个人信息", Index = 51, Icon = "el-icon-eieco-personal" };
            Create(mainPersonal, adminLogonInfo);
            Create(new MenuInfo() { Name = "个人中心", Index = 1, Icon = "el-icon-menu", MainId = mainPersonal.IdString, Link = "PersonalCenter.Common" }, adminLogonInfo);
            Create(new MenuInfo() { Name = "修改密码", Index = 2, Icon = "el-icon-menu", MainId = mainPersonal.IdString, Link = "PersonalCenter.ChangePwd" }, adminLogonInfo);
            // 添加系统管理主菜单
            MenuInfo mainSystem = new MenuInfo() { Name = "系统管理", Index = 52, Icon = "el-icon-eieco-system-manage" };
            Create(mainSystem, adminLogonInfo);
            Create(new MenuInfo() { Name = "部门管理", Index = 13, Icon = "el-icon-menu", MainId = mainSystem.IdString, Link = "System.Departments" }, adminLogonInfo);
            Create(new MenuInfo() { Name = "员工管理", Index = 14, Icon = "el-icon-menu", MainId = mainSystem.IdString, Link = "System.Employees", }, adminLogonInfo);
            Create(new MenuInfo() { Name = "角色管理", Index = 15, Icon = "el-icon-menu", MainId = mainSystem.IdString, Link = "System.Roles", }, adminLogonInfo);
            Create(new MenuInfo() { Name = "菜单管理", Index = 19, Icon = "el-icon-menu", MainId = mainSystem.IdString, Link = "System.Menus", }, adminLogonInfo);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取主菜单的树形结构
        /// </summary>
        /// <returns></returns>
        public List<MenuMainView> SearchMenuTree()
        {
            List<MenuInfo> all = Dal.Queryable().ToList();
            List<MenuMainView> result = new List<MenuMainView>();
            foreach (MenuInfo main in all.Where(e => string.IsNullOrWhiteSpace(e.MainId)).OrderBy(e => e.Index))
            {
                MenuMainView m = ObjectTran.Tran<MenuMainView>(main);
                var sub = all.Where(e => e.MainId == main.IdString).OrderBy(e => e.Index).ToList();
                m.SubMenu = ObjectTran.Tran<List<MenuSubView>>(sub);
                result.Add(m);
            }
            return result;
        }

        ///// <summary>
        ///// 根据主菜单名称获取一个主菜单信息
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //internal MenuInfo SingleMainByName(string name)
        //{
        //    return Dal.Queryable().Where(e => e.Name == name && string.IsNullOrWhiteSpace(e.MainId)).FirstOrDefault();
        //}
        #endregion

        #region 保存操作
        /// <summary>
        /// 保存菜单的前置验证
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void SavingCheck(MenuInfo info, TokenBase logonInfo)
        {
            if (string.IsNullOrWhiteSpace(info.Name))
            {
                ApiException.ThrowBadRequest("菜单名称不能为空");
            }
            if (info.Index <= 0)
            {
                info.Index = 10;
            }
            // 子菜单保存
            if (!string.IsNullOrWhiteSpace(info.MainId))
            {
                if (string.IsNullOrWhiteSpace(info.Link))
                {
                    ApiException.ThrowBadRequest("子菜单的地址不能为空");
                }
            }
        }

        /// <summary>
        /// 更新菜单后操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        protected override void Updated(MenuInfo info, MenuInfo old, TokenBase logonInfo)
        {
            // 获取消息总线对象。
            ICapPublisher capBus = ServiceLoader.GetService<ICapPublisher>();
            MenuUpdateView uv = new MenuUpdateView()
            {
                NewMenu = ObjectTran.Tran<MenuMainView>(info),
                OldMenu = ObjectTran.Tran<MenuMainView>(old),
                LogonInfo = (StringTokenInfo)logonInfo
            };
            // 发送消息处理
            capBus.Publish(MqBus.MenuMq.Update, uv);
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除菜单时的操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void Removing(MenuInfo info, TokenBase logonInfo)
        {
            // 删除菜单时。同时删除所有的下级菜单
            foreach (var menu in Dal.Queryable().Where(e => e.MainId.Contains(info.IdString)))
            {
                Dal.RemoveObject(menu);
            }
        }
        #endregion
    }


    ///// <summary>
    /////  菜单      [ Service ]
    ///// </summary>
    //public class MenuService : MongoBusinessService<MenuMainDAL, MenuMainInfo, MenuQuery>
    //{
    //    #region 构造方法
    //    /// <summary>
    //    /// 构造方法
    //    /// </summary>
    //    /// <param name="dal"></param>
    //    public MenuService(MenuMainDAL dal) : base(dal)
    //    {

    //    }

    //    #endregion

    //    private readonly MenuType DefaultMenuType = MenuType.ManageSingle;

    //    #region 查询
    //    /// <summary>
    //    /// 获取主菜单的树形结构
    //    /// </summary>
    //    /// <param name="menuType"></param>
    //    /// <returns></returns>
    //    public List<MenuMainView> SearchMainTree(MenuType menuType = MenuType.ManageSingle)
    //    {
    //        var query = Dal.Queryable();
    //        switch (menuType)
    //        {
    //            case MenuType.None: { break; }
    //            case MenuType.ManageMultiple:
    //            case MenuType.ManageSingle: { query = query.Where(e => e.Type == menuType); break; }
    //            default: { query = query.Where(e => e.Type == MenuType.ManageSingle); break; }
    //        }
    //        var list = query.OrderBy(e => e.Index).ToList();
    //        return ObjectTran.Tran<List<MenuMainView>>(list);
    //    }
    //    /// <summary>
    //    /// 根据主菜单名称获取一个主菜单信息
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    private MenuMainInfo GetMainByName(string name)
    //    {
    //        return Dal.Queryable().Where(e => e.Name == name).FirstOrDefault();
    //    }
    //    #endregion

    //    #region 初始化
    //    /// <summary>
    //    /// 初始化菜单
    //    /// </summary>
    //    public async void Init()
    //    {
    //        List<MenuMainInfo> mainList = new List<MenuMainInfo>();
    //        MenuMainInfo mainPersonal = new MenuMainInfo() { Name = "个人信息", Index = 51, Icon = "el-icon-eieco-personal" };
    //        mainPersonal.SubMenu.Add(new MenuSubInfo() { Name = "个人中心", Link = "PersonalCenter.Common", Index = 1, Icon = "el-icon-menu", Type = MenuType.ManageSingle });
    //        mainPersonal.SubMenu.Add(new MenuSubInfo() { Name = "修改密码", Link = "PersonalCenter.ChangePwd", Index = 2, Icon = "el-icon-menu", Type = MenuType.ManageSingle });
    //        mainList.Add(mainPersonal);

    //        MenuMainInfo mainSystem = new MenuMainInfo() { Name = "系统管理", Index = 52, Icon = "el-icon-eieco-system-manage" };
    //        mainSystem.SubMenu.Add(new MenuSubInfo() { Name = "部门管理", Link = "System.Departments", Index = 13, Icon = "el-icon-menu", Type = MenuType.ManageSingle });
    //        mainSystem.SubMenu.Add(new MenuSubInfo() { Name = "员工管理", Link = "System.Employees", Index = 14, Icon = "el-icon-menu", Type = MenuType.ManageSingle });
    //        mainSystem.SubMenu.Add(new MenuSubInfo() { Name = "角色管理", Link = "System.Roles", Index = 15, Icon = "el-icon-menu", Type = MenuType.ManageSingle });
    //        mainSystem.SubMenu.Add(new MenuSubInfo() { Name = "菜单管理", Link = "System.Menus", Index = 19, Icon = "el-icon-menu", Type = MenuType.ManageSingle });
    //        mainList.Add(mainSystem);

    //        //MenuServiceOption.Option.InitMenuData?.Invoke(mainList);

    //        Dal.Drop();
    //        foreach (var main in mainList)
    //        {
    //            await Create(main, new TokenLogonInfo() { });
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected override void Saving(MenuMainInfo info, TokenLogonInfo logonInfo)
    //    {
    //        if (info.Type == MenuType.None)
    //        {
    //            info.Type = DefaultMenuType;
    //        }
    //    }
    //    #endregion

    //    /// <summary>
    //    /// 保存主菜单的前置验证
    //    /// </summary>
    //    /// <param name="info"></param>
    //    /// <param name="logonInfo"></param>
    //    protected override void SavingCheck(MenuMainInfo info, TokenLogonInfo logonInfo)
    //    {
    //        if (string.IsNullOrWhiteSpace(info.Name))
    //        {
    //            ApiException.ThrowBadRequest("主菜单名称不能为空");
    //        }
    //        if (info.Index <= 0)
    //        {
    //            info.Index = 10;
    //        }
    //        if (info.Type == MenuType.None)
    //        {
    //            info.Type = DefaultMenuType;
    //        }

    //        foreach (var sub in info.SubMenu)
    //        {
    //            if (string.IsNullOrWhiteSpace(sub.Name))
    //            {
    //                ApiException.ThrowBadRequest("子菜单名称不能为空");
    //            }
    //            if (string.IsNullOrWhiteSpace(sub.Link))
    //            {
    //                ApiException.ThrowBadRequest("子菜单的地址不能为空");
    //            }
    //            if (sub.Index <= 0)
    //            {
    //                sub.Index = 10;
    //            }
    //            if (sub.Type == MenuType.None)
    //            {
    //                sub.Type = DefaultMenuType;
    //            }
    //        }
    //    }


    //    /// <summary>
    //    /// 保存一个子菜单信息   如果对应的父菜单不存在会新建
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <param name="index"></param>
    //    /// <param name="link"></param>
    //    /// <param name="parentName"></param>
    //    public void SaveSubMenu(string name, int index, string link, string parentName)
    //    {
    //        SaveSubMenu(new MenuSubInfo() { Name = name, Index = index, Link = link }, parentName);
    //    }
    //    /// <summary>
    //    /// 保存一个子菜单信息   如果对应的父菜单不存在会新建
    //    /// </summary>
    //    /// <param name="subMenu"></param>
    //    /// <param name="parentName"></param>
    //    public async void SaveSubMenu(MenuSubInfo subMenu, string parentName)
    //    {
    //        if (string.IsNullOrEmpty(subMenu.Link))
    //        {
    //            ApiException.ThrowBadRequest("菜单地址不能为空");
    //        }

    //        var main = GetMainByName(parentName);
    //        if (main == null)
    //        {
    //            main = await CreateMainMenu(parentName);
    //        }

    //        var sub = main.SubMenu.Where(e => e.Link == subMenu.Link).FirstOrDefault();
    //        if (sub == null)
    //        {
    //            main.SubMenu.Add(subMenu);
    //        }
    //        else
    //        {
    //            sub.Name = subMenu.Name;
    //            sub.Index = subMenu.Index;
    //            sub.Link = subMenu.Link;
    //        }
    //        await Update(main, new TokenLogonInfo() { });
    //    }

    //    /// <summary>
    //    /// 在父菜单中删除一个子菜单
    //    /// </summary>
    //    /// <param name="subName"></param>
    //    /// <param name="parentName"></param>
    //    public async void RemoveSubMenu(string subName, string parentName)
    //    {
    //        if (string.IsNullOrEmpty(subName) || string.IsNullOrEmpty(parentName))
    //        {
    //            ApiException.ThrowBadRequest("菜单名称不能为空");
    //        }

    //        var main = GetMainByName(parentName);
    //        if (main == null)
    //        {
    //            return;
    //        }

    //        for (int i = 0; i < main.SubMenu.Count; i++)
    //        {
    //            if (main.SubMenu[i].Name == subName)
    //            {
    //                main.SubMenu.RemoveAt(i);
    //                break;
    //            }
    //        }

    //        await Update(main, new TokenLogonInfo() { });
    //    }

    //    /// <summary>
    //    /// 根据菜单名新增一个主菜单
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    private async Task<MenuMainInfo> CreateMainMenu(string name)
    //    {
    //        MenuMainInfo main = new MenuMainInfo()
    //        {
    //            Name = name,
    //            Index = 10,
    //            Type = DefaultMenuType,
    //            Link = string.Empty,
    //            CreateDate = DateTime.Now,
    //            UpdateDate = DateTime.Now
    //        };
    //        await Create(main, new TokenLogonInfo() { });
    //        return main;
    //    }

    //    /// <summary>
    //    /// 保存一个主菜单信息 （子菜单不变）
    //    /// </summary>
    //    /// <param name="mainInfo"></param>
    //    public void SaveMainMenu(MenuMainInfo mainInfo)
    //    {
    //        if (string.IsNullOrEmpty(mainInfo.Name))
    //        {
    //            ApiException.ThrowBadRequest("菜单名称不能为空");
    //        }

    //        var main = GetMainByName(mainInfo.Name);
    //        if (main != null)
    //        {
    //            main.Index = mainInfo.Index;
    //            main.Link = mainInfo.Link;
    //            main.Icon = mainInfo.Icon;
    //            main.Index = mainInfo.Index;
    //            Dal.UpdateObject(main);
    //        }
    //        else
    //        {
    //            Dal.InsertObject(main);
    //        }
    //    }

    //}
}
