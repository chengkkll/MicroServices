using BenchmarkDotNet.Attributes;
using NpgBenchmark.DAL;
using NpgBenchmark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.DAL;
using TianCheng.DAL.NpgByDapper;
using TianCheng.Common;

namespace NpgBenchmark.Benchmark
{
    [SimpleJob(targetCount: 1000)]
    public class SearchIdMain
    {
        static public MockIntDal IntDal = new MockIntDal();
        static public MockGuidDAL GuidDal = new MockGuidDAL();
        private static readonly Random random = new Random();
        static public IEnumerable<Guid> data;

        static private Guid GetId()
        {
            return data.ElementAt(random.Next(0, data.Count()));
        }

        public SearchIdMain()
        {
            ReadJson.Init();
            // 设置AutoMapper映射信息
            AutoMapperExtension.RegisterAutoMapper();

            data = GuidDal.SearchId(new GuidQuery() { Page = new QueryPagination() { Index = 0, Size = 5000 } });
        }

        [Benchmark]
        public Guid 获取随机Id()
        {
            return GetId();
        }

        [Benchmark]
        public MockGuidDB 根据Id查询()
        {
            return GuidDal.SingleById(GetId());
        }

        [Benchmark]
        public MockGuidInfo 根据Id查询后转换()
        {
            return GuidDal.SingleDOById(GetId());
        }
    }
}
