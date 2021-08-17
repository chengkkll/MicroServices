using System;
using System.Linq;

namespace TianCheng.Common
{
    /// <summary>
    /// 获取当前应用程序的环境信息
    /// </summary>
    public class AppEnvironment
    {
        /// <summary>
        /// 获取Web程序的运行Uri地址
        /// </summary>
        public static Uri AppUri
        {
            get
            {
                return GetUri();
            }
        }
        //static private bool show = false;
        /// <summary>
        /// 获取当前应用程序运行的Uri地址
        /// </summary>
        /// <returns></returns>
        static private Uri GetUri()
        {
            string urls = GetEnvironmentUrls();

            if (string.IsNullOrWhiteSpace(urls))
            {
                urls = GetCommandUrls();
            }
            // todo:环境变量改成日志输入
            //Console.WriteLine(urls);
            //if (show == false)
            //{
            //    PrintEnvList();
            //    show = true;
            //}
            // 
            return new Uri(GetUrlAddress(urls));
        }
        /// <summary>
        /// 分析字符串并获取一个Url地址
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        static private string GetUrlAddress(string urls)
        {
            urls = urls.ToLower();
            if (urls.Contains(";"))
            {
                var urlarray = urls.Split(";", System.StringSplitOptions.RemoveEmptyEntries);
                urls = urlarray.Where(e => e.Contains("http")).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(urls))
                {
                    urls = urlarray.Where(e => e.Contains("https")).FirstOrDefault();
                }
            }
            return urls;
        }
        /// <summary>
        /// 在环境变量中获取urls
        /// </summary>
        /// <returns></returns>
        static private string GetEnvironmentUrls()
        {
            return System.Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
        }
        //static private void PrintEnvList()
        //{
        //    foreach (var key in System.Environment.GetEnvironmentVariables().Keys)
        //    {
        //        Console.WriteLine($"{key} ======> {System.Environment.GetEnvironmentVariable(key.ToString())}");
        //    }
        //}
        /// <summary>
        /// 在命令行中获取urls
        /// </summary>
        /// <returns></returns>
        static private string GetCommandUrls()
        {
            bool flag = false;
            foreach (var item in System.Environment.GetCommandLineArgs())
            {
                if (flag)
                {
                    return item;
                }
                flag = (item == "--urls");
            }
            return string.Empty;
        }
    }
}
