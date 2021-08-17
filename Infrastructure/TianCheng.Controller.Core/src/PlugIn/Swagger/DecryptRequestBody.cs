using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// 加密请求体的处理
    /// </summary>
    public class DecryptRequestBody : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var attrList = context.MethodInfo.GetCustomAttributes(typeof(DecryptBodyAttribute), true);
            if (attrList != null && attrList.Count() > 0)
            {
                if (!(attrList.FirstOrDefault() is DecryptBodyAttribute))
                {
                    return;
                }
                //foreach (var para in operation.Parameters)
                //{

                //}

                //// 设置加密接口参数
                //operation.Parameters.Clear();
                //operation.Parameters.Add(new OpenApiParameter()
                //{
                //    Name = "加密后数据",
                //    In = ParameterLocation.Header,
                //    Description = "加密后的数据",
                //    Required = true,
                //    Schema = new OpenApiSchema { Type = "string" }
                //});

                operation.RequestBody.Description = "加密后的数据";
                operation.RequestBody.Required = true;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DecryptBodyAttribute : Attribute
    {

    }
}
