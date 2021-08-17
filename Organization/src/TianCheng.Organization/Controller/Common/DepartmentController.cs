using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Service.Core;
using TianCheng.Organization.Model;
using TianCheng.Organization.Services;

namespace TianCheng.Organization.Controller
{
    /// <summary>
    /// 组织机构管理
    /// </summary>
    [Produces("application/json")]
    [Route("Department")]
    public class DepartmentController : MongoController<DepartmentService>
    {
        #region 构造方法
        /// <summary> 
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public DepartmentController(DepartmentService service) : base(service)
        {
        }
        #endregion

        #region 新增修改数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="view">请求体中放置新增部门的信息</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Create")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpPost("")]
        public ResultView Create([FromBody] DepartmentView view)
        {
            return Service.Create(view, this);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="view">请求体中带入修改部门的信息</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Update")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpPut("")]
        public ResultView Update([FromBody] DepartmentView view)
        {
            return Service.Update(view, this);
        }

        #endregion

        #region 数据删除
        /// <summary>
        /// 逻辑删除数据
        /// </summary>
        /// <param name="id">要删除的部门id</param>
        /// <power>删除</power>       
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Delete")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpDelete("Delete/{id}")]
        public ResultView Delete(string id)
        {
            return Service.Delete(id, this);
        }
        /// <summary>
        /// 物理删除数据
        /// </summary>
        /// <param name="id">要删除的部门id</param>
        /// <power>粉碎数据</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Remove")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpDelete("Remove/{id}")]
        public Task<ResultView> Remove(string id)
        {
            return Service.Remove(id, this);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 根据ID获取组织机构详情            
        /// </summary>
        /// <power>详情</power>
        /// <param name="id">组织机构ID</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Single")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpGet("{id}")]
        public DepartmentView SearchById(string id)
        {
            return Service.SingleById<DepartmentView>(id);
        }

        /// <summary>
        /// 查询组织结构列表（分页 + 查询条件）
        /// </summary>
        /// <remarks> 
        ///     排序规则包含： 
        /// 
        ///         name         : 按名称排列
        ///         code         : 按编码排列
        ///         parent       : 按上级部门名称排列
        ///         index        : 按部门序号排列
        ///         date         : 按最后更新时间排列   为默认排序
        ///         
        /// </remarks> 
        /// <param name="queryInfo">查询信息。（包含分页信息、查询条件、排序条件）</param>
        /// <power>查询</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Search")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpPost("Search")]
        public PagedResult<DepartmentView> SearchPage([FromBody] DepartmentQuery queryInfo)
        {
            return Service.FilterPage<DepartmentView>(queryInfo);
        }

        /// <summary>
        /// 查询组织结构列表（无分页 + 查询条件）
        /// </summary>
        /// <remarks> 
        ///     排序规则包含： 
        /// 
        ///         name         : 按名称排列
        ///         code         : 按编码排列
        ///         parent       : 按上级部门名称排列
        ///         index        : 按部门序号排列
        ///         date         : 按最后更新时间排列   为默认排序
        ///         
        /// </remarks> 
        /// <param name="queryInfo">查询信息。（包含查询条件、排序条件）</param>
        /// <power>查询</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Search")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpPost("SearchALL")]
        public IEnumerable<DepartmentView> SearchFilter([FromBody] DepartmentQuery queryInfo)
        {
            return Service.Filter<DepartmentView>(queryInfo);
        }

        /// <summary>
        /// 获取所有的组织机构列表
        /// </summary>
        /// <remarks> 返回的下级对象结构为SelectView，适合用作下拉列表中的显示</remarks>
        /// <power>列表选择</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpGet("Select")]
        public IEnumerable<SelectView> Select()
        {
            DepartmentQuery query = new DepartmentQuery();
            return Service.Select(query);
        }
        /// <summary>
        /// 获取根部门
        /// </summary>
        /// <power>列表选择</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpGet("Root")]
        public DepartmentView Root()
        {
            return Service.FirstRoot();
        }

        /// <summary>
        /// 查询指定机构下的子机构
        /// </summary>
        /// <param name="id">机构管理id</param>
        /// <remarks> 返回的下级对象结构为SelectView，适合用作下拉列表中的显示</remarks>
        /// <power>列表选择</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpGet("{id}/Sub")]
        public IEnumerable<SelectView> Sub(string id)
        {
            DepartmentQuery query = new DepartmentQuery() { ParentId = id };
            return Service.Select(query);
        }

        /// <summary>
        /// 查询当前用户的所有下级部门
        /// </summary>
        /// <remarks> 返回的下级对象结构为SelectView，适合用作下拉列表中的显示</remarks>
        /// <power>列表选择</power>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Department.Select")]
        [SwaggerOperation(Tags = new[] { "系统管理-组织机构管理" })]
        [HttpGet("My/Sub")]
        public IEnumerable<SelectView> MySub()
        {
            var LogonInfo = this.GetStringTokenInfo();
            if (string.IsNullOrWhiteSpace(LogonInfo.DepartmentId))
            {
                ApiException.ThrowBadRequest("您需要先有所属部门才可执行此操作");
            }

            DepartmentQuery query = new DepartmentQuery() { ParentId = LogonInfo.DepartmentId };
            IEnumerable<SelectView> subList = Service.Select(query);
            return subList;
        }
        #endregion
    }
}
