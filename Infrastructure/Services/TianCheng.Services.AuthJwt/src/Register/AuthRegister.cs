using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// 认证与授权的注册
    /// </summary>
    public class AuthRegister
    {
        /// <summary>
        /// 添加Auth的配置信息
        /// </summary>
        /// <param name="sectionName">配置节点名称</param>
        static public IServiceCollection ConfigureServices(string sectionName)
        {
            IServiceCollection services = ServiceLoader.Services;
            // 1、检查并解析所有的功能点。
            AuthActionAnalyze.Analyze();
            // 2、获取配置节点信息并注册
            ServiceLoader.Services.Configure<TokenConfigure>(ServiceLoader.Configuration.GetSection(sectionName));

            // 3、容器操作
            // 将配置读取服务放入容器
            services.AddSingleton<TokenConfigureService>();
            // 将授权逻辑对象放入容器
            services.AddSingleton<IAuthorizationHandler, AuthActionHandler>();
            // 获取所有继承IAuthService接口的服务。并放入容器
            var iauth = typeof(IAuthService);
            foreach (var service in AssemblyHelper.GetTypeByInterface<IAuthService>())
            {
                if (service.IsClass)
                {
                    // 两种方式放入容器，方便调用
                    services.AddSingleton(iauth, service);
                    services.AddSingleton(service);
                }
            }

            // 4、设置认证处理  设置默认为jwt认证，并设置jwt认证参数
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var service = ServiceLoader.GetService<TokenConfigureService>();
                var config = service.Options;
                options.RequireHttpsMetadata = config.IsHttps;                       // 是否使用https
                options.TokenValidationParameters = config.ValidationParameters;     // 设置验证jwt的参数
                options.SaveToken = true;
            });

            // 5、设置授权处理
            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = true;          // 如果验证失败继续执行后续验证,当一个api 方法设置了多个授权时会用到。
                options.AddPolicy(AuthActionRequirement.PolicyName, policy => policy.Requirements.Add(new AuthActionRequirement()));    // 设置授权的处理方式
            });

            // 6、返回服务，方便后续链式调用
            return services;
        }

        /// <summary>
        /// 配置请求管道信息
        /// </summary>
        /// <param name="app"></param>
        static public void ConfigurePipeline(IApplicationBuilder app)
        {
            // 认证
            app.UseAuthentication();
            // 授权
            app.UseAuthorization();
        }
    }
}
