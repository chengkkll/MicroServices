using TianCheng.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using TianCheng.Common;

namespace TianCheng.DAL.MySqlByEF
{
    public partial class MysqlOperation<C, T, Q, IdType> :
        IMysqlOperation<T, IdType>,
        IMysqlOperationFilter<T, Q>,
        IDBOperationRegister,
        IDBOperationFilter<T, Q>
        where C : MysqlContext, new()
        where Q : QueryObject
        where T : class, IIdModel<IdType>, new()
    {
        #region 基础数据过滤  Filter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual IQueryable GetTable(string typeName)
        {
            var type = Type.GetType(typeName);
            var method = this.GetType().GetMethod("Set", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            return method.MakeGenericMethod(type) as IQueryable;
        }
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual IQueryable<T> BaseFilter(Q filter)
        {
            // 查询条件
            var query = this.Context.Set<T>();

            // 示例：
            //// 设置排序方式
            //switch (filter.Sort.Property)
            //{
            //    case "date": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
            //    default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
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

        #region 按Id查询
        /// <summary>
        /// 根据Id获取对象实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T SingleById(IdType id)
        {
            return Context.Set<T>().FirstOrDefault(e => e.Id.Equals(id));
        }
        #endregion

        #region Search
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> Search()
        {
            try
            {
                return Context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "获取所有数据时错误。类型：[{TypeName}] ", typeof(T).FullName);
                throw;
            }
        }
        /// <summary>
        /// 获取当前对象的查询链式接口
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Queryable()
        {
            return Context.Set<T>();
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="filterFactory"></param>
        /// <returns></returns>
        public List<T> Search(Expression<Func<T, bool>> filterFactory)
        {
            try
            {
                return Context.Set<T>().Where(filterFactory).ToList();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "根据表达式树查询数据时错误。类型：[{TypeName}]\r\n查询条件：[{@Change}] ", typeof(T).FullName, filterFactory);
                throw;
            }
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<T> Search(Q filter)
        {
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
            var queryResult = BaseFilter(filter);
            PagedResultPagination pagination = new PagedResultPagination();
            queryResult = SetFilterPagination(filter, queryResult, pagination);
            var infoList = ObjectTran.Tran<IEnumerable<View>>(queryResult.ToList());
            return new PagedResult<View>(infoList, pagination);
        }
        #endregion

        #region 下拉列表数据查询
        /// <summary>
        /// 获取满足条件的前几条数据    input.Page.Size 为指定的返回记录的最大条数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<SelectView> Select(Q filter)
        {
            return Select<SelectView>(filter);
        }
        /// <summary>
        /// 获取满足条件的前几条数据    input.Page.Size 为指定的返回记录的最大条数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="size"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<SelectView> Select(Q filter, int size = 10, Expression<Func<T, SelectView>> selector = null)
        {
            return Select<SelectView>(filter, size, selector);
        }
        /// <summary>
        /// 获取满足条件的前几条数据    input.Page.Size 为指定的返回记录的最大条数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="size"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<S> Select<S>(Q filter, int size = 10, Expression<Func<T, S>> selector = null) where S : new()
        {
            var query = BaseFilter(filter).Take(size);
            if (selector != null)
            {
                return query.Select(selector).ToList();
            }
            return ObjectTran.Tran<IEnumerable<S>>(query);
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
            return BaseFilter(filter).Count();
        }
        /// <summary>
        /// 无条件查询记录数量
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return Context.Set<T>().LongCount();
        }
        #endregion

        #region 其它工具方法
        /// <summary>
        /// 将所有数据转成字典数据
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Dictionary<string, T> DataDict(Func<T, string> action)
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            foreach (var item in Search())
            {
                string key = action.Invoke(item);
                if (dict.ContainsKey(key))
                {
                    continue;
                }
                dict.Add(key, item);
            }
            return dict;
        }
        #endregion
    }
}