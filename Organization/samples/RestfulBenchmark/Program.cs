using BenchmarkDotNet.Running;
using RestfulBenchmark.OrgApi;
using System;
using System.Threading.Tasks;

namespace RestfulBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            BenchmarkRunner.Run<LoginApi>();
#endif

#if DEBUG
            LoginApi api = new LoginApi();
            api.登录();
#endif
        }
    }
}