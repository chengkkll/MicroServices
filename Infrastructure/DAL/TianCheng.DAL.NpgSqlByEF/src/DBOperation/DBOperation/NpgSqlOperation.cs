using Microsoft.Extensions.Logging;
using TianCheng.DAL;
using TianCheng.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.IO;

namespace TianCheng.DAL.NpgSqlByEF
{
    public class NpgSqlOperation : IDBOperationRegister
    {
        #region 构造方法
        /// <summary>
        /// 构造函数注入
        /// </summary>
        public NpgSqlOperation()
        {

        }
        #endregion


        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="file"></param>
        /// <param name="connection"></param>
        /// <param name="dbName">如果填写数据库名称，会重新删除数据库并新建</param>
        static public void Init(string file, string connection, string dbName = null)
        {
            try
            {
                // 创建数据库
                if (!string.IsNullOrWhiteSpace(dbName))
                {
                    using NpgSqlContext c = new NpgSqlContext();
                    c.Database.GetDbConnection().ConnectionString = connection;// "Host=192.168.1.22;Port=59432;User ID=tc;Password=123qwe;";
                    string createDB = $"DROP DATABASE if exists {dbName};" +
                        $"CREATE DATABASE {dbName}" +
                        @"      WITH 
                                OWNER = tc
                                ENCODING = 'UTF8'
                                LC_COLLATE = 'en_US.utf8'
                                LC_CTYPE = 'en_US.utf8'
                                TABLESPACE = pg_default
                                CONNECTION LIMIT = -1;";
                    c.Database.ExecuteSqlRaw(createDB);

                    connection = $"{connection} Database={dbName};";
                }
                // 加载sql文件并执行
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                string sql = File.ReadAllText(file, Encoding.GetEncoding("GB2312"));
                using NpgSqlContext d = new NpgSqlContext();
                d.Database.GetDbConnection().ConnectionString = connection;
                d.Database.ExecuteSqlRaw(sql);
            }
            catch (Exception ex)
            {
                TianCheng.Common.GlobalLog.Logger.Error(ex, "初始化sql错误。[{@InitSqlPath}]", file);
                throw;
            }
        }


        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="sqlFile">脚本文件路径</param>
        /// <param name="connection">数据库连接</param>
        static public void RunSqlFile(string sqlFile, string connection)
        {
            try
            {
                // 加载sql文件
                string sql = File.ReadAllText(sqlFile);
                using NpgSqlContext c = new NpgSqlContext();
                c.Database.GetDbConnection().ConnectionString = connection;
                // 执行脚本文件
                c.Database.ExecuteSqlRaw(sql);
            }
            catch (Exception ex)
            {
                GlobalLog.Logger.Error(ex, "执行数据库脚本文件错误。[{@SqlFilePath}]", sqlFile);
                throw;
            }
        }
    }


    /// <summary>
    /// NpgSql 数据库通用操作基类
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public partial class NpgSqlOperation<C, T, Q, IdType> :
        IDBOperation<T>,
        IDBOperation<T, IdType>,
        INpgSqlOperation<T, IdType>,
        INpgSqlOperationFilter<T, Q>,
        IDBOperationRegister,
        IDBOperationFilter<T, Q>
        where C : NpgSqlContext, new()
        where Q : QueryObject
        where T : class, IIdModel<IdType>, new()
    {
        #region 构造方法
        /// <summary>
        /// 数据库操作上下文
        /// </summary>
        public C Context { get; private set; }

        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        public NpgSqlOperation(C context)
        {
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(this.GetType());
            Context = context;
            // 取消跟踪
            Context.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;

            // 目前只有一个数据库，所以在这里做统一设置
            Context.ToMaster();
        }
        #endregion
    }
}
