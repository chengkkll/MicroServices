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
        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Empty;
            return MongoCollection.CountDocuments(filter);
        }
    }
}
