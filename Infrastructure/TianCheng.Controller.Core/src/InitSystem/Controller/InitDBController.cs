using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Controller.Core
{
    ///// <summary>
    ///// Consul配置接口     [对外接口Api]
    ///// </summary>
    //[Produces("application/json")]
    //[Route("PostgreSql")]
    //public class InitDBController : BController<InitDBService>
    //{
    //    #region 构造方法
    //    /// <summary> 
    //    /// 构造方法
    //    /// </summary>
    //    /// <param name="service"></param>
    //    public InitDBController(InitDBService service) : base(service)
    //    {
    //    }
    //    #endregion

    //    #region 初始化Postgresql配置  

    //    /// <summary>
    //    /// 初始化数据库
    //    /// </summary>
    //    /// <param name="connection">Host=192.168.1.22;Port=59432;User ID=tc;Password=123qwe;</param>
    //    /// <returns></returns>
    //    [SwaggerOperation(Tags = new[] { "系统初始化" })]
    //    [HttpPost("InitSql")]
    //    public ResultView InitPostgers([FromForm] string connection)
    //    {
    //        return Service.InitSql(connection);
    //    }
    //    #endregion
    //}
}
