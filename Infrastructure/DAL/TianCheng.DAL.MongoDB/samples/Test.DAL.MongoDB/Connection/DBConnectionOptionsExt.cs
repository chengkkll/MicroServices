using System.Collections.Generic;
using System.Linq;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// 数据库链接配置
    /// </summary>
    static public class DBConnectionOptionsExt
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        static public string MongoConnectionString(this DBConnectionOptions options)
        {
            if (options.ServerAddress.EndsWith("/") || options.Database.StartsWith("/"))
            {
                return options.ServerAddress + options.Database;
            }
            return options.ServerAddress + "/" + options.Database;
        }

        static public string DefaultConnectionString(this List<DBConnectionOptions> options, out string server, out string database)
        {
            var confs = options.Where(o => o.Type == DBType.MongoDB);
            int count = confs.Count();
            if (count == 0)
            {
                throw new MongoConfigurationException();
            }
            if (count > 1)
            {
                var prov = confs.Where(e => e.Name == ConnectionProvider.DefaultOptionName).FirstOrDefault();
                if(prov != null)
                {
                    return LoadConnect(prov, out server, out database);
                }
            }
            return LoadConnect(confs.FirstOrDefault(),out server, out database);
        }

        static private string LoadConnect(DBConnectionOptions options, out string server, out string database)
        {
            if (options == null)
            {
                throw new MongoConfigurationException();
            }
            string conn = options.MongoConnectionString();
            if (string.IsNullOrWhiteSpace(conn))
            {
                throw new MongoConfigurationException();
            }
            server = options.ServerAddress;
            database = options.Database;
            return conn;
        }
    }
}
