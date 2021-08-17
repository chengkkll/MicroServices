using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 通用的业务服务
    /// </summary>
    /// <typeparam name="DAL"></typeparam>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    /// <typeparam name="QO"></typeparam>
    public class BusinessService<DAL, DO, IdType, QO> : BusinessService<DAL, DO, IdType>, IFilterService<DO, QO>
        where DO : IIdModel<IdType>, new()
        where DAL : IDBOperation<DO, IdType>, IDBOperation<DO>, IDBOperationFilter<DO, QO>
        where QO : QueryObject, new()
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public BusinessService(DAL dal) : base(dal)
        {
        }
        #endregion

        #region 基础数据过滤  Filter
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="controller">如果指定controller将自动保存当前用户的查询条件</param>
        /// <returns></returns>
        public virtual IEnumerable<DO> Filter(QO query, ControllerBase controller = null)
        {
            // 保存当前用户的查询条件
            this.SetSearchQuery(query, controller);

            // 执行查询操作
            return Dal.Search(query);
        }
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="controller">如果指定controller将自动保存当前用户的查询条件</param>
        /// <returns></returns>
        public virtual IEnumerable<View> Filter<View>(QO query, ControllerBase controller = null)
        {
            var result = Filter(query, controller);
            return ObjectTran.Tran<IEnumerable<View>>(result);
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询 获取实体对象
        /// </summary>
        /// <param name="query"></param>
        /// <param name="controller">如果指定controller将自动保存当前用户的查询条件</param>
        /// <returns></returns>
        public PagedResult<DO> FilterPage(QO query, ControllerBase controller = null)
        {
            // 保存当前用户的查询条件
            this.SetSearchQuery(query, controller);
            // 执行查询操作
            return Dal.SearchPages(query);
        }
        /// <summary>
        /// 分页查询 获取实体对象
        /// </summary>
        /// <param name="query"></param>
        /// <param name="controller">如果指定controller将自动保存当前用户的查询条件</param>
        /// <returns></returns>
        public PagedResult<View> FilterPage<View>(QO query, ControllerBase controller = null)
        {
            var result = FilterPage(query, controller);
            return ObjectTran.Tran<PagedResult<View>>(result);
        }
        #endregion

        #region 下拉列表数据查询
        /// <summary>
        /// 获取一个查询列表对象形式的集合
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<SelectView> Select(QO query)
        {
            return Dal.Select(query);
        }
        #endregion

        #region Count
        /// <summary>
        /// 根据条件获取所有的记录条数
        /// </summary>
        /// <returns></returns>
        public int Count(QO query)
        {
            return Dal.Count(query);
        }
        #endregion
    }
}
