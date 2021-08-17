using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using Serilog;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Controller.Core.PlugIn.Swagger;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;
using TianCheng.Organization.Model;
using TianCheng.Organization.Services;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;


namespace TianCheng.Organization.Restful
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTianCheng(Configuration, options: options =>
            {
                options.AddSerilog();
                options.AddAutoMapper();
                //options.AddCap();
                options.AddControllerSetting("api/org");
                options.AddHasCors();
                options.AddConsul();
                options.AddDbServices();
                options.AddMongoAccess();
                options.AddBusinessServices();
                options.AddAuthJwt();
                options.AddSwagger();
                options.AddOrganization();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            app.UseTianCheng(options =>
            {
                options.UseSerilog();
                options.UseCors();
                options.UseControllerSetting();
                options.UseDbServices();
                options.UseAuthJwt();
                options.UseConsul();
                options.UseSwagger();
            });


            //app.UseCap();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
