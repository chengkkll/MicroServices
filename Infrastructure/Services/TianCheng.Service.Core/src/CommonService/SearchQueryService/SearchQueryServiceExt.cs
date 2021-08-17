using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 用户查询条件的调用
    /// </summary>
    static public class SearchQueryServiceExt
    {
        #region 保存用户的当前查询条件
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="employeeId"></param>
        public static void SetSearchQuery<DO, QO>(this IFilterService<DO, QO> service, QO query, string employeeId)
            where QO : QueryObject
        {
            ServiceLoader.GetService<ISearchQueryService>()?.Set<DO, QO>(query, service.GetType().Name, employeeId);
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="employeeId"></param>
        public static void SetSearchQuery<DO, QO>(this IFilterService<DO, QO> service, QO query, TokenBase token)
            where QO : QueryObject
        {
            if (token == null)
            {
                return;
            }
            ServiceLoader.GetService<ISearchQueryService>()?.Set<DO, QO>(query, service.GetType().Name, token.GetEmployeeStringId);
        }
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <param name="showItem"></param>
        /// <param name="controller"></param>
        public static void SetSearchQuery<DO, QO>(this IFilterService<DO, QO> service, QO query, ControllerBase controller)
            where QO : QueryObject
        {
            if (controller == null)
            {
                return;
            }
            ServiceLoader.GetService<ISearchQueryService>()?.Set<DO, QO>(query, service.GetType().Name, controller.GetEmployeeStringId());
        }
        #endregion

        #region 获取用户的上一次查询
        /// <summary>
        /// 获取用户的上一次查询
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="service"></param>
        /// <param name="employeeId"></param>
        public static string GetSearchQuery<DO, QO>(this IFilterService<DO, QO> service, string employeeId)
           where QO : QueryObject
        {
            return ServiceLoader.GetService<ISearchQueryService>()?.Get<DO, QO>(service.GetType().Name, employeeId);
        }
        /// <summary>
        /// 获取用户的上一次查询条件
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="service"></param>
        /// <param name="controller"></param>
        public static string GetSearchQuery<DO, QO>(this IFilterService<DO, QO> service, ControllerBase controller)
            where QO : QueryObject
        {
            return ServiceLoader.GetService<ISearchQueryService>()?.Get<DO, QO>(service.GetType().Name, controller.GetEmployeeStringId());
        }
        #endregion
    }
}
