using BenchmarkDotNet.Attributes;
using MongoDBBenchmark;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDBBenchmark.Benchmark
{
    [SimpleJob(targetCount: 100)]
    public class InsertMain
    {
        [Benchmark]
        public void 插入一条数据()
        {
            TianCheng.Common.LoadRestfulApi.Get<IEnumerable<DemoView>>("https://localhost:5001/api/values/default/append");
        }
    }
}
