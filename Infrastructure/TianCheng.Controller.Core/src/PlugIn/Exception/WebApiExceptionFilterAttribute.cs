using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写基类的异常处理方法
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiException ae)
            {
                context.HttpContext.Response.StatusCode = (int)ae.HttpStatus;
                context.Result = new JsonResult(new { code = ae.Code, message = ae.Message });
                base.OnException(context);
            }
            else if (context.Exception is System.TimeoutException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Result = new JsonResult(new { code = 502, message = "无法链接数据库" });
                base.OnException(context);
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(new { code = 500, message = context.Exception.Message });
                base.OnException(context);
            }
        }
    }
}
