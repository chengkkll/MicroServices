using AutoMapper;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// DepartmentView => DepartmentInfo
    /// </summary>
    public class DepartmentViewToInfo : ITypeConverter<DepartmentView, DepartmentInfo>
    {
        /// <summary>
        /// EmployeeInfo => SelectView
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public DepartmentInfo Convert(DepartmentView source, DepartmentInfo destination, ResolutionContext context)
        {
            return new DepartmentInfo
            {
                Id = MongoIdModel.ConvertId(source.Id),
                Index = source.Index,
                Name = source.Name,
                Desc = source.Desc,
                Code = source.Code,
                ParentId = source.ParentId,
                ParentName = source.ParentName,
                ManageId = source.ManageId,
                ManageName = source.ManageName
            };
        }
    }


}
