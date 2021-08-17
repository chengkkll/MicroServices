using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 控制器中获取Token中用户信息
    /// </summary>
    static public class ControllerTokenLogonExt
    {
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        static public StringTokenInfo GetStringTokenInfo(this ControllerBase controller)
        {
            if (controller.User == null || controller.User.Claims == null)
            {
                return new StringTokenInfo();
            }
            StringTokenInfo _LogonInfo = new StringTokenInfo();
            foreach (var item in controller.User.Claims)
            {
                switch (item.Type)
                {
                    case "name":
                        {
                            _LogonInfo.Name = item.Value;
                            break;
                        }
                    case "id":
                        {
                            _LogonInfo.EmployeeId = item.Value;
                            break;
                        }
                    case "roleId":
                        {
                            _LogonInfo.RoleId = item.Value;
                            break;
                        }
                    case "depId":
                        {
                            _LogonInfo.DepartmentId = item.Value;
                            break;
                        }
                    case "depName":
                        {
                            _LogonInfo.DepartmentName = item.Value;
                            break;
                        }
                    case "function_policy":
                        {
                            _LogonInfo.FunctionPolicyList.Add(item.Value);
                            break;
                        }
                }
            }
            return _LogonInfo;
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        static public IntTokenInfo GetIntTokenInfo(this ControllerBase controller)
        {
            if (controller.User == null || controller.User.Claims == null)
            {
                return new IntTokenInfo();
            }
            IntTokenInfo _LogonInfo = new IntTokenInfo();
            foreach (var item in controller.User.Claims)
            {
                switch (item.Type)
                {
                    case "name":
                        {
                            _LogonInfo.Name = item.Value;
                            break;
                        }
                    case "id":
                        {
                            if (int.TryParse(item.Value, out int id))
                            {
                                _LogonInfo.EmployeeId = id;
                            }
                            break;
                        }
                    case "roleId":
                        {
                            _LogonInfo.RoleId = item.Value;
                            break;
                        }
                    case "depId":
                        {
                            if (int.TryParse(item.Value, out int id))
                            {
                                _LogonInfo.DepartmentId = id;
                            }
                            break;
                        }
                    case "depName":
                        {
                            _LogonInfo.DepartmentName = item.Value;
                            break;
                        }
                    case "function_policy":
                        {
                            _LogonInfo.FunctionPolicyList.Add(item.Value);
                            break;
                        }
                }
            }
            return _LogonInfo;
        }

        /// <summary>
        /// 获取当前登录用户的Id
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        static public string GetEmployeeStringId(this ControllerBase controller)
        {
            // todo : 如果当前请求无权限验证，就不会解析Token，导致无用户信息
            if (controller.User == null || controller.User.Claims == null)
            {
                return string.Empty;
            }
            var item = controller.User.Claims.Where(e => e.Type == "id").FirstOrDefault();
            return item.Value;
        }
    }
}
