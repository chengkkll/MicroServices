using System.Collections.Generic;
using System.Linq;
using TianCheng.Common;
using TianCheng.DAL.MongoDB;
using TianCheng.Organization.Model;

namespace TianCheng.Organization.DAL
{
    ///// <summary>
    ///// 菜单信息 [数据持久化]
    ///// </summary>
    //[DBMapping("system_menu")]
    //public class MenuMainDAL : MongoOperation<MenuMainInfo, MenuQuery>
    //{
    //    #region 查询
    //    /// <summary>
    //    /// 获取主菜单的树形结构
    //    /// </summary>
    //    /// <param name="menuType"></param>
    //    /// <returns></returns>
    //    protected List<MenuMainView> SearchMainTree(MenuType menuType = MenuType.None)
    //    {
    //        var query = Queryable();
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
    //    #endregion
    //}
}
