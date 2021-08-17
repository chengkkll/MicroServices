using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// 通用的返回值说明
    /// </summary>
    public class AuthorizeResponseDescription : IOperationFilter
    {
        private static readonly OpenApiResponse R401 = new OpenApiResponse() { Description = "Token值错误，需要重新登陆。" };
        private static readonly OpenApiResponse R403 = new OpenApiResponse() { Description = "没有权限调用本接口。" };
        /// <summary>
        /// 每个api方法均会调用一次
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 针对使用AuthorizeAttribute特性的接口添加返回值注释
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Where(e => !string.IsNullOrWhiteSpace(e.Policy))
                .Select(attr => attr.Policy)
                .Distinct();
            if (requiredScopes.Count() == 0)
            {
                requiredScopes = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<TianCheng.Services.AuthJwt.AuthActionAttribute>()
                    .Select(attr => attr.ActionCode)
                    .Distinct();
            }

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", R401);
                operation.Responses.Add("403", R403);

                var oAuthScheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = requiredScopes.ToList()
                    }
                };

                if (!string.IsNullOrEmpty(operation.Description))
                {
                    operation.Description += "\r\n";
                }
                operation.Description += $"<p>权限说明： {requiredScopes.FirstOrDefault()} </p><pre><code>终端可根据权限名来判断当前登录用户是否可调用本接口\r\n权限名： {requiredScopes.FirstOrDefault()}\r\n获取当前用户的权限信息接口为：employee/power</code></pre>";
            }
        }
    }
}
