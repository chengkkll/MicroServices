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
    /// 员工信息 [数据持久化]
    /// </summary>
    [DBMapping("system_employee")]
    public class EmployeeDAL : MongoOperation<EmployeeInfo, EmployeeQuery>
    {

        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<EmployeeInfo> BaseFilter(EmployeeQuery input)
        {
            var query = Queryable();
            //_logger.LogInformation("this is a log test: input.code:{0}, input.key:{1}", input.Name, input.State);
            // var query = _Dal.Queryable();

            #region 查询条件
            // 逻辑删除的过滤 0-不显示逻辑删除的数据 1-显示所有数据，包含逻辑删除的   2-只显示逻辑删除的数据
            if (input.HasDelete == 0)
            {
                query = query.Where(e => e.IsDelete == false);
            }
            if (input.HasDelete == 2)
            {
                query = query.Where(e => e.IsDelete);
            }
            // 根据查询的关键字查询
            if (!string.IsNullOrWhiteSpace(input.Key))
            {
                query = query.Where(e => (!string.IsNullOrEmpty(e.Name) && e.Name.Contains(input.Key)) ||
                                         (e.Role != null && !string.IsNullOrEmpty(e.Role.Name) && e.Role.Name.Contains(input.Key)) ||
                                         (e.Department != null && !string.IsNullOrEmpty(e.Department.Name) && e.Department.Name.Contains(input.Key)) ||
                                         (!string.IsNullOrEmpty(e.LogonAccount) && e.LogonAccount.Contains(input.Key)));
            }

            // 按名称的模糊查询
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                query = query.Where(e => !string.IsNullOrEmpty(e.Name) && e.Name.Contains(input.Name));
            }
            // 按部门ID查询
            if (!string.IsNullOrWhiteSpace(input.DepartmentId))
            {
                query = query.Where(e => !string.IsNullOrEmpty(e.Department.Id) && e.Department.Id == input.DepartmentId);
            }
            //根据根节点查询部门
            if (!string.IsNullOrWhiteSpace(input.RootDepartment))
            {
                query = query.Where(e => e.ParentDepartment != null && e.ParentDepartment.Ids != null && e.ParentDepartment.Ids.Contains(input.RootDepartment));
            }
            // 按角色ID查询
            if (!string.IsNullOrWhiteSpace(input.RoleId))
            {
                query = query.Where(e => e.Role != null && e.Role.Id == input.RoleId);
            }

            //按状态查询  1-可用，3-锁住，5-禁用
            switch (input.State)
            {
                case UserState.Enable: { query = query.Where(e => e.State == UserState.Enable); break; }
                case UserState.LogonLock: { query = query.Where(e => e.State == UserState.LogonLock); break; }
                case UserState.Disable: { query = query.Where(e => e.State == UserState.Disable); break; }
            }
            #endregion

            #region 设置排序规则
            switch (input.Sort.Property)
            {
                case "name": { query = input.Sort.IsAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name); break; }
                case "department.name": { query = input.Sort.IsAsc ? query.OrderBy(e => e.Department.Name) : query.OrderByDescending(e => e.Department.Name); break; }
                case "role.name": { query = input.Sort.IsAsc ? query.OrderBy(e => e.Role.Name) : query.OrderByDescending(e => e.Role.Name); break; }
                case "state": { query = input.Sort.IsAsc ? query.OrderBy(e => e.State) : query.OrderByDescending(e => e.State); break; }
                case "updateDate": { query = input.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
                default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }

        /// <summary>
        /// 批量更新部门名称
        /// </summary>
        /// <param name="departmentInfo"></param>
        public void UpdateDepartmentInfo(DepartmentView departmentInfo)
        {
            // 验证参数
            if (departmentInfo == null || string.IsNullOrWhiteSpace(departmentInfo.Id))
            {
                return;
            }
            // 设置查询条件
            FilterDefinition<EmployeeInfo> filter = Builders<EmployeeInfo>.Filter.Eq("Department.Id", departmentInfo.Id);
            // 设置更新内容
            UpdateDefinition<EmployeeInfo> ud = Builders<EmployeeInfo>.Update.Set("Department.Name", departmentInfo.Name)
                                                                             .Set("ParentDepartment.Id", departmentInfo.ParentId)
                                                                             .Set("ParentDepartment.Name", departmentInfo.ParentName)
                                                                             .Set("ParentDepartment.Ids", departmentInfo.ParentsIds);
            // 执行数据的持久化操作
            UpdateProperties(filter, ud, out _);
        }
        /// <summary>
        /// 批量更新角色名称
        /// </summary>
        /// <param name="roleInfo"></param>
        public void UpdateRoleInfo(string roleId, string roleName)
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return;
            }
            // 设置查询条件
            FilterDefinition<EmployeeInfo> filter = Builders<EmployeeInfo>.Filter.Eq("Role.Id", roleId);
            // 设置更新内容
            UpdateDefinition<EmployeeInfo> ud = Builders<EmployeeInfo>.Update.Set("Role.Name", roleName);
            // 执行数据的持久化操作
            UpdateProperties(filter, ud, out _);
        }
    }
}
