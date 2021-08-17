using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL
{
    /// <summary>
    /// 按条件查询的接口
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public interface IDBOperationFilter<DO, Q>
        where Q : QueryObject
    {
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<DO> Search(Q query);
        /// <summary>
        /// 根据查询条件获取分页结果
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PagedResult<DO> SearchPages(Q query);

        /// <summary>
        /// 获取下拉列表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<SelectView> Select(Q query);

        /// <summary>
        /// 查询满足条件的记录数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int Count(Q query);
    }
}
