using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using TianCheng.Common;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// MongoDB 持久化操作
    /// </summary>
    public partial class MongoOperation<T, Q> : MongoOperation<T>, IDBOperationFilter<T, Q>
        where Q : QueryObject
        where T : MongoIdModel
    {
        #region 基础数据过滤  Filter
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual IQueryable<T> BaseFilter(Q filter)
        {
            Log.LogTrace("按条件查询（IQueryable<T> BaseFilter<Q>(Q input)）。查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            // 查询条件
            var query = Queryable();
            // 设置排序方式
            //switch (filter.Sort.Property)
            //{
            //    case "date": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
            //    default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
            //}

            //// 查询职责分离模式获取查询条件
            //var mediator = ServiceLoader.GetService<IMediator>();
            //IQueryable<T> query = mediator.Send(filter).Result;
            //if (query == null)
            //{
            //    return Queryable();
            //}


            // 返回查询结果
            return query;
        }
        #endregion

        #region 根据分页/分步信息过滤数据
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="filter">查询条件（包括分页及排序的信息）</param>
        /// <param name="queryResult">所有满足过滤条件的数据</param>
        /// <param name="resultPagination">返回的分页信息</param>
        protected virtual IQueryable<T> SetFilterPagination(Q filter, IQueryable<T> queryResult,
            PagedResultPagination resultPagination)
        {
            Log.LogTrace("设置数据分页效果 （IQueryable<T> SetFilterPagination<Q>(Q filter, IQueryable<T> queryResult,PagedResultPagination resultPagination)）   查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            #region 设置分页信息及返回值
            // 初始化查询条件
            if (filter.Page == null) filter.Page = QueryPagination.DefaultObject;
            if (filter.Page.Size == 0) { filter.Page.Size = QueryPagination.DefaultPageSize; }

            // 设置返回的分页信息
            resultPagination.PageSize = filter.Page.Size;
            resultPagination.TotalRecords = queryResult.Count();
            resultPagination.TotalPage = (int)Math.Ceiling((double)resultPagination.TotalRecords / (double)resultPagination.PageSize);

            // 设置页号
            resultPagination.PageIndex = filter.Page.Index;
            if (resultPagination.PageIndex > resultPagination.TotalPage) { resultPagination.PageIndex = resultPagination.TotalPage; }
            if (resultPagination.PageIndex == 0) { resultPagination.PageIndex = 1; }

            // 返回查询的数据结果
            return queryResult.Skip((resultPagination.PageIndex - 1) * resultPagination.PageSize).Take(resultPagination.PageSize);
            #endregion
        }
        #endregion

        #region 分步查询
        /// <summary>
        /// 分步查询数据，返回实体对象信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> FilterStep(Q filter)
        {
            Log.LogTrace("设置分步查询数据 （IEnumerable<T> FilterStep(Q input)）   查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            var queryResult = BaseFilter(filter);
            if (filter.Page == null) filter.Page = QueryPagination.DefaultObject;
            if (filter.Page.Index <= 0) filter.Page.Index = 1;
            if (filter.Page.Size <= 0) filter.Page.Size = QueryPagination.DefaultPageSize;

            PagedResultPagination resultPage = new PagedResultPagination();
            queryResult = SetFilterPagination(filter, queryResult, resultPage);
            if (resultPage.TotalPage < filter.Page.Index)  // 如果查询到最后一页，返回空列表
            {
                return new List<T>();
            }

            return queryResult.ToList();
        }
        #endregion

        #region Search
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<T> Search(Q filter)
        {
            Log.LogTrace("按条件查询 （IEnumerable<T> Search<Q>(Q filter)）   查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            return BaseFilter(filter).ToList();
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询 获取实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public PagedResult<T> SearchPages(Q filter)
        {
            Log.LogTrace("分页查询 （PagedResult<T> FilterPage<Q>(Q filter)）   查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            var queryResult = BaseFilter(filter);
            if (filter.Page == null) filter.Page = QueryPagination.DefaultObject;
            if (filter.Page.Index <= 0) filter.Page.Index = 0;
            if (filter.Page.Size <= 0) filter.Page.Size = QueryPagination.DefaultPageSize;

            PagedResultPagination pagination = new PagedResultPagination();
            queryResult = SetFilterPagination(filter, queryResult, pagination);
            var infoList = queryResult.ToList();
            return new PagedResult<T>(infoList, pagination);
        }
        /// <summary>
        /// 分页查询 获取指定对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public PagedResult<View> SearchPages<View>(Q filter)
        {
            Log.LogTrace("分页查询 （PagedResult<View> FilterPage<Q, View>(Q filter)）   查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            var queryResult = BaseFilter(filter);
            PagedResultPagination pagination = new PagedResultPagination();
            queryResult = SetFilterPagination(filter, queryResult, pagination);
            var infoList = ObjectTran.Tran<IEnumerable<View>>(queryResult.ToList());
            return new PagedResult<View>(infoList, pagination);
        }
        #endregion

        #region 下拉列表数据查询
        /// <summary>
        /// 获取一个查询列表对象形式的集合
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<SelectView> Select(Q filter)
        {
            Log.LogTrace("下拉列表查询 （IEnumerable<SelectView> Select<Q>(Q filter)）   查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            return ObjectTran.Tran<IEnumerable<SelectView>>(BaseFilter(filter));
        }

        /// <summary>
        /// 获取满足条件的前几条数据    input.Page.Size 为指定的返回记录的最大条数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<SelectView> SelectTop(Q filter)
        {
            Log.LogTrace("下拉列表查询 （IEnumerable<SelectView> SelectTop<Q>(Q filter)）   查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);

            var queryResult = BaseFilter(filter);
            PagedResultPagination pagination = new PagedResultPagination();
            filter.Page.Index = 1;
            if (filter.Page.Size <= 0) filter.Page.Size = 10;
            queryResult = SetFilterPagination(filter, queryResult, pagination);

            var infoList = queryResult.ToList();
            return ObjectTran.Tran<IEnumerable<SelectView>>(infoList);
        }
        #endregion

        #region Count
        /// <summary>
        /// 根据条件获取记录数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int Count(Q filter)
        {
            Log.LogTrace("按条件查询（int Count<Q>(Q filter)）。查询对象类型为：{EntityType}  查询条件类型为：{QueryType}   查询条件为：{QueryInfo}", typeof(T).FullName, typeof(Q).FullName, filter);
            return BaseFilter(filter).Count();
        }
        #endregion
    }
}
