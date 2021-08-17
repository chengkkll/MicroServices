using MongoDB.Driver;
using System;
using System.Linq;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;
using TianCheng.Organization.Model;
using MediatR;
using TianCheng.Common;
using System.Threading.Tasks;
using System.Threading;

namespace TianCheng.Organization.DAL
{
    /// <summary>
    /// 部门信息 [数据持久化]
    /// </summary>
    [DBMapping("system_department")]
    public class DepartmentDAL : MongoOperation<DepartmentInfo, DepartmentQuery>
    {
        #region 查询方法
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected override IQueryable<DepartmentInfo> BaseFilter(DepartmentQuery filter)
        {
            var query = Queryable();

            #region 查询条件
            //不显示删除的数据
            query = query.Where(e => e.IsDelete == false);

            // 按名称的模糊查询
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(e => !string.IsNullOrEmpty(e.Name) && e.Name.Contains(filter.Name));
            }
            // 按上级部门ID查询
            if (!string.IsNullOrWhiteSpace(filter.ParentId))
            {
                query = query.Where(e => e.ParentsIds.Contains(filter.ParentId));
            }
            // 只查询根部门
            if (filter.OnlyRoot == true)
            {
                query = query.Where(e => string.IsNullOrEmpty(e.ParentId));
            }
            #endregion

            #region 设置排序规则
            //设置排序方式
            switch (filter.Sort.Property)
            {
                case "name": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name); break; }
                case "code": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.Code) : query.OrderByDescending(e => e.Code); break; }
                case "parent": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.ParentName) : query.OrderByDescending(e => e.ParentName); break; }
                case "index": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.Index) : query.OrderByDescending(e => e.Index); break; }
                case "date": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
                default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }
        #endregion

        ///// <summary>
        ///// 获取指定部门及其下级部门
        ///// </summary>
        ///// <param name="id">部门ID</param>
        //public List<DepartmentInfo> GetSelfAndSub(string id)
        //{
        //    var builder = Builders<DepartmentInfo>.Filter;
        //    FilterDefinition<DepartmentInfo> filter = builder.Eq("ParentsIds", id) | builder.Eq("_id", new ObjectId(id));

        //    try
        //    {
        //        return _mongoCollection.Find(filter).ToList();
        //    }
        //    catch (System.TimeoutException te)
        //    {
        //        DBLog.Logger.LogWarning(te, "数据库链接超时。链接字符串：" + _options.ConnectionOptions.ConnectionString());
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLog.Logger.LogWarning(ex, "操作异常终止。");
        //        throw;
        //    }
        //}

        /// <summary>
        /// 批量更新上级部门名称
        /// </summary>
        /// <param name="department"></param>
        public void UpdateParentsDepartmentName(DepartmentView department)
        {
            // 验证参数
            if (department == null || string.IsNullOrWhiteSpace(department.Id))
            {
                return;
            }
            // 设置查询条件
            FilterDefinition<DepartmentInfo> filter = Builders<DepartmentInfo>.Filter.Eq("ParentId", department.Id);
            // 设置更新内容
            UpdateDefinition<DepartmentInfo> ud = Builders<DepartmentInfo>.Update.Set("ParentName", department.Name);
            // 执行数据的持久化操作
            UpdateProperties(filter, ud, out _);
        }
    }
}
