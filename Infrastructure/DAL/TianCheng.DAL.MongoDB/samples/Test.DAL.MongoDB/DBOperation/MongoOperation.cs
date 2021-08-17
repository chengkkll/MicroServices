using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TianCheng.Common;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoOperation<T> : MongoOperation<T, QueryObject>
        where T : MongoIdModel
    {

    }

    public partial class MongoO<T>
    {
        protected IMongoCollection<T> MongoCollection { get; private set; }

        static private Dictionary<string, IMongoCollection<T>> ProviderDict = new Dictionary<string, IMongoCollection<T>>();

        static MongoO()
        {
            string ConnectString = "mongodb://localhost:27017";
            string database = "samples";

            var client = new MongoClient(ConnectString);
            var db = client.GetDatabase(database);
            ProviderDict.Add("test_demo", db.GetCollection<T>("test_demo"));
        }

        public MongoO()
        {
            DBMappingAttribute attribute = this.GetType().GetCustomAttribute<DBMappingAttribute>(false);
            MongoCollection = ProviderDict[attribute.CollectionName];
        }

        /// <summary>
        /// 获取当前集合的查询链式接口
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Queryable()
        {
            return MongoCollection.AsQueryable();
        }

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Queryable().Count();
        }
    }
    /// <summary>
    /// MongoDB 持久化操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public partial class MongoOperation<T, Q>
        //: IDBOperationRegister
        //IMongoDBOperation<T>,
        //IDBOperationFilter<T, Q>
        where Q : QueryObject
        where T : MongoIdModel
    {
        #region 构造
        /// <summary>
        /// 表（集合）
        /// </summary>
        protected IMongoCollection<T> MongoCollection { get; private set; }
        /// <summary>
        /// 数据库操作服务
        /// </summary>
        protected MongoProvider Provider { get; private set; }

        ///// <summary>
        ///// 日志操作对象
        ///// </summary>
        //private Serilog.ILogger logger = null;
        ///// <summary>
        ///// 日志操作对象
        ///// </summary>
        //private Serilog.ILogger Logger
        //{
        //    get
        //    {
        //        if (logger == null)
        //        {
        //            logger = MongoLog.Logger.ForContext(this.GetType());
        //        }
        //        return logger;
        //    }
        //}
        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;

        protected string ConnectString { get; private set; }
        /// <summary>
        /// 构造方法处理
        /// </summary>
        public MongoOperation()
        {
            //Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(GetType());
            //// 数据库连接修改事件
            //MongoConnection.ConnectionChange = OnConnectionChange;
            //// 获取操作当前数据集合的服务
            //Provider = MongoConnection.GetProvider(this.GetType().FullName);
            //// 获取一个表（集合）操作
            //MongoCollection = Provider.Collection;

            //attribute.DALTypeName = type.FullName;
            // attribute.CollectionName

            //var connProv = ServiceLoader.GetService<ConnectionProvider>();
            //string ConnectString = connProv.Options.DefaultConnectionString(out string server, out string database);

            string ConnectString = "mongodb://localhost:27017";
            string database = "samples";

            var client = new MongoClient(ConnectString);
            var db = client.GetDatabase(database);
            DBMappingAttribute attribute = this.GetType().GetCustomAttribute<DBMappingAttribute>(false);
            MongoCollection = db.GetCollection<T>(attribute.CollectionName);
        }
        /// <summary>
        /// 数据库连接修改事件
        /// </summary>
        public void OnConnectionChange()
        {
            // 重新获取操作当前数据集合的服务
            //Provider = MongoConnection.GetProvider(this.GetType().FullName);
        }
        #endregion

        #region 数据库操作  统一异常处理
        /// <summary>
        /// 安全的数据库操作处理
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public R SafeDBOperation<R>(Func<R> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (TimeoutException te)
            {
                throw new MongoConnectionException(te, Provider.Connection.ConnectionString);
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is TimeoutException)
                    {
                        throw new MongoConnectionException(x, Provider.Connection.ConnectionString);
                    }
                    return false;
                });
                throw;
            }
            catch (Exception ex)
            {
                throw new MongoOperationException(ex);
            }
        }

        /// <summary>
        /// 安全的数据库操作处理
        /// </summary>
        /// <param name="action"></param>
        public void SafeDBOperation(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (TimeoutException te)
            {
                throw new MongoConnectionException(te, Provider.Connection.ConnectionString);
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is TimeoutException)
                    {
                        throw new MongoConnectionException(x, Provider.Connection.ConnectionString);
                    }
                    return false;
                });
                throw;
            }
            catch (Exception ex)
            {
                throw new MongoOperationException(ex);
            }
        }
        #endregion

        /// <summary>
        /// 获取当前集合的查询链式接口
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Queryable()
        {
            return MongoCollection.AsQueryable();
        }

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Queryable().Count();
        }


        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <returns></returns>
        public long Count2()
        {
            
            FilterDefinition<T> filter = Builders<T>.Filter.Empty;
            return MongoCollection.CountDocuments(filter);
        }
    }
}
