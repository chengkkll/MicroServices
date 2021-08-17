using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace WebApplication1
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                //Configuration.GetSection("exceptionless").Bind(Exceptionless.ExceptionlessClient.Default.Configuration);
                Serilog.Log.Logger = TianCheng.Common.GlobalLog.Logger;
                Serilog.Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Serilog.Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .TianChengConfiguration()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog(dispose: true);
    }
}
