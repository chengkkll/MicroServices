using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using TianCheng.Common;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using TianCheng.ConsulHelper;
using Flurl;
using Flurl.Http;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 登录处理
    /// </summary>
    public class TAuthService : IAuthService<LoginView>, IBusinessService
    {
        /// <summary>
        /// 获取用户是否有指定的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        private bool HasPowerByHttp(string userId, string powerCode)
        {
            string restfulPath = $"api/org/Auth/HasPowerCode/{userId}/{powerCode}";
            string serviceName = "TianCheng.Organization";

            var loader = ServiceLoader.GetService<IConsulServiceDiscovery>();
            var service = loader.GetServices(serviceName, null);

            // 组织请求的api地址
            string Host = service.ServiceHostAddress();
            if (service.Service.Tags.Contains("http"))
            {
                Host = "http://" + Host;
            }
            else
            {
                Host = "https://" + Host;
            }
            return Host.AppendPathSegment(restfulPath).GetJsonAsync<bool>().Result;
        }
        /// <summary>
        /// 本地判断是否包含权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        public bool HasPowerLocalhost(string userId, string powerCode)
        {
            EmployeeDAL employeeDal = ServiceLoader.GetService<EmployeeDAL>();
            var empl = employeeDal.SingleByStringId(userId);
            // 无法找到用户信息
            if (empl == null)
            {
                return false;
            }
            // 无法找到您的角色信息，请联系管理员
            if (string.IsNullOrWhiteSpace(empl.Role.Id))
            {
                return false;
            }
            RoleDAL roleDal = ServiceLoader.GetService<RoleDAL>();
            var role = roleDal.SingleByStringId(empl.Role.Id);
            // 您的角色信息错误，请联系管理员
            if (role == null)
            {
                return false;
            }

            return role.FunctionPower.Any(e => e.Policy == powerCode);
        }

        /// <summary>
        /// 判断用户是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        public bool HasPower(string userId, string powerCode)
        {
            return HasPowerByHttp(userId, powerCode);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public TokenResult Login(LoginView loginInfo)
        {
            string account = loginInfo.Account.Trim();
            string password = loginInfo.Password.Trim();
            //查询密码正确的可用用户列表
            EmployeeDAL employeeDal = ServiceLoader.GetService<EmployeeDAL>();
            var pwdQuery = employeeDal.Queryable().Where(e => e.LogonPassword == password && e.IsDelete == false).ToList();

            // 通过账号和密码验证登录
            var employeeList = pwdQuery.Where(e => e.LogonAccount == account);
            if (employeeList.Count() > 1)
            {
                throw ApiException.BadRequest("有多个满足条件的用户，无法登陆。");
            }

            //// 如果查找不到用户信息，并且允许用电话登录，尝试电话号码+登录密码登录
            //if (employeeList.Count() == 0 && AuthServiceOption.Option.IsLogonByTelephone)
            //{
            //    employeeList = pwdQuery.Where(e => e.Telephone == account || e.Mobile == account);
            //    if (employeeList.Count() > 1)
            //    {
            //        throw ApiException.BadRequest("有多个满足条件的用户，无法通过电话登陆。");
            //    }
            //}

            var employee = employeeList.FirstOrDefault();

            if (employee == null)
            {
                throw ApiException.BadRequest("您的登陆账号或密码错误。");
            }
            if (employee.State == UserState.Disable)
            {
                throw ApiException.BadRequest("您被禁止登录，请与管理员联系。");
            }
            if (employee.State == UserState.LogonLock)
            {
                throw ApiException.BadRequest("账号已锁，请与管理员联系。");
            }

            // 执行扩展的登录事件
            //OnLogin?.Invoke(employee);

            // 设置Claim并返回Token
            return JwtBuilder.BuildToken(account, new List<Claim>
            {
                ClaimHelper.NewUserId(employee.Id.ToString()),  // id
                ClaimHelper.NewUserName(employee.Name ?? ""),   // name
                new Claim("roleId",employee.Role?.Id ?? ""),
                new Claim("depId",employee.Department?.Id ?? ""),
                new Claim("depName",employee.Department?.Name ?? ""),
                new Claim("workstation",employee.Role?.Code ?? ""),
                ClaimHelper.NewAuthorizationService(this)
            });
        }
    }
}
