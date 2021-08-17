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
    /// 
    /// </summary>
    public class InitDBService : IBusinessService, IServiceRegister
    {
        private string BasePath { get; }

        /// <summary>
        /// 
        /// </summary>
        public InitDBService()
        {
            BasePath = AppContext.BaseDirectory.ToLower().Contains("bin\\debug") ? Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\") : AppContext.BaseDirectory;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public ResultView InitSql(string connection)
        {
            CreateDB(connection);
            InitDB(connection);
            return ResultView.Success();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection">不用加数据库名，类似："Host=192.168.1.22;Port=59432;User ID=tc;Password=123qwe;"</param>
        /// <returns></returns>
        public void CreateDB(string connection)
        {
            string path = Path.Combine(BasePath, "ServicesConfigure", "sql", "create_db.sql");

            NpgSqlOperation.RunSqlFile(path, connection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection">需要加数据库名，类似："Host=192.168.1.22;Port=59432;User ID=tc;Password=123qwe;Database=tc_test"</param>
        public void InitDB(string connection)
        {
            string path = Path.Combine(BasePath, "ServicesConfigure", "sql", "init_db.sql");
            // 获取数据库的名称，并根据数据库的名称更新链接字符串
            if (!connection.ToLower().Contains("database"))
            {
                string dbName = string.Empty;
                foreach (string line in File.ReadAllLines(path))
                {
                    if (line.StartsWith("-- Database:"))
                    {
                        dbName = line.Replace("-- Database:", "").Replace("/r", "").Replace("/n", "");
                        break;
                    }
                }
                connection = $"{connection} Database={dbName};";
            }
            
            NpgSqlOperation.RunSqlFile(path, connection);
        }
    }
}
