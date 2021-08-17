using BenchmarkDotNet.Attributes;
using NpgBenchmark.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NpgBenchmark.Model;
using TianCheng.DAL.NpgByDapper;
using TianCheng.DAL;
using TianCheng.Common;

namespace NpgBenchmark.Benchmark
{
    [SimpleJob(targetCount: 1000)]
    public class DeleteMain
    {
        static public MockIntDal IntDal = new MockIntDal();
        static public MockGuidDAL GuidDal = new MockGuidDAL();

        static public Queue<Guid> idList = new Queue<Guid>();
        static public Queue<Guid> delList = new Queue<Guid>();

        public DeleteMain()
        {
            ReadJson.Init();
            // 设置AutoMapper映射信息
            AutoMapperExtension.RegisterAutoMapper();

            foreach (var id in GuidDal.SearchId(new GuidQuery() { Page = new QueryPagination() { Index = 0, Size = 6000 } }))
            {
                idList.Enqueue(id);
            }

            foreach (var id in GuidDal.SearchId(new GuidQuery() { IsDelete = true, Page = new QueryPagination { Index = 0, Size = 300 } }))
            {
                delList.Enqueue(id);
            }
        }

        public Guid RemoveId
        {
            get
            {
                if (idList.Count == 0)
                {
                    foreach (var id in GuidDal.SearchId(new GuidQuery() { Page = new QueryPagination() { Index = 0, Size = 100 } }))
                    {
                        idList.Enqueue(id);
                    }
                }
                return idList.Peek();
            }
        }

        [Benchmark]
        public void 物理删除_单个()
        {
            GuidDal.RemoveByTypeId(RemoveId);
        }

        [Benchmark]
        public void 物理删除_多个()
        {
            GuidDal.RemoveByTypeIdList(new List<Guid>() { RemoveId, RemoveId, RemoveId });
        }

        [Benchmark]
        public void 清空表()
        {
            GuidDal.Drop();
        }

        //[Benchmark]
        //public void 逻辑删除_单个()
        //{
        //    GuidDal.DeleteByTypeId(idList.Dequeue());
        //}

        //[Benchmark]
        //public void 逻辑删除_多个()
        //{
        //    List<Guid> list = new List<Guid>
        //    {
        //        idList.Dequeue(),
        //        idList.Dequeue(),
        //        idList.Dequeue()
        //    };

        //    GuidDal.DeleteByTypeIdList(list);
        //    Console.WriteLine(list.ToJson());
        //}

        //[Benchmark]
        //public void 恢复删除_单个()
        //{
        //    GuidDal.UndeleteByTypeId(delList.Dequeue());
        //}

        //[Benchmark]
        //public void 恢复删除_多个()
        //{
        //    List<Guid> list = new List<Guid>
        //    {
        //        delList.Dequeue(),
        //        delList.Dequeue(),
        //        delList.Dequeue()
        //    };

        //    GuidDal.UndeleteByTypeIdList(list);
        //    Console.WriteLine(list.ToJson());
        //}
    }
}
