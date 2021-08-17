using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TianCheng.Common;
using TianCheng.Service.Core;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using Microsoft.AspNetCore.Http;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 登录记录管理    [ Service ]
    /// </summary>
    public class LoginHistoryService : MongoBusinessService<LoginHistoryDAL, LoginHistoryInfo, LoginHistoryQuery>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public LoginHistoryService(LoginHistoryDAL dal) : base(dal)
        {
        }
        #endregion

        /// <summary>
        /// 登录事件处理
        /// </summary>
        /// <param name="employee"></param>
        internal static void OnLogin(EmployeeInfo employee)
        {
            // 1、记录登录信息
            // 2、如果登录用户需要验证IP地址，验证
            if (employee.LoginType == LoginVerifierType.IpAddress)
            {
                // 获取用户所在部门的IP地址范围
                // 比较用户的IP地址
                IHttpContextAccessor accessor = ServiceLoader.GetService<IHttpContextAccessor>();
                string userIp = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            if (employee.LoginType == LoginVerifierType.ShortRange)
            {

            }
        }
    }
}
