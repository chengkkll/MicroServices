using BenchmarkDotNet.Attributes;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TianCheng.Organization.Model;
using TianCheng.Services.AuthJwt;

namespace RestfulBenchmark.OrgApi
{
    /// <summary>
    /// 登录Api测试
    /// </summary>
    [SimpleJob(targetCount: 2)]
    public class LoginApi
    {
        // 测试账号列表
        private static readonly List<LoginView> TestList = new List<LoginView>();

        private static string ApiUrl { get; } = "http://127.0.0.1:5056";

        private static string ApiAddress_Login { get; } = "/api/org/Auth/login";
        static LoginApi()
        {
            TestList.Add(new LoginView() { Account = "a", Password = "a" });
            TestList.Add(new LoginView() { Account = "a", Password = "b" });
        }

        private readonly Random random = new Random();
        private LoginView LoginPostObject
        {
            get
            {
                return TestList[random.Next(0, TestList.Count)];
            }
        }
        private static string ApiAddress_GetFun { get; } = "/api/org/Function";

        private static readonly string Token = " eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjVmOTQ2MzAxNWZjMGZjMmE5NDI1ZmJjNyIsIm5hbWUiOiLpooTliLbnrqHnkIblkZgiLCJyb2xlSWQiOiI1Zjk0NjMwMTVmYzBmYzJhOTQyNWZiZDAiLCJkZXBJZCI6IiIsImRlcE5hbWUiOiIiLCJ3b3Jrc3RhdGlvbiI6IiIsImF1dGgiOiJUaWFuQ2hlbmcuT3JnYW5pemF0aW9uLlNlcnZpY2VzLlRBdXRoU2VydmljZSIsInN1YiI6ImEiLCJqdGkiOiJkMGY0NzM0ZS03MTVmLTQ1NjgtOWI4NC0wNWY3MmJmMjJjMGMiLCJpYXQiOiIyMDIwLzExLzkgMTM6NTg6MjkiLCJuYmYiOjE2MDQ5MzAzMDksImV4cCI6MTYwNTAxNjcwOSwiaXNzIjoiVGlhbkNoZW5nLmlzc3VlciIsImF1ZCI6IlRpYW5DaGVuZy5hdWRpIn0.pbuEFaMowUsDKxhnhDCEvq587ATCqN93Aa3KgR7qiSM";


        /// <summary>
        /// 异常捕捉的处理
        /// </summary>
        /// <param name="action"></param>
        public void RunAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (FlurlHttpTimeoutException)
            {
                // handle timeout
            }
            catch (FlurlHttpException ex)
            {
                Console.WriteLine($"请求失败! [{ex.Call.Response.StatusCode}] Url:{ex.Call.Request.Url}");
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is FlurlHttpException)
                    {
                        FlurlHttpException ex = x as FlurlHttpException;
                        Console.WriteLine($"请求失败! {ex.Call.Response.StatusCode}");
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求失败! {ex.Message},{ex.GetType()}");
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        [Benchmark]
        public void 登录()
        {
            //TianCheng.Common.LoadRestfulApi.Post("https://localhost:5001/MockGuid");
            RunAction(() =>
            {
                var response = ApiUrl
                .AppendPathSegment(ApiAddress_Login)
                //.SetQueryParams(new { a = 1, b = 2 })
                //.WithOAuthBearerToken("my_oauth_token")
                .PostJsonAsync(LoginPostObject).Result;
                //.ReceiveJson<TokenResult>();

                if (response.StatusCode < 300)
                {
                    var result = response.GetJsonAsync<TokenResult>();
                    Console.WriteLine($"Success! {result.Result.Token}");
                }
                else
                {
                    Console.WriteLine($"请求失败! {response.StatusCode}");
                }
            });
        }


        /// <summary>
        /// 获取权限
        /// </summary>
        [Benchmark]
        public void 获取权限()
        {
            RunAction(() =>
            {
                var response = ApiUrl
                .AppendPathSegment(ApiAddress_GetFun)
                .WithOAuthBearerToken(Token)
                .GetJsonAsync().Result;

                if (response.StatusCode < 300)
                {
                    var result = response.GetJsonAsync<FunctionModuleView>();
                    Console.WriteLine($"Success! {result.Result.Code}");
                }
                else
                {
                    Console.WriteLine($"请求失败! {response.StatusCode}");
                }
            });
        }
    }
}
