using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianCheng.Common;
using Microsoft.Extensions.Logging;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// MongoDB 数据库基础操作
    /// </summary>
    public partial class MongoOperation<T> : IDBOperation<T>, IDBOperation<T, ObjectId>, IMongoIdDBOperation<T>, IDBOperationRegister
        where T : MongoIdModel
    {
        #region 删除表（集合）
        /// <summary>
        /// 删除表（集合）
        /// </summary>
        public void Drop()
        {
            Log.LogTrace("删除集合 准备删除==> 集合名称：[{CollectionName}]\t对应类型：[{TypeName}] ", Provider.CollectionName, typeof(T).FullName);
            SafeDBOperation(() =>
            {
                Provider.Database.DropCollection(Provider.CollectionName);
                Log.LogWarning("删除集合 已删除  ==> 集合名称：[{CollectionName}]\t对应类型：[{TypeName}] ", Provider.CollectionName, typeof(T).FullName);
            });
        }

        /// <summary>
        /// 删除表（集合） 异步
        /// </summary>
        /// <returns></returns>
        public async void DropAsync()
        {
            Log.LogTrace("异步删除集合 准备删除==> 集合名称：[{CollectionName}]\t对应类型：[{TypeName}] ", Provider.CollectionName, typeof(T).FullName);

            await Task.Run(() =>
            {
                SafeDBOperation(() =>
                {
                    Provider.Database.DropCollectionAsync(Provider.CollectionName);
                    Log.LogWarning("异步删除集合 已删除  ==> 集合名称：[{CollectionName}]\t对应类型：[{TypeName}] ", Provider.CollectionName, typeof(T).FullName);
                });
            });
        }
        #endregion
    }
}
