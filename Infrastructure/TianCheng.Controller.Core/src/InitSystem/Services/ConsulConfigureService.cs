using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TianCheng.Common;
using TianCheng.Service.Core;
using Flurl.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TianCheng.DAL.NpgSqlByEF;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 配置中心服务
    /// </summary>
    public class ConsulConfigureService : IBusinessService, IServiceRegister
    {
        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public async Task<ResultView> InitConsulKvAsync()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "ServicesConfigure");
            if (AppContext.BaseDirectory.ToLower().Contains("bin\\debug"))
            {
                path = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "ServicesConfigure");
            }

            ConsulOptions consulOptions = ServiceLoader.GetService<IConsulConfigurationService>().Options;

            await "http://localhost:8500/v1/kv/ServicesConfig/".PutAsync();
            await "http://localhost:8500/v1/kv/ServicesConfig/log.json".PutStringAsync(File.ReadAllText(Path.Combine(path, "log.json")));
            await "http://localhost:8500/v1/kv/ServicesConfig/auth.json".PutStringAsync(File.ReadAllText(Path.Combine(path, "auth.json")));
            await $"http://localhost:8500/v1/kv/ServicesConfig/{consulOptions.ServiceName}/".PutAsync();
            await $"http://localhost:8500/v1/kv/ServicesConfig/{consulOptions.ServiceName}/appsettings.json".PutStringAsync(File.ReadAllText(Path.Combine(path, consulOptions.ServiceName, "appsettings.json")));
            await $"http://localhost:8500/v1/kv/ServicesConfig/{consulOptions.ServiceName}/appsettings.Development.json".PutStringAsync(File.ReadAllText(Path.Combine(path, consulOptions.ServiceName, "appsettings.Development.json")));
            await $"http://localhost:8500/v1/kv/ServicesConfig/{consulOptions.ServiceName}/appsettings.Production.json".PutStringAsync(File.ReadAllText(Path.Combine(path, consulOptions.ServiceName, "appsettings.Production.json")));

            return ResultView.Success(path);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public ResultView InitSql()
        //{
        //    var path = Path.Combine(AppContext.BaseDirectory, "ServicesConfigure", "init_db.sql");
        //    if (AppContext.BaseDirectory.ToLower().Contains("bin\\debug"))
        //    {
        //        path = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "ServicesConfigure", "init_db.sql");
        //    }

        //    NpgSqlOperation.Init(path, "Host=192.168.1.22;Port=59432;User ID=tc;Password=123qwe;", "wcard_org");

        //    return ResultView.Success();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="connection"></param>
        ///// <returns></returns>
        //public ResultView CreateDB(string connection)
        //{
        //    string path;
        //    if (AppContext.BaseDirectory.ToLower().Contains("bin\\debug"))
        //    {
        //        path = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "ServicesConfigure", "sql", "create_db.sql");
        //    }
        //    else
        //    {
        //        path = Path.Combine(AppContext.BaseDirectory, "ServicesConfigure", "sql", "create_db.sql");
        //    }

        //    NpgSqlOperation.RunSqlFile(path, connection);

        //    return ResultView.Success();
        //}
    }
}
