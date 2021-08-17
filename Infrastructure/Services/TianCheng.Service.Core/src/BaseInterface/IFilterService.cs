using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 过滤数据的服务接口定义
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="QO"></typeparam>
    public interface IFilterService<DO, QO>
    {
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="controller">如果指定controller将自动保存当前用户的查询条件</param>
        /// <returns></returns>
        public IEnumerable<DO> Filter(QO query, ControllerBase controller = null);
        /// <summary>
        /// 分页查询 获取实体对象
        /// </summary>
        /// <param name="query"></param>
        /// <param name="controller">如果指定controller将自动保存当前用户的查询条件</param>
        /// <returns></returns>
        public PagedResult<DO> FilterPage(QO query, ControllerBase controller = null);

        /// <summary>
        /// 获取一个查询列表对象形式的集合
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<SelectView> Select(QO query);

        /// <summary>
        /// 根据条件获取所有的记录条数
        /// </summary>
        /// <returns></returns>
        public int Count(QO query);
    }
}
