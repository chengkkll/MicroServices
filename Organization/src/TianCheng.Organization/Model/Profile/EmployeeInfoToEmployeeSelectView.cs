using AutoMapper;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// EmployeeInfo => EmployeeSelectView
    /// </summary>
    public class EmployeeInfoToEmployeeSelectView : ITypeConverter<EmployeeInfo, EmployeeSelectView>
    {
        /// <summary>
        /// EmployeeInfo => SelectView
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public EmployeeSelectView Convert(EmployeeInfo source, EmployeeSelectView destination, ResolutionContext context)
        {
            return new EmployeeSelectView
            {
                Id = source.Id.ToString(),
                Name = source.Name,
                Code = source.Code,
                DepartmentId = source.Department.Id,
                DepartmentName = source.Department.Name,
                RoleId = source.Role.Id,
                RoleName = source.Role.Name,
                IsDelete = source.IsDelete
            };
        }
    }
}
