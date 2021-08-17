using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Serilog;
using TianCheng.Common;
using TianCheng.Controller.Core;
using TianCheng.Controller.Core.PlugIn.Swagger;
using TianCheng.DAL;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTianCheng(Configuration, options =>
            {
                options.AddSerilog();
                options.AddAutoMapper();
                options.AddControllerSetting();
                options.AddHasCors();
                options.AddConsul();
                options.AddDbServices();
                options.AddBusinessServices();
                options.AddAuthJwt();
                options.AddSwagger();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
