using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NpgBenchmark.Benchmark
{
    [SimpleJob(targetCount: 100)]
    public class ApiInsertMain
    {
        [Benchmark]
        public void 插入一条数据()
        {
            TianCheng.Common.LoadRestfulApi.Post("https://localhost:5001/MockGuid");
        }
    }
}
