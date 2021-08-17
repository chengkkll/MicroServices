using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgSqlByEF
{
    /// <summary>
    /// NpgSql 的数据上下文对象
    /// </summary>
    public class NpgSqlContext : DbContext
    {
        /// <summary>
        /// 默认使用主库
        /// </summary>
        public void ToMaster()
        {
            ConnectionProvider connectionProvider = ServiceLoader.GetService<ConnectionProvider>();
            // Master Connection
            Database.GetDbConnection().ConnectionString = connectionProvider.PostgreDefault.ConnectionString;
        }
        /// <summary>
        /// 操作日志只在控制台输入出，为了方便调试
        /// </summary>
        public static readonly ILoggerFactory NpgLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        /// <summary>
        /// 数据库配置
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionProvider connectionProvider = ServiceLoader.GetService<ConnectionProvider>();

            // 数据库配置
            optionsBuilder.UseNpgsql(connectionProvider.PostgreDefault.ConnectionString);

            // 开发模式在控制台输出日志
            if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                optionsBuilder.UseLoggerFactory(NpgLoggerFactory);
            }
            // 取消跟踪
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 映射文件配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 自动注册映射的文件
            foreach (Type type in AssemblyHelper.GetTypeByInterface<INpgSqlConfiguration>())
            {
                INpgSqlConfiguration map = Activator.CreateInstance(type) as INpgSqlConfiguration;
                map.Configure(modelBuilder);
            }
        }
    }
}
