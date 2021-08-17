using Microsoft.AspNetCore.Authorization;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// 定义一些固定的权限条件
    /// </summary>
    public class AuthActionRequirement : IAuthorizationRequirement
    {
        static public readonly string PolicyName = "AuthAction";
    }
}
