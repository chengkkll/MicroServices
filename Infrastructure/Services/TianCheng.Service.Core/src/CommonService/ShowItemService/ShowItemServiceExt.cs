using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 显示项的扩展服务
    /// </summary>
    static public class ShowItemServiceExt
    {
        #region Get
        /// <summary>
        /// 获取显示项Json
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="logonInfo">登录人信息</param>
        public static string GetShowItem<DO, QO>(this IFilterService<DO, QO> service, TokenBase logonInfo)
            where DO : new()
            where QO : QueryObject, new()
        {
            return ServiceLoader.GetService<IShowItemService>()?.Get<DO, QO>(service.GetType().Name, logonInfo.GetEmployeeStringId);
        }
        /// <summary>
        /// 获取显示项Json
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="controller">登录人信息</param>
        public static string GetShowItem<DO, QO>(this IFilterService<DO, QO> service, ControllerBase controller)
            where DO : new()
            where QO : QueryObject, new()
        {
            return ServiceLoader.GetService<IShowItemService>()?.Get<DO, QO>(service.GetType().Name, controller.GetEmployeeStringId());
        }
        /// <summary>
        /// 获取显示项Json
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="logonInfo">登录人信息</param>
        public static string GetShowItem<V, QO>(this IBusinessService service, TokenBase logonInfo)
            where V : new()
            where QO : QueryObject, new()
        {
            return ServiceLoader.GetService<IShowItemService>()?.Get<V, QO>(service.GetType().Name, logonInfo.GetEmployeeStringId);
        }
        /// <summary>
        /// 获取显示项Json
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="controller">登录人信息</param>
        public static string GetShowItem<V, QO>(this IBusinessService service, ControllerBase controller)
            where V : new()
            where QO : QueryObject, new()
        {
            return ServiceLoader.GetService<IShowItemService>()?.Get<V, QO>(service.GetType().Name, controller.GetEmployeeStringId());
        }
        #endregion

        #region Set
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="employeeId"></param>
        public static void SetShowItem<DO, QO>(this IFilterService<DO, QO> service, ShowItem<DO, QO> showItem, string employeeId)
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.Set<DO, QO>(showItem, service.GetType().Name, employeeId);
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="employeeId"></param>
        public static void SetShowItem<DO, QO>(this IFilterService<DO, QO> service, ShowItem<DO, QO> showItem, TokenBase logonInfo)
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.Set<DO, QO>(showItem, service.GetType().Name, logonInfo.GetEmployeeStringId);
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="controller"></param>
        public static void SetShowItem<DO, QO>(this IFilterService<DO, QO> service, ShowItem<DO, QO> showItem, ControllerBase controller)
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.Set<DO, QO>(showItem, service.GetType().Name, controller.GetEmployeeStringId());
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="showItem"></param>
        /// <param name="employeeId"></param>
        public static void SetShowItem<V, QO>(this IBusinessService service, ShowItem<V, QO> showItem, string employeeId)
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.Set<V, QO>(showItem, service.GetType().Name, employeeId);
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="showItem"></param>
        /// <param name="controller"></param>
        public static void SetShowItem<V, QO>(this IBusinessService service, ShowItem<V, QO> showItem, ControllerBase controller)
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.Set<V, QO>(showItem, service.GetType().Name, controller.GetEmployeeStringId());
        }
        #endregion

        #region Set Employee Default
        /// <summary>
        /// 获取显示项Json
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="employeeId">登录用户</param>
        public static void SetShowItemEmployeeDefault<DO, QO>(this IBusinessService service, string employeeId)
            where DO : new()
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.SetEmployeeDefault<DO, QO>(service.GetType().Name, employeeId);
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="showItem"></param>
        /// <param name="controller"></param>
        public static void SetShowItemEmployeeDefault<V, QO>(this IBusinessService service, ControllerBase controller)
            where V : new()
            where QO : QueryObject, new()
        {
            service.SetShowItemEmployeeDefault<V, QO>(controller.GetEmployeeStringId());
        }
        #endregion

        #region Get Default
        /// <summary>
        /// 获取显示项Json
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        public static string GetDefaultShowItem<DO, QO>(this IFilterService<DO, QO> service)
            where DO : new()
            where QO : QueryObject, new()
        {
            return ServiceLoader.GetService<IShowItemService>()?.GetDefault<DO, QO>(service.GetType().Name);
        }
        /// <summary>
        /// 获取显示项Json
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        public static string GetDefaultShowItem<V, QO>(this IBusinessService service)
            where V : new()
            where QO : QueryObject, new()
        {
            return ServiceLoader.GetService<IShowItemService>()?.GetDefault<V, QO>(service.GetType().Name);
        }
        #endregion

        #region Set Default
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="employeeId"></param>
        public static void SetDefaultShowItem<DO, QO>(this IFilterService<DO, QO> service, ShowItem<DO, QO> showItem)
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.SetDefault<DO, QO>(showItem, service.GetType().Name);
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="employeeId"></param>
        public static void SetDefaultShowItem<V, QO>(this IBusinessService service, ShowItem<V, QO> showItem)
            where QO : QueryObject, new()
        {
            ServiceLoader.GetService<IShowItemService>()?.SetDefault<V, QO>(showItem, service.GetType().Name);
        }
        #endregion
    }
}
