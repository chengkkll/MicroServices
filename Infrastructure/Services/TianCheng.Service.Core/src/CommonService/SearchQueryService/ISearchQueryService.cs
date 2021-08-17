using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 用户查询条件接口
    /// </summary>
    public interface ISearchQueryService
    {
        /// <summary>
        /// 设置用户查询条件
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="type">类型</param>
        /// <param name="employeeId">用户Id</param>
        public void Set<DO, QO>(QO query, string type, string employeeId) where QO : QueryObject;

        /// <summary>
        /// 获取用户的查询条件
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="type">类型</param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public string Get<DO, QO>(string type, string employeeId) where QO : QueryObject;
    }
}
