using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Inventory.DTO;
using TianCheng.Inventory.Services;
using TianCheng.Service.Core;

namespace TianCheng.Inventory.Controller
{
    /// <summary>
    /// 分类管理
    /// </summary>
    [Produces("application/json")]
    [Route("Category")]
    public class CategoryController : MongoController<CategoryService>
    {
        #region 构造方法
        /// <summary> 
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public CategoryController(CategoryService service) : base(service)
        {
        }
        #endregion

        #region 新增修改数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="view">请求体中放置新增的信息</param>
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Create")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
        [HttpPost("")]
        public ResultView Create([FromBody] CategoryView view)
        {
            return Service.Create(view, this);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="view">请求体中带入修改的信息</param>
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Update")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
        [HttpPut("")]
        public ResultView Update([FromBody] CategoryView view)
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
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Delete")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
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
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Remove")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
        [HttpDelete("Remove/{id}")]
        public Task<ResultView> Remove(string id)
        {
            return Service.Remove(id, this);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 根据ID获取详情            
        /// </summary>
        /// <power>详情</power>
        /// <param name="id">ID</param>
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Single")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
        [HttpGet("{id}")]
        public CategoryView SearchById(string id)
        {
            return Service.SingleById<CategoryView>(id);
        }

        /// <summary>
        /// 查询数据列表（分页 + 查询条件）
        /// </summary>
        /// <remarks> 
        ///     排序规则包含： 
        /// 
        ///         name         : 按名称排列
        ///         date         : 按最后更新时间排列   为默认排序
        ///         
        /// </remarks> 
        /// <param name="queryInfo">查询信息。（包含分页信息、查询条件、排序条件）</param>
        /// <power>查询</power>
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Search")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
        [HttpPost("Search")]
        public PagedResult<CategoryView> SearchPage([FromBody] CategoryQuery queryInfo)
        {
            return Service.FilterPage<CategoryView>(queryInfo);
        }

        /// <summary>
        /// 查询数据列表（无分页 + 查询条件）
        /// </summary>
        /// <remarks> 
        ///     排序规则包含： 
        /// 
        ///         name         : 按名称排列
        ///         date         : 按最后更新时间排列   为默认排序
        ///         
        /// </remarks> 
        /// <param name="queryInfo">查询信息。（包含查询条件、排序条件）</param>
        /// <power>查询</power>
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Search")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
        [HttpPost("SearchALL")]
        public IEnumerable<CategoryView> SearchFilter([FromBody] CategoryQuery queryInfo)
        {
            return Service.Filter<CategoryView>(queryInfo);
        }

        /// <summary>
        /// 获取所有数据列表
        /// </summary>
        /// <remarks> 返回的下级对象结构为SelectView，适合用作下拉列表中的显示</remarks>
        /// <power>列表选择</power>
        [TianCheng.Services.AuthJwt.AuthAction("Inventory.Category.Select")]
        [SwaggerOperation(Tags = new[] { "库存管理-分类管理" })]
        [HttpGet("Select")]
        public IEnumerable<SelectView> Select()
        {
            CategoryQuery query = new CategoryQuery();
            return Service.Select(query);
        }
        #endregion
    }
}
