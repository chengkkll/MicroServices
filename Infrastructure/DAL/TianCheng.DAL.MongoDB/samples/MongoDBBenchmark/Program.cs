using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TianCheng.DAL;
using TianCheng.Common;
using MongoDBBenchmark.Benchmark;
using BenchmarkDotNet.Running;

namespace MongoDBBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {

#if RELEASE
            //var conf = new ConfigurationBuilder().TianChengConfiguration();
            //IServiceCollection services = new ServiceCollection();
            //services.AddTianChengCommon(conf, options =>
            //{
            //    options.AddDbServices();
            //});
            //IServiceProvider provider = services.BuildServiceProvider();

            //ServiceLoader.Instance = provider;

            BenchmarkRunner.Run<SearchMain>();
#endif

#if DEBUG

            #region 初始化配置
            var conf = new ConfigurationBuilder().TianChengConfiguration();
            IServiceCollection services = new ServiceCollection();
            services.AddTianChengCommon(conf, options =>
            {
                options.AddDbServices();
            });
            services.AddSingleton<InsertMain>();
            services.AddSingleton<DemoDAL>();
            //TianChengConfigureOptions cs = TianChengConfigureOptions.Instance;
            //cs.AddDbServices();

            IServiceProvider provider = services.BuildServiceProvider();

            ServiceLoader.Instance = provider;


            #endregion
            //var service = ServiceLoader.GetService<SearchMain>();
            var dal = ServiceLoader.GetService<DemoDAL>();
            int count = dal.Count();
            Console.WriteLine(count);

            //var data = service.查询所有数据();
            //Console.WriteLine(data.ToJson());
#endif


            Console.WriteLine("操作完成");
            Console.ReadKey();
        }
    }
}
