using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Linq;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;
using TianCheng.Organization.Model;

namespace TianCheng.Organization.DAL
{
    /// <summary>
    /// 登录记录 [数据持久化]
    /// </summary>
    [DBMapping("System_RoleInfo")]
    public class LoginHistoryDAL : MongoOperation<LoginHistoryInfo, LoginHistoryQuery>
    {
        #region 查询
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<LoginHistoryInfo> BaseFilter(LoginHistoryQuery input)
        {
            var query = Queryable();

            #region 查询条件
            // 根据查询的关键字查询
            if (!string.IsNullOrWhiteSpace(input.Key))
            {
                query = query.Where(e => (!string.IsNullOrEmpty(e.Name) && e.Name.Contains(input.Key)) ||
                                         (!string.IsNullOrEmpty(e.DepartmentName) && e.DepartmentName.Contains(input.Key)) ||
                                         (!string.IsNullOrEmpty(e.IpAddressCityName) && e.IpAddressCityName.Contains(input.Key)) ||
                                         (!string.IsNullOrEmpty(e.IpAddress) && e.IpAddress.Contains(input.Key)));
            }
            #endregion

            #region 设置排序规则
            switch (input.Sort.Property)
            {
                case "name": { query = input.Sort.IsAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name); break; }
                case "departmentName": { query = input.Sort.IsAsc ? query.OrderBy(e => e.DepartmentName) : query.OrderByDescending(e => e.DepartmentName); break; }
                case "ipAddress": { query = input.Sort.IsAsc ? query.OrderBy(e => e.IpAddress) : query.OrderByDescending(e => e.IpAddress); break; }
                case "date": { query = input.Sort.IsAsc ? query.OrderBy(e => e.CreateDate) : query.OrderByDescending(e => e.CreateDate); break; }
                default: { query = query.OrderByDescending(e => e.CreateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }
        #endregion
    }
}
