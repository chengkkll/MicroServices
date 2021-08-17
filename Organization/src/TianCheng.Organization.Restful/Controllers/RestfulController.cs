using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TianCheng.Common;
using TianCheng.Organization.Services;

namespace TianCheng.Organization.Restful.Controllers
{
    /// <summary>
    /// Restful 接口
    /// </summary>
    [Produces("application/json")]
    [Route("Restful")]
    [ApiController]
    public class RestfulController : ControllerBase
    {
        /// <summary>
        /// 获取当前请求的服务地址
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "系统管理-接口验证" })]
        [HttpGet("")]
        public string Get()
        {
            // 获取当前服务地址和端口
            var features = ServiceLoader.ApplicationBuilder.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            return $"TianCheng.Organization Api Service Address ==> [{address}]";
        }

        /// <summary>
        /// 测试用户角色
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "系统管理-接口验证" })]
        [HttpGet("Role/{id}")]
        public string Get(string id)
        {
            string roleId = id;
            EmployeeService employeeService = ServiceLoader.GetService<EmployeeService>();
            var result = employeeService.IsUseRoleId(roleId);
            int count = employeeService.CountByRoleId(roleId);
            // 获取当前服务地址和端口
            return $"{roleId}==>{count}:{result}";
        }
    }
}
