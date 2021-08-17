using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Organization.Model;
using TianCheng.Organization.Services;

namespace TianCheng.Organization.Controller
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Produces("application/json")]
    [Route("Menu")]
    public class MenuController : MongoController<SystemMenuService>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public MenuController(SystemMenuService service) : base(service)
        {
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 查询单页面的菜单信息，以树形结构显示，无分页信息
        /// </summary>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Menu.Tree")]
        [SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        [HttpGet("")]
        public List<MenuMainView> Tree()
        {
            return Service.SearchMenuTree();
        }
        #endregion

        ///// <summary>
        ///// 初始化菜单   清除已有菜单，重新设置默认菜单
        ///// </summary>
        //[TianCheng.Services.AuthJwt.AuthAction("SystemManage.Menu.Init")]
        //[SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        //[HttpPost("Init")]
        //public void Init()
        //{
        //    Service.Init();
        //}

        #region 新增修改数据
        /// <summary>
        /// 新增一个菜单
        /// </summary>
        /// <param name="view">请求体中放置新增对象的信息</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Menu.Create")]
        [SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        [HttpPost("")]
        public ResultView Create([FromBody] MenuView view)
        {
            return Service.Create(view, this);
        }

        /// <summary>
        /// 修改一个菜单
        /// </summary>
        /// <param name="view">请求体中带入修改对象的信息</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Menu.Update")]
        [SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        [HttpPut("")]
        public ResultView Update([FromBody] MenuView view)
        {
            return Service.Update(view, this);
        }
        #endregion

        #region 数据删除
        /// <summary>
        /// 删除一个主菜单
        /// </summary>
        /// <param name="id">要删除的对象id</param>
        [TianCheng.Services.AuthJwt.AuthAction("SystemManage.Menu.Remove")]
        [SwaggerOperation(Tags = new[] { "系统管理-权限管理" })]
        [HttpDelete("Remove/{id}")]
        public Task<ResultView> Remove(string id)
        {
            return Service.Remove(id, this);
        }
        #endregion
    }
}

