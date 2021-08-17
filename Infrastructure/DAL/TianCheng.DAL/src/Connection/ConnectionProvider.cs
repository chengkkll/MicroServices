using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL
{
    /// <summary>
    /// 数据库连接支持
    /// </summary>
    public class ConnectionProvider : BaseConfigureService<List<DBConnectionOptions>>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="monitor"></param>
        public ConnectionProvider(IOptionsMonitor<List<DBConnectionOptions>> monitor) : base(monitor)
        {

        }
        /// <summary>
        /// 每次更新配置时，重新组装配置信息
        /// </summary>
        /// <param name="optons"></param>
        /// <returns></returns>
        protected override List<DBConnectionOptions> Assembling(List<DBConnectionOptions> optons)
        {
            foreach (var conn in optons)
            {
                conn.Name = conn.Name.ToLower();
            }
            return optons;
        }

        /// <summary>
        /// 默认的连接名称
        /// </summary>
        public const string DefaultOptionName = "default";

        #region Mongo
        /// <summary>
        /// 获取所有的Mongo连接
        /// </summary>
        public IEnumerable<DBConnectionOptions> MongoAll
        {
            get
            {
                return All(DBType.MongoDB);
            }
        }

        /// <summary>
        /// 获取Mongo的默认连接
        /// </summary>
        public DBConnectionOptions MongoDefault
        {
            get
            {
                return GetDefault(DBType.MongoDB);
            }
        }
        #endregion

        #region Redis
        /// <summary>
        /// 获取所有的Redis连接
        /// </summary>
        public IEnumerable<DBConnectionOptions> RedisAll
        {
            get
            {
                return All(DBType.Redis);
            }
        }

        /// <summary>
        /// 获取Redis的默认连接
        /// </summary>
        public DBConnectionOptions RedisDefault
        {
            get
            {
                var opt = GetDefault(DBType.Redis);
                if (opt == null)
                {
                    GlobalLog.Logger.Warning("没有找到Redis的连接配置");
                    return new DBConnectionOptions();
                }
                if (string.IsNullOrWhiteSpace(opt.ServerAddress))
                {
                    foreach (var item in opt.ConnectionString.Split(","))
                    {
                        if (item.Contains(":"))
                        {
                            UpdateServerPort(item, opt);
                            continue;
                        }
                        if (item.Contains("password"))
                        {
                            UpdatePassword(item, opt);
                        }
                    }
                }
                return opt;
            }
        }
        private void UpdateServerPort(string info, DBConnectionOptions options)
        {
            var s = info.Split(":");
            if (!string.IsNullOrWhiteSpace(s[0]))
            {
                options.Database = s[0];
            }
            if (s.Length >= 2 && !string.IsNullOrWhiteSpace(s[1]))
            {
                if (int.TryParse(s[1], out int p))
                {
                    options.Port = p;
                }
            }
        }
        private void UpdatePassword(string info, DBConnectionOptions options)
        {
            var s = info.Split("=");
            if (s[0].ToLower().Contains("password") && string.IsNullOrWhiteSpace(s[1]))
            {
                options.Password = s[1];
            }
        }
        #endregion

        #region Postgre
        /// <summary>
        /// 获取所有的Postgre连接
        /// </summary>
        public IEnumerable<DBConnectionOptions> PostgreAll
        {
            get
            {
                return All(DBType.PostgreSql);
            }
        }

        /// <summary>
        /// 获取Postgre的默认连接
        /// </summary>
        public DBConnectionOptions PostgreDefault
        {
            get
            {
                return GetDefault(DBType.PostgreSql);
            }
        }
        #endregion

        #region MySql
        /// <summary>
        /// 获取所有的MySql连接
        /// </summary>
        public IEnumerable<DBConnectionOptions> MySqlAll
        {
            get
            {
                return All(DBType.MySql);
            }
        }

        /// <summary>
        /// 获取MySql的默认连接
        /// </summary>
        public DBConnectionOptions MySqlDefault
        {
            get
            {
                return GetDefault(DBType.MySql);
            }
        }
        #endregion


        #region SqlServer
        /// <summary>
        /// 获取所有的MySql连接
        /// </summary>
        public IEnumerable<DBConnectionOptions> MsSqlAll
        {
            get
            {
                return All(DBType.MsSql);
            }
        }

        /// <summary>
        /// 获取MySql的默认连接
        /// </summary>
        public DBConnectionOptions MsSqlDefault
        {
            get
            {
                return GetDefault(DBType.MsSql);
            }
        }
        #endregion

        #region Core
        /// <summary>
        /// 根据数据库类型获取所有的连接信息
        /// </summary>
        /// <param name="dBType"></param>
        /// <returns></returns>
        public IEnumerable<DBConnectionOptions> All(DBType dBType)
        {
            if (Options != null)
            {
                GlobalLog.Logger.Information("找到连接信息：[{@Options}]", Options);
            }
            return Options.Where(o => o.Type == dBType);
        }

        /// <summary>
        /// 获取一个指定名称的连接
        /// </summary>
        /// <param name="dBType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DBConnectionOptions GetConnection(DBType dBType, string name)
        {
            return Options.Where(e => e.Name == name && e.Type == dBType).FirstOrDefault();
        }

        /// <summary>
        /// 获取一个数据库类型的默认连接
        /// </summary>
        /// <param name="dBType"></param>
        /// <returns></returns>
        public DBConnectionOptions GetDefault(DBType dBType)
        {
            IEnumerable<DBConnectionOptions> all = All(dBType);
            int count = all.Count();
            if (count == 0)
            {
                return null;
            }
            if (count == 1)
            {
                return all.FirstOrDefault();
            }
            return all.Where(e => e.Name == DefaultOptionName).FirstOrDefault() ?? all.FirstOrDefault();
        }
        #endregion
    }


}
