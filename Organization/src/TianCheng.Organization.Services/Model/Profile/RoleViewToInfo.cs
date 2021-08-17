using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// RoleView => RoleInfo
    /// </summary>
    public class RoleViewToInfo : ITypeConverter<RoleView, RoleInfo>
    {
        /// <summary>
        /// EmployeeInfo => SelectView
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public RoleInfo Convert(RoleView source, RoleInfo destination, ResolutionContext context)
        {
            return new RoleInfo
            {
                Id = MongoIdModel.ConvertId(source.Id),
                Name = source.Name,
                Desc = source.Desc,
                DefaultPage = source.DefaultPage,
                PagePower = source.PagePower,
                FunctionPower = source.FunctionPower,
                IsSystem = source.IsSystem
            };
        }
    }
}
