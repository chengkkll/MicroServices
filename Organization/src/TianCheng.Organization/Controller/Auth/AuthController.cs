using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TianCheng.Controller.Core;
using TianCheng.Services.AuthJwt;
using TianCheng.Organization.Services;
using TianCheng.Common;

namespace TianCheng.Organization.Controller
{
    /// <summary>
    /// ϵͳ����
    /// </summary>
    [Produces("application/json")]
    [Route("Auth")]
    public class AuthController : MongoController<TAuthService>
    {
        #region ���췽��
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public AuthController(TAuthService service) : base(service)
        {
        }
        #endregion

        /// <summary>
        /// ����˵�¼�ӿ�
        /// </summary>
        /// <param name="loginView"></param>
        /// <response code="200">��¼�ɹ�����token</response>
        /// <returns></returns>
        [HttpPost("login")]
        [SwaggerOperation(Tags = new[] { "ϵͳ����-��¼��֤" })]
        public TokenResult Login([FromBody] LoginView loginView)
        {
            return Service.Login(loginView);
        }


        /// <summary>
        /// �ж��û��Ƿ���ָ����Ȩ��
        /// </summary>
        /// <param name="userId">�û�id</param>
        /// <param name="code">Ȩ������</param>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "ϵͳ����-Ա������" })]
        [HttpGet("HasPowerCode/{userId}/{code}")]
        public bool HasPowerCode(string userId, string code)
        {
            return Service.HasPowerLocalhost(userId, code);
        }

        ///// <summary>
        ///// �˳���¼
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("logout")]
        //[SwaggerOperation(Tags = new[] { "ϵͳ����-��¼��֤" })]
        //public ResultView Logout()
        //{
        //    _authService.Logout(LogonInfo);
        //    return ResultView.Success();
        //}
    }
}