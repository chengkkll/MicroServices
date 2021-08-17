using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// 请求时对View对象进行字段过滤处理（仅过滤显示）
    /// </summary>
    public class ViewRequestFilter : IOperationFilter
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

            // 1、如果方法/参数中使用了CreateViewAttribute说明需要进行设置
            if (context.MethodInfo.GetCustomAttributes<CreateViewAttribute>(true).Count() > 0 ||
                context.MethodInfo.GetParameters().Any(p => p.GetCustomAttributes<CreateViewAttribute>().Count() > 0))
            {
                // 设置应用的对象名称
                string key = operation.RequestBody.Content.FirstOrDefault().Value.Schema.Reference.Id;
                string result = operation.RequestBody.Content.FirstOrDefault().Value.Schema.Reference.Id + "CreateView";
                foreach (var item in operation.RequestBody.Content)
                {
                    item.Value.Schema.Reference.Id = result;
                }
                // 记录要修改的对象信息
                CreateViewAttribute.Append(key, result);
            }


            // 2、如果方法/参数中使用了UpdateViewAttribute说明需要进行设置
            if (context.MethodInfo.GetCustomAttributes<UpdateViewAttribute>(true).Count() > 0 ||
                context.MethodInfo.GetParameters().Any(p => p.GetCustomAttributes<UpdateViewAttribute>().Count() > 0))
            {
                // 设置应用的对象名称
                string key = operation.RequestBody.Content.FirstOrDefault().Value.Schema.Reference.Id;
                string result = operation.RequestBody.Content.FirstOrDefault().Value.Schema.Reference.Id + "UpdateView";
                foreach (var item in operation.RequestBody.Content)
                {
                    item.Value.Schema.Reference.Id = result;
                }
                // 记录要修改的对象信息
                UpdateViewAttribute.Append(key, result);
            }
        }
    }
}
