using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using System.Threading.Tasks;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// 授权处理
    /// </summary>
    public class AuthActionHandler : AuthorizationHandler<AuthActionRequirement>
    {
        /// <summary>
        /// 重载异步处理程序
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthActionRequirement requirement)
        {
            // 根据请求来判断 权限代码
            string powerCode = AuthActionAnalyze.GetPowerCode(context.Resource as RouteEndpoint);

            // 获取token中的用户id
            ClaimsIdentity ci = context.User.Identity as ClaimsIdentity;
            string uid = ClaimHelper.GetUserId(ci);
            string authServiceName = ClaimHelper.GetAuthorizationService(ci);

            if (!string.IsNullOrWhiteSpace(uid))
            {
                // 判断当前用户是否拥有指定权限
                IAuthService authService = ServiceLoader.GetService<IAuthService>(authServiceName);
                if (authService == null)
                {
                    // 使用配置文件中指定的默认处理服务
                    TokenConfigure tokenConfigure = ServiceLoader.GetService<TokenConfigureService>().Options;
                    string confServiceName = tokenConfigure.AuthService;
                    authService = ServiceLoader.GetService<IAuthService>(confServiceName);
                    // 如果指定的服务不存在，在容器中随便取一个
                    if(authService == null)
                    {
                        authService = ServiceLoader.GetService<IAuthService>();
                    }
                }
                if (authService != null && authService.HasPower(uid, powerCode))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
