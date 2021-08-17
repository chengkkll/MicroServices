using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgSqlByEF
{
    /// <summary>
    /// NpgSql数据库条件查询接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public interface INpgSqlOperationFilter<T, Q>
        where T : class
        where Q : QueryObject
    {
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<T> Search(Q filter);

        /// <summary>
        /// 分页查询 获取实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        PagedResult<T> SearchPages(Q filter);
        /// <summary>
        /// 分页查询 获取指定对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        PagedResult<View> SearchPages<View>(Q filter);

        /// <summary>
        /// 获取满足条件的前几条数据    input.Page.Size 为指定的返回记录的最大条数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="size"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        IEnumerable<SelectView> Select(Q filter, int size = 10, Expression<Func<T, SelectView>> selector = null);
        /// <summary>
        /// 获取满足条件的前几条数据    input.Page.Size 为指定的返回记录的最大条数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="size"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        IEnumerable<S> Select<S>(Q filter, int size = 10, Expression<Func<T, S>> selector = null) where S : new();
        /// <summary>
        /// 根据条件获取记录数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        int Count(Q filter);
    }
}
