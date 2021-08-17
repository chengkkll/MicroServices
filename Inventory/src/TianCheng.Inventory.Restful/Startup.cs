using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Controller.Core.PlugIn.Swagger;
using TianCheng.DAL;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;

namespace TianCheng.Inventory.Restful
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
                options.AddControllerSetting("api/Inventory");
                options.AddHasCors();
                options.AddConsul();
                options.AddDbServices();
                options.AddBusinessServices();
                options.AddAuthJwt();
                options.AddSwagger();
            });
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseTianCheng(options =>
            {
                options.UseSerilog();
                options.UseCors();
                options.UseControllerSetting();

                options.UseAuthJwt();
                options.UseConsul();
                options.UseSwagger();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
