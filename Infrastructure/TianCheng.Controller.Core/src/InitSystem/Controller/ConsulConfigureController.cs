using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TianCheng.Common;

namespace TianCheng.Controller.Core
{
    ///// <summary>
    ///// Consul配置接口     [对外接口Api]
    ///// </summary>
    //[Produces("application/json")]
    //[Route("ConsulConfigure")]
    //public class ConsulConfigureController : BController<ConsulConfigureService>
    //{
    //    #region 构造方法
    //    /// <summary> 
    //    /// 构造方法
    //    /// </summary>
    //    /// <param name="service"></param>
    //    public ConsulConfigureController(ConsulConfigureService service) : base(service)
    //    {
    //    }
    //    #endregion

    //    #region 初始化Consul配置
    //    /// <summary>
    //    /// 初始化Consul配置
    //    /// </summary>
    //    [SwaggerOperation(Tags = new[] { "系统初始化" })]
    //    [HttpPut("Init")]
    //    public Task<ResultView> InitConsulKv()
    //    {
    //        return Service.InitConsulKvAsync();
    //    }
    //    #endregion
    //}
}
