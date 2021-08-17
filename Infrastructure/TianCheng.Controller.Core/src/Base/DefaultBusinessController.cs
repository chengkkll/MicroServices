using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.DAL;
using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 带有通用业务操作的Controller
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="VO"></typeparam>
    /// <typeparam name="QO"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public class DefaultBusinessController<DO, VO, QO, IdType> : BController<DO, QO, IdType>
        where DO : IBusinessModel<IdType>, new()
        where QO : QueryObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public DefaultBusinessController(ViewBusinessService<DO, QO, IdType> service) : base(service)
        {

        }

        #region 新增修改数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="view">请求体中放置新增对象的信息</param>
        /// <power>新增</power>
        /// <returns>返回创建成功的对象ID</returns>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.Create")]
        [HttpPost("")]
        public ResultView Create([FromBody]VO view)
        {
            // 新增数据
            return Service.Create(view, this.LogonInfo());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="view">请求体中带入修改对象的信息</param>
        /// <returns></returns>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.Update")]
        [HttpPut("")]
        public ResultView Update([FromBody]VO view)
        {
            // 修改数据
            return Service.Update(view, LogonInfo);
        }
        #endregion

        #region 数据删除
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">要逻辑删除的对象ID</param>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.Delete")]
        [HttpDelete("Delete/{id}")]
        public ResultView Delete(string id)
        {
            return Service.Delete(id, LogonInfo);
        }

        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.UnDelete")]
        [HttpPatch("UnDelete/{id}")]
        public ResultView UnDelete(string id)
        {
            return Service.UnDelete(id, LogonInfo);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">要物理删除的对象</param>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.Remove")]
        [HttpDelete("Remove/{id}")]
        public ResultView Remove(string id)
        {
            return Service.Remove(id, LogonInfo);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 查看详情 - 根据ID
        /// </summary>
        /// <param name="id">要获取的对象ID</param>
        /// <returns></returns>
        /// <power>详情</power>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.SearchById")]
        [HttpGet("{id}")]
        public VO SingleByStringId(string id)
        {
            return Service.SingleById<VO>(id);
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="queryInfo">查询信息。（包含分页信息、查询条件、排序条件）
        /// 排序规则参见上面的描述
        /// </param>
        /// <power>查询</power>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.Search")]
        [HttpPost("Search")]
        public PagedResult<VO> SearchPage([FromBody]QO queryInfo)
        {
            return Service.FilterPage<VO>(queryInfo);
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回满足条件的数量</returns>
        /// <power>查询</power>
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Company.Manage.Search")]
        [HttpPost("SearchCount")]
        public virtual int SearchCount([FromBody]QO queryInfo)
        {
            return Service.Count(queryInfo);
        }

        #endregion
    }
}
