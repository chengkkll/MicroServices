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
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace GatewayApp1
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
            //services.AddControllers();
            services.AddOcelot(Configuration).AddConsul();//.AddConfigStoredInConsul();

            services.AddTianCheng(Configuration, options: options =>
             {
                 options.AddSerilog();
                 options.AddAutoMapper();
                 options.AddControllerSetting();
                 options.AddHasCors();
                 options.AddConsul();
                 options.AddBusinessServices();
                 options.AddAuthJwt();
                 options.AddSwagger();
             });

            //services.AddMvc();
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("ApiGateway", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Íø¹Ø·þÎñ", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //var apis = new List<string> { "TianCheng.Organization", "TianCheng.Inventory" };
            //app.UseSwagger()
            //   .UseSwaggerUI(options =>
            //   {
            //       apis.ForEach(m =>
            //       {
            //           options.SwaggerEndpoint($"{m}/swagger.json", m);
            //       });
            //   });

            app.UseOcelot().Wait();

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

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
