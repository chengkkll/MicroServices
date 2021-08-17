using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace MongoDBBenchmark.Benchmark
{
    [SimpleJob(targetCount: 1000)]
    public class SearchMain
    {
        [Benchmark]
        public void 查询所有数据()
        {
            //var dal = ServiceLoader.GetService<DemoDAL>();
            DemoDAL dal = new DemoDAL();
            int count = dal.Count();
            Console.WriteLine(count);
        }

        [Benchmark]
        public void Mongo统计()
        {
            //var dal = ServiceLoader.GetService<DemoDAL>();
            DemoDAL dal = new DemoDAL();
            Console.WriteLine(dal.Count2());
        }

        //[Benchmark]
        //public void 字典查询()
        //{
        //    //var dal = ServiceLoader.GetService<DemoDAL>();
        //    DemoDAL2 dal = new DemoDAL2();
        //    int count = dal.Count();
        //    Console.WriteLine(count);
        //}
    }
}
