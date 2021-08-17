using AutoMapper;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// EmployeeInfo => SelectView
    /// </summary>
    public class EmployeeInfoToSelectView : ITypeConverter<EmployeeInfo, SelectView>
    {
        /// <summary>
        /// EmployeeInfo => SelectView
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SelectView Convert(EmployeeInfo source, SelectView destination, ResolutionContext context)
        {
            return new SelectView
            {
                Id = source.Id.ToString(),
                Name = source.Name,
                Code = source.Department.Id
            };
        }
    }
}
