using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TianCheng.Controller.Core;
using TianCheng.Services.AuthJwt;
using TianCheng.Organization.Services;
using TianCheng.Common;

namespace TianCheng.Organization.Controller
{
    /// <summary>
    /// 系统控制
    /// </summary>
    [Produces("application/json")]
    [Route("Auth")]
    public class AuthController : MongoController<TAuthService>
    {
        #region 构造方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public AuthController(TAuthService service) : base(service)
        {
        }
        #endregion

        /// <summary>
        /// 管理端登录接口
        /// </summary>
        /// <param name="loginView"></param>
        /// <response code="200">登录成功返回token</response>
        /// <returns></returns>
        [HttpPost("login")]
        [SwaggerOperation(Tags = new[] { "系统管理-登录验证" })]
        public TokenResult Login([FromBody] LoginView loginView)
        {
            return Service.Login(loginView);
        }


        /// <summary>
        /// 判断用户是否有指定的权限
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="code">权限名称</param>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "系统管理-员工管理" })]
        [HttpGet("HasPowerCode/{userId}/{code}")]
        public bool HasPowerCode(string userId, string code)
        {
            return Service.HasPowerLocalhost(userId, code);
        }

        ///// <summary>
        ///// 退出登录
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("logout")]
        //[SwaggerOperation(Tags = new[] { "系统管理-登录验证" })]
        //public ResultView Logout()
        //{
        //    _authService.Logout(LogonInfo);
        //    return ResultView.Success();
        //}
    }
}