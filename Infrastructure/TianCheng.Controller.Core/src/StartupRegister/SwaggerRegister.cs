using TianCheng.Controller.Core.PlugIn.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using TianCheng.Common;
using Microsoft.Extensions.Logging;
using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// TianChengConfigureOptions 的扩展方法
    /// </summary>
    static public class SwaggerRegister
    {
        #region Swagger
        /// <summary>
        /// 注册Swagger的Key
        /// </summary>
        static private readonly string Swagger = "Swagger";

        /// <summary>
        /// 判断是否已注册Swagger
        /// </summary>
        /// <returns></returns>
        static public bool HasSwagger(this TianChengConfigureOptions options)
        {
            return options.HasRegister(Swagger);
        }
        /// <summary>
        /// 注册Swagger服务
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sectionName">swagger配置节点名称</param>
        static public void AddSwagger(this TianChengConfigureOptions options, string sectionName = "Swagger")
        {
            IServiceCollection services = ServiceLoader.Services;

            // todo : 需要优化
            var configurationService = new SwaggerConfigurationService(sectionName);
            ServiceLoader.Services.AddSingleton<ISwaggerConfigurationService>(s => configurationService);
            var configList = configurationService.Data;

            Dictionary<string, SwaggerDocInfo> configDict = configList.ToDict();
            services.AddSwaggerGen(options =>
            {
                foreach (var key in configDict.Keys)
                {
                    var doc = configDict[key];

                    options.SwaggerDoc(doc.Code, new OpenApiInfo
                    {
                        Version = doc.Version,
                        Title = doc.Title,
                        Description = doc.Description,
                        TermsOfService = string.IsNullOrWhiteSpace(doc.TermsOfService) ? null : new Uri(doc.TermsOfService),
                        Contact = new OpenApiContact
                        {
                            Name = doc.ContactName,
                            Email = doc.ContactEmail,
                            Url = string.IsNullOrWhiteSpace(doc.ContactUrl) ? null : new Uri(doc.ContactUrl),
                        },
                        License = new OpenApiLicense
                        {
                            Name = doc.LicenseName,
                            Url = string.IsNullOrWhiteSpace(doc.LicenseUrl) ? null : new Uri(doc.LicenseUrl),
                        }
                    });
                }

                // 启用便签功能，实现接口按指定分类（Tags）进行分组
                options.EnableAnnotations();

                // 拷贝引用程序集目录下的xml文件
                SwaggerXmlFile.CopyXmlFile();
                foreach (var file in SwaggerXmlFile.GetXmlFile())
                {
                    options.IncludeXmlComments(file);
                }

                // 增加默认的接口注释信息
                options.DocumentFilter<ControllerDescription>();

                // 根据特性设置的View属性
                options.OperationFilter<ViewRequestFilter>();  // 增加特性的标记
                options.DocumentFilter<ViewComponentsChange>();// 根据标记信息来处理文件对象结构
                options.SchemaFilter<SetDefaultValueFilter>(); // 设置默认值

                // 设置页面顶部的登录验证内容
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = $"Token验证。格式：Bearer token \r\n\t 注意：Bearer与token之间需要 一个空格连接"
                });
                // 有权限控制的接口注释信息添加 
                options.OperationFilter<AuthorizeResponseDescription>();
            });

            // 标记注册完成
            options.Register(Swagger);
        }

        /// <summary>
        /// 使用Swagger服务
        /// </summary>
        /// <param name="options"></param>
        static public void UseSwagger(this TianChengConfigureOptions options)
        {
            if (options.HasSwagger())
            {
                options.App.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                });
                options.App.UseSwaggerUI(options =>
                {
                    ISwaggerConfigurationService SwaggerConfigurationService = ServiceLoader.GetService<ISwaggerConfigurationService>();
                    // 设置页面名称
                    var curr = SwaggerConfigurationService.Current;
                    options.DocumentTitle = curr.DocumentTitle;
                    if (string.IsNullOrWhiteSpace(curr.Code))
                    {
                        ServiceLog.Logger.Warning("请配置Swagger的信息");
                        options.SwaggerEndpoint(curr.EndpointUrl, curr.EndpointDesc);
                    }
                    else
                    {
                        options.SwaggerEndpoint($"{curr.Code}/swagger.json", curr.EndpointDesc);
                    }

                    options.RoutePrefix = "swagger";
                    // 设置文件的选项
                    foreach (var conf in SwaggerConfigurationService.Data)
                    {
                        if (conf.Code != curr.Code)
                        {
                            options.SwaggerEndpoint($"{curr.Code}/swagger.json", conf.EndpointDesc);
                        }
                    }

                    // 启用地址路径  可通过url路径直接跳转到文档中api所在的位置
                    options.EnableDeepLinking();
                    // 打开页面时，标签的显示方式。  none表示不展开；list表示展开标记；full表示展开标记和操作api
                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    // 启用筛选。顶部栏将显示一个编辑框，可以根据标签名称对接口进行过滤。
                    options.EnableFilter();
                    // 显示引用实体的深度。
                    options.DefaultModelExpandDepth(2);
                    // -1表示不显示引用实体
                    //options.DefaultModelsExpandDepth(-1);
                    // 显示请求耗时 单位：ms
                    options.DisplayRequestDuration();
                    // 是否在api中显示OperationId,默认OperationId的值为方法名称
                    options.DisplayOperationId();
                });
            }
        }
        #endregion
    }
}
