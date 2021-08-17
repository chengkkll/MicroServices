using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.Common;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using TianCheng.Service.Core;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 菜单的消息总线处理程序
    /// </summary>
    public class MenuServiceCap : BaseCapSubscribe
    {
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="updateView"></param>
        [CapSubscribe(MqBus.MenuMq.Update, Group = "tiancheng.org.menu.update.role")]
        public void UpdateRoleMenu(MenuUpdateView updateView)
        {
            string menuId = updateView.NewMenu.Id;
            RoleDAL roleDAL = ServiceLoader.GetService<RoleDAL>();
            foreach (var role in roleDAL.Queryable())
            {
                bool flag = false;
                foreach (var main in role.PagePower)
                {
                    if (main.Id == menuId)
                    {
                        main.Name = updateView.NewMenu.Name;
                        main.Icon = updateView.NewMenu.Icon;
                        main.Link = updateView.NewMenu.Link;
                        main.Index = updateView.NewMenu.Index;
                        flag = true;
                        break;
                    }
                    var sub = main.SubMenu.Where(e => e.Id == menuId).FirstOrDefault();
                    if (sub != null)
                    {
                        sub.Name = updateView.NewMenu.Name;
                        sub.Icon = updateView.NewMenu.Icon;
                        sub.Link = updateView.NewMenu.Link;
                        sub.Index = updateView.NewMenu.Index;
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    roleDAL.UpdateObject(role);
                }
            }
        }
    }
}
