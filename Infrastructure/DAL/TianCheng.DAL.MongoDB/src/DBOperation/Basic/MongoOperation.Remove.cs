using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianCheng.Common;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// MongoDB 数据库基础操作
    /// </summary>
    public partial class MongoOperation<T> : IDBOperation<T>, IDBOperation<T, ObjectId>, IMongoIdDBOperation<T>, IDBOperationRegister
        where T : MongoIdModel
    {
        #region 物理删除
        /// <summary>
        ///  将对象内容作为查询条件来物理删除数据 
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveSearch(T entity)
        {
            Log.LogTrace("物理删除数据 ==> 类型：[{TypeName}]\r\n数据信息为：[{Entity}] ", typeof(T).FullName, entity.ToJson());

            SafeDBOperation(() =>
            {
                var query = MongoSerializer.SerializeQueryModel(entity);
                DeleteResult result = MongoCollection.DeleteManyAsync(query).Result;
                if (result.DeletedCount == 0)
                {
                    Log.LogTrace("以对象内容作为查询条件来物理删除数据 ==> 无数据被删除。\r\n 类型：[{TypeName}]\r\n删除条件为：[{FilterObject}] ", typeof(T).FullName, entity.ToJson());
                    return;
                }
                Log.LogTrace("以对象内容作为查询条件来物理删除数据 ==> 已删除数据：{DeletedCount}条\r\n 类型：[{TypeName}]\r\n删除条件为：[{FilterObject}] ", result.DeletedCount, typeof(T).FullName, entity.ToJson());
            });
        }

        /// <summary>
        ///  物理删除对象 
        /// </summary>
        /// <param name="entity"></param>
        public bool RemoveObject(T entity)
        {
            return RemoveObject(entity.Id);
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool RemoveMany(IEnumerable<string> ids)
        {
            List<ObjectId> objIdList = new List<ObjectId>();
            foreach (string id in ids)
            {
                if (!ObjectId.TryParse(id, out ObjectId objId))
                {
                    // 做记录，物理删除的ID不存在
                    Log.LogError("按ID物理删除时参数错误，无法转换成ObjectId的Id值为：[{Id}]\r\nID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", id, ids.ToJson(), typeof(T).FullName);
                    continue;
                }
                objIdList.Add(objId);
            }

            return RemoveMany(objIdList);
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <returns></returns>
        public bool RemoveMany(IEnumerable<ObjectId> ids)
        {
            var filter = Builders<T>.Filter.AnyIn("_id", ids);
            return SafeDBOperation(() =>
            {
                DeleteResult result = MongoCollection.DeleteManyAsync(filter).Result;
                if (result.DeletedCount != ids.Count())
                {
                    // 没有完全删除，做日志记录
                    Log.LogWarning("根据ID列表 物理删除一组数据 ==> 已删除数据：{DeletedCount}条,按条件应删除{PlanCount}条\r\n 类型：[{TypeName}]\r\n查询条件为：[{FilterObject}] ", result.DeletedCount, ids.Count(), typeof(T).FullName, ids.ToJson());
                    return false;
                }
                return true;
            });
        }


        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public bool RemoveObject(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objId))
            {
                Log.LogError("按ID物理删除时参数错误，无法转换成ObjectId的Id值为：[{Id}]\r\n类型为：[{TypeName}]", id, typeof(T).FullName);
                return false;
            }
            return RemoveObject(objId);
        }

        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public bool RemoveObject(ObjectId id)
        {
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                return SafeDBOperation(() =>
                {
                    T result = MongoCollection.FindOneAndDeleteAsync(filter).Result;
                    return true;
                });
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "按ID物理删除时错误。Id值为：[{Id}]\r\n类型为：[{TypeName}]", id.ToString(), typeof(T).FullName);
                throw;
            }
        }
        #endregion
    }
}
