using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianCheng.Common;
using Microsoft.Extensions.Logging;
using System.Linq;
using MongoDB.Bson.Serialization;
using System.Linq.Expressions;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// MongoDB 数据库基础操作
    /// </summary>
    public partial class MongoOperation<T> : IDBOperation<T>, IDBOperation<T, ObjectId>, IMongoIdDBOperation<T>, IDBOperationRegister
        where T : MongoIdModel
    {
        #region 根据Id查询
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T SingleById(string id)
        {
            return SingleById(id.ToObjectId());
        }

        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <returns></returns>
        public T SingleById(ObjectId id)
        {
            return SafeDBOperation(() =>
            {
                var result = MongoCollection.FindAsync(new BsonDocument("_id", id)).Result;
                List<T> objList = result.ToList();
                if (objList.Count == 0)
                {
                    Log.LogWarning("按ObjectId查询时,无法找到对象。ObjectId：[{Id}]  查询对象类型：[{TypeName}]", id, typeof(T).FullName);
                }
                return objList.FirstOrDefault();
            });
        }

        /// <summary>
        /// 根据ID列表获取对象集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> SearchByIds(IEnumerable<string> ids)
        {
            return SafeDBOperation(() =>
            {
                IEnumerable<string> oids = ids.Select(e => $"ObjectId('{e}')");
                var query = new BsonDocument("_id", BsonSerializer.Deserialize<BsonDocument>("{'$in':[" + String.Join(",", oids) + "]}"));
                var result = MongoCollection.FindAsync(query).Result;
                List<T> objList = result.ToList();
                if (objList.Count == 0)
                {
                    Log.LogWarning("按ObjectId查询时,无法找到对象。ObjectId：[{Ids}]  查询对象类型：[{TypeName}]", ids, typeof(T).FullName);
                }
                return objList;
            });
        }
        #endregion

        #region 查看是否有重复项
        /// <summary>
        /// 查看指定属性值在表中是否有重复项
        /// </summary>
        /// <param name="id">当前对象ID</param>
        /// <param name="prop">属性名</param>
        /// <param name="val">属性值</param>
        /// <param name="ignoreDelete">是否忽略逻辑删除数据</param>
        /// <returns></returns>
        public bool HasRepeat(string id, string prop, string val, bool ignoreDelete = true)
        {
            return HasRepeat(id.ToObjectId(), prop, val, ignoreDelete);
        }

        /// <summary>
        /// 查看指定属性值在表中是否有重复项
        /// </summary>
        /// <param name="objectId">当前对象ID</param>
        /// <param name="prop">属性名</param>
        /// <param name="val">属性值</param>
        /// <param name="ignoreDelete">是否忽略逻辑删除数据</param>
        /// <returns></returns>
        public bool HasRepeat(ObjectId objectId, string prop, string val, bool ignoreDelete = true)
        {
            return SafeDBOperation(() =>
            {
                var builder = Builders<T>.Filter;
                FilterDefinition<T> filter = builder.Eq(prop, val);
                if (ignoreDelete)
                {
                    filter &= builder.Eq("IsDelete", false);
                }
                if (!(objectId == null || String.IsNullOrEmpty(objectId.ToString()) || objectId.Timestamp == 0 || objectId.Machine == 0 || objectId.Increment == 0))
                {
                    filter &= builder.Ne("_id", objectId);
                }
                return MongoCollection.CountDocuments(filter) > 0;
            });
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 获取当前集合的查询链式接口
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Queryable()
        {
            return MongoCollection.AsQueryable();
        }

        /// <summary>
        /// 将数据转成字典数据
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Dictionary<string, T> DataDict(Func<T, string> action)
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            foreach (var item in MongoCollection.AsQueryable())
            {
                string key = action.Invoke(item);
                if (dict.ContainsKey(key))
                {
                    continue;
                }
                dict.Add(key, item);
            }
            return dict;
        }

        /// <summary>
        /// 根据Mongodb的查询条件查询
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<T> Search(FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
        {
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }

            return SafeDBOperation(() =>
            {
                if (sort == null)
                {
                    return MongoCollection.Find(filter).ToListAsync().Result;
                }
                return MongoCollection.Find(filter).Sort(sort).ToListAsync().Result;
            });
        }

        /// <summary>
        /// 根据Mongodb的查询条件查询 ( 分页 )
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public List<T> Search(FilterDefinition<T> filter, QueryObject queryInfo)
        {
            SortDefinition<T> sort = "";
            FieldDefinition<T> field = queryInfo.Sort.Property;
            sort = Builders<T>.Sort.Combine(sort, queryInfo.Sort.IsAsc ? Builders<T>.Sort.Ascending(field) : Builders<T>.Sort.Descending(field));

            return SafeDBOperation(() =>
            {
                return MongoCollection.Find(filter).Sort(sort).ToListAsync().Result;
            });
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="filterFactory"></param>
        /// <returns></returns>
        public List<T> Search(Expression<Func<T, bool>> filterFactory)
        {
            return MongoCollection.AsQueryable().Where(filterFactory).ToList();
        }

        /// <summary>
        /// 根据实体来查询
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Search(T entity)
        {
            return SafeDBOperation(() =>
            {
                var query = MongoSerializer.SerializeQueryModel(entity);
                return MongoCollection.FindAsync(query).Result.ToEnumerable();
            });
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [Obsolete("尽量避免使用")]
        public List<T> Search()
        {
            return MongoCollection.AsQueryable().ToList();
        }
        #endregion

        #region 数据统计
        /// <summary>
        /// 通过Aggregate统计查询
        /// </summary>
        public List<R> Aggregate<R>(PipelineDefinition<T, R> pipeline)
        {
            var cursor = MongoCollection.AggregateAsync(pipeline).Result;
            var result = cursor.ToList();
            return result;
        }
        #endregion
    }
}
