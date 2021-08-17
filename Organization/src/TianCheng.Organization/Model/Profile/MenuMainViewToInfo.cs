using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    ///// <summary>
    ///// MenuMainView => MenuMainInfo
    ///// </summary>
    //public class MenuMainViewToInfo : ITypeConverter<MenuMainView, MenuMainInfo>
    //{
    //    /// <summary>
    //    /// EmployeeInfo => SelectView
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <param name="destination"></param>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    public MenuMainInfo Convert(MenuMainView source, MenuMainInfo destination, ResolutionContext context)
    //    {
    //        return new MenuMainInfo
    //        {
    //            Id = MongoIdModel.ConvertId(source.Id),
    //            Name = source.Name,
    //            Index = source.Index,
    //            Title = source.Title,
    //            Icon = source.Icon,
    //            Link = source.Link,
    //            SubMenu = ObjectTran.Tran<List<MenuSubInfo>>(source.SubMenu)
    //        };
    //    }
    //}
}
