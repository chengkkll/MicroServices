using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NpgBenchmark.Benchmark;
using NpgBenchmark.DAL;
using System;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.DAL.NpgByDapper;
using TianCheng.DAL;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace NpgBenchmark
{
    class Program
    {
        public static void Main(string[] args)
        {
            //
            ReadJson.Init();

            #region 初始化配置
            var conf = new ConfigurationBuilder().TianChengConfiguration();
            IServiceCollection services = new ServiceCollection();
            services.AddTianChengCommon(conf, options =>
             {
                 options.AddDbServices();
             });
            IServiceProvider provider = services.BuildServiceProvider();

            ServiceLoader.Instance = provider;
            #endregion


            //Console.WriteLine(envName);

            //var builder = new HostBuilder()
            //    .TianChengConfiguration()
            //    .ConfigureAppConfiguration((hostContext, config) =>
            //    {
            //         ServiceLoader.Configuration = config.Build();
            //    })
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        services.AddOptions();
            //        ServiceLoader.Services = services;
            //        TianChengConfigureOptions cs = TianChengConfigureOptions.Instance;
            //        cs.AddDbServices();

            //        services.AddSingleton<IHostedService, MainRunService>();

            //        ServiceLoader.Instance = ServiceLoader.Services.BuildServiceProvider();
            //    });
            //await builder.RunConsoleAsync();
            //.ConfigureLogging();

#if RELEASE
            BenchmarkRunner.Run<ApiInsertMain>();
#endif

#if DEBUG

            var dal = new InsertMain();
            dal.单条guid主键插入Dapper_SQL();

            //var dal = new InsertMain();
            //dal.单条guid主键插入Dapper_SQL();
            //for (int i = 0; i < 1; i++)
            //{
            //    //Task.Run(() =>
            //    //{
            //    //var result =
            //    //dal.条件查询转对象();
            //    //var result = dal.条件查询数量();
            //    ////dal.分页查询();
            //    //Console.WriteLine($"item : {i}");
            //    //Console.WriteLine($"result : {result.ToJson()}");
            //    //});
            //}
#endif
            Console.WriteLine("操作完成");
            Console.ReadLine();
        }
    }
}
