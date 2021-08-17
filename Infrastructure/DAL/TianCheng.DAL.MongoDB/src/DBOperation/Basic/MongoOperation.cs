using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Reflection;
using TianCheng.Common;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// MongoDB 数据库基础操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class MongoOperation<T> : IDBOperation<T>, IDBOperation<T, ObjectId>, IMongoIdDBOperation<T>, IDBOperationRegister
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

        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;

        /// <summary>
        /// 构造方法处理
        /// </summary>
        public MongoOperation()
        {
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(GetType());
            // 数据库连接修改事件
            MongoConnection.ConnectionChange = OnConnectionChange;
            // 获取操作当前数据集合的服务
            Provider = MongoConnection.GetProvider(this.GetType().FullName);
            // 获取一个表（集合）操作
            //MongoCollection = Provider.Collection;

            var connectionProvider = ServiceLoader.GetService<ConnectionProvider>();
            string ConnectString = connectionProvider.Options.DefaultConnectionString(out string server, out string database);

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
            Provider = MongoConnection.GetProvider(this.GetType().FullName);
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
    }
}
