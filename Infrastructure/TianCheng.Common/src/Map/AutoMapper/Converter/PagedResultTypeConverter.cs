using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// PagedResult 返回结果类型转换
    /// </summary>
    /// <typeparam name="T">转换前对象类型</typeparam>
    /// <typeparam name="O">转换后对象类型</typeparam>
    public class PagedResultTypeConverter<T, O> : ITypeConverter<PagedResult<T>, PagedResult<O>>
    {
        /// <summary>
        ///  ObjectId -> String
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public PagedResult<O> Convert(PagedResult<T> source, PagedResult<O> destination, ResolutionContext context)
        {
            return new PagedResult<O>(ObjectTran.Tran<IEnumerable<O>>(source.Data), source.Pagination);
        }
    }
}
