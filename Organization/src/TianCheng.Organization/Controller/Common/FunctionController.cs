using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Organization.Model;
using TianCheng.Organization.Services;
using TianCheng.Service.Core;

namespace TianCheng.Organization.Controller
{
    /// <summary>
    /// 功能点管理
    /// </summary>
    [Produces("application/json")]
    [Route("Function")]
    public class FunctionController : MongoController<FunctionService>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public FunctionController(FunctionService service) : base(service)
        {
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 查询功能点
        /// </summary>
        /// <remarks>查询所有的功能点信息，以树形结构显示结果，无分页信息</remarks>
        /// <power>查询</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Function.Search")]
        [SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        [HttpGet("")]
        public List<FunctionModuleView> Search()
        {
            return Service.LoadTree();
        }
        #endregion

        /// <summary>
        /// 更新组织结构功能权限
        /// </summary>
        /// <remarks>初始化功能点。   清除已有功能点，分析引用项目的注释信息来重置功能点</remarks>
        /// <power>初始化</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Function.Init")]
        [SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        [HttpPost("Module/Organization")]
        public Task<ResultView> InitOrganization()
        {
            return Service.InitOrganization(this.GetStringTokenInfo());
        }

        /// <summary>
        /// 更新一个模块的权限
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="docs"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        [HttpPost("Module/{code}/{name}")]
        public Task<ResultView> AppendModule(List<IFormFile> assemblies, List<IFormFile> docs, string code, string name)
        {
            return Service.AppendModule(assemblies, docs, code, name, this.GetStringTokenInfo());
        }
    }
}
