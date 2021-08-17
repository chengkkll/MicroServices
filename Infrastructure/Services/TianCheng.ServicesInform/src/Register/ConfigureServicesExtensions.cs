using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TianCheng.Common;
using TianCheng.ConsulHelper;
using TianCheng.ServicesInform;

namespace TianCheng.ServicesInform
{
    /// <summary>
    /// 
    /// </summary>
    static public class ConfigureServicesExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentMicroServiceName"></param>
        static public void AddInform(string currentMicroServiceName)
        {
            // 检查Consul的连接
            // 解析所有的CommandName ,并注册到Consul中。
            List<string> CommandNameList = new List<string>();
            string interfaceName = typeof(ICommandName).Name;
            foreach (var assembly in AssemblyHelper.GetAssemblyList())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsClass && !type.IsValueType)
                    {
                        continue;
                    }
                    if (type.GetInterfaces().Where(i => i.Name == interfaceName).Count() > 0)
                    {
                        var inst = Activator.CreateInstance(type);
                        foreach (var prop in type.GetFields())
                        {
                            CommandNameList.Add(prop.GetValue(inst).ToString());
                        }
                        foreach (var prop in type.GetProperties())
                        {
                            CommandNameList.Add(prop.GetValue(inst).ToString());
                        }
                    }
                }
            }
            // 将命令加入Consul中
            foreach (var cn in CommandNameList)
            {
                InitCommandName(cn);
            }

            //var context = services.BuildServiceProvider().GetService<IHttpContextAccessor>();
            //cw context.HttpContext.Request.Host.Value;

            // 查询本服务是否有需要注册的扩展命令
            List<MultiCheckAttribute> data = new List<MultiCheckAttribute>();
            foreach (var assembly in AssemblyHelper.GetAssemblyList())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Name.Contains("Role"))
                    {

                    }
                    foreach (var method in type.GetMethods())
                    {
                        if (method.GetCustomAttributes(typeof(MultiCheckAttribute), true).Count() > 0)
                        {
                            MultiCheckAttribute mck = new MultiCheckAttribute("");
                            if (GetAttribute(method, out MultiCheckAttribute multi))
                            {
                                mck.CommandName = multi.CommandName;
                            }

                            if (GetHttpMethodAttribute(method, out HttpMethodAttribute routeMehtod))
                            {
                                mck.HttpMethod = routeMehtod.HttpMethods.FirstOrDefault();
                                mck.RouteMethod = routeMehtod.Template;
                                mck.RouteController = GetRouteAttribute(type);
                                mck.CurrentProject = currentMicroServiceName;
                            }
                            data.Add(mck);
                        }
                    }
                }
            }

            // 将信息注册到配置中
            foreach (var info in data)
            {
                AppendServiceToCommandName(info);
            }
        }

        #region 反射获取对象
        static private bool GetAttribute<T>(MethodInfo method, out T result)
        {
            result = (T)method.GetCustomAttributes(typeof(MultiCheckAttribute), true).FirstOrDefault();
            return result != null;
        }

        static private bool GetHttpMethodAttribute(MethodInfo method, out HttpMethodAttribute result)
        {
            result = method.GetCustomAttributes(typeof(HttpPostAttribute), true).FirstOrDefault() as HttpPostAttribute;
            if (result == null)
            {
                result = method.GetCustomAttributes(typeof(HttpGetAttribute), true).FirstOrDefault() as HttpGetAttribute;
            }
            if (result == null)
            {
                result = method.GetCustomAttributes(typeof(HttpPutAttribute), true).FirstOrDefault() as HttpPutAttribute;
            }
            if (result == null)
            {
                result = method.GetCustomAttributes(typeof(HttpPatchAttribute), true).FirstOrDefault() as HttpPatchAttribute;
            }
            if (result == null)
            {
                result = method.GetCustomAttributes(typeof(HttpDeleteAttribute), true).FirstOrDefault() as HttpDeleteAttribute;
            }
            return result != null;
        }

        static private string GetRouteAttribute(Type type)
        {
            RouteAttribute result = (RouteAttribute)type.GetCustomAttributes(typeof(RouteAttribute), true).FirstOrDefault();
            if (result != null)
            {
                if (result.Template != "[controller]")
                {
                    return result.Template;
                }
            }
            return type.Name.Replace("Controller", "");
        }
        #endregion

        #region 更新数据
        /// <summary>
        /// 发送端设置更新数据的初始值
        /// </summary>
        /// <param name="key"></param>
        static private void InitCommandName(string key)
        {
            // todo: 创建用于更新配置的接口，此处通过反射来使用配置好的调用配置修改服务


            ConsulConfigurationHelper.Set(MultiCheckAttribute.GetKey(key), "[]", false);
        }

        /// <summary>
        /// 处理端更新设置配置
        /// </summary>
        /// <param name="info"></param>
        static private void AppendServiceToCommandName(MultiCheckAttribute info)
        {
            var data = ConsulConfigurationHelper.GetObject<List<MultiCheckAttribute>>(info.Key);

            var config = data.Where(e => e.CurrentProject == info.CurrentProject &&
                                         e.RouteController == info.RouteController &&
                                         e.RouteMethod == info.RouteMethod &&
                                         e.HttpMethod == info.HttpMethod).FirstOrDefault();
            if (config != null)
            {
                config = info;
            }
            else
            {
                data.Add(info);
            }

            ConsulConfigurationHelper.Set(info.Key, data.ToJson());
        }
        #endregion

    }


}
