using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianCheng.Common;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// MongoDB 数据库基础操作
    /// </summary>
    public partial class MongoOperation<T> : IDBOperation<T>, IDBOperation<T, ObjectId>, IMongoIdDBOperation<T>, IDBOperationRegister
        where T : MongoIdModel
    {
        #region Save
        /// <summary>
        /// 保存对象，根据ID是否为空来判断是新增还是修改操作
        /// </summary>
        /// <param name="entity"></param>
        public void Save(T entity)
        {
            if (entity.IdEmpty)
            {
                InsertObject(entity);
            }
            else
            {
                UpdateObject(entity);
            }
        }
        #endregion

        #region 数据插入
        /// <summary>
        /// 插入单条新数据
        /// </summary>
        /// <param name="entity"></param>
        public void InsertObject(T entity)
        {
            Log.LogTrace("插入数据 ==> 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] ", typeof(T).FullName, entity);
            SafeDBOperation(() =>
            {
                MongoCollection.InsertOne(entity);
            });
        }

        /// <summary>
        /// 插入多条新数据
        /// </summary>
        /// <param name="entities"></param>
        public void InsertMany(IEnumerable<T> entities)
        {
            Log.LogTrace("插入多条数据 ==> 类型：[{TypeName}]\r\n数据信息为：[{@Entities}] ", typeof(T).FullName, entities);
            SafeDBOperation(() =>
            {
                MongoCollection.InsertMany(entities);
            });
        }

        #region 异步插入操作
        /// <summary>
        /// 插入单条数据  异步   （据说异步写入数据比同步的慢，有待测试）
        /// </summary>
        /// <param name="entity"></param>
        public async void InsertAsync(T entity)
        {
            Log.LogTrace("异步插入数据 ==> 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] ", typeof(T).FullName, entity);

            await Task.Run(() =>
            {
                SafeDBOperation(() =>
                {
                    MongoCollection.InsertOneAsync(entity);
                });
            });
        }

        /// <summary>
        /// 插入多条数据 异步   （据说异步写入数据比同步的慢，有待测试）
        /// </summary>
        /// <param name="entities"></param>
        public async void InsertAsync(IEnumerable<T> entities)
        {
            Log.LogTrace("异步插入多条数据 ==> 类型：[{TypeName}]\r\n数据信息为：[{@Entities}] ", typeof(T).FullName, entities);
            await Task.Run(() =>
            {
                SafeDBOperation(() =>
                {
                    MongoCollection.InsertManyAsync(entities);
                });
            });
        }
        #endregion

        #endregion

        #region 数据更新
        /// <summary>
        /// 更新单条数据 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateObject(T entity)
        {
            Log.LogTrace("更新单条数据 ==> 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] ", typeof(T).FullName, entity);

            ReplaceOneResult result = null;
            SafeDBOperation(() =>
            {
                result = MongoCollection.ReplaceOne(o => o.Id == entity.Id, entity);
            });

            if (result.ModifiedCount == 1)
            {
                return;
            }
            if (result.ModifiedCount == 0 && result.MatchedCount == 1 && result.IsModifiedCountAvailable && result.IsAcknowledged)
            {
                Log.LogTrace("更新单条数据时，新数据与元数据相同，数据没有发生改变。 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] \r\n更新结果为：[{@Result}]", typeof(T).FullName, entity, result);
                return;
            }
            Log.LogError("更新单条数据时，无数据改变。 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] \r\n更新结果为：[{@Result}]", typeof(T).FullName, entity, result);
        }
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateMany(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                UpdateObject(item);
            }
        }

        #region 异步更新操作
        /// <summary>
        /// 更新单条数据  异步   （据说异步写入数据比同步的慢，有待测试）
        /// </summary>
        /// <param name="entity"></param>
        public async Task<bool> UpdateAsync(T entity)
        {
            Log.LogTrace("异步更新单条数据 ==> 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] ", typeof(T).FullName, entity);
            return await Task.Run(() =>
            {
                return SafeDBOperation(() =>
                {
                    ReplaceOneResult result = MongoCollection.ReplaceOneAsync(new BsonDocument("_id", entity.Id), entity).Result;
                    if (result.ModifiedCount == 1)
                    {
                        return true;
                    }
                    if (result.ModifiedCount == 0 && result.MatchedCount == 1 && result.IsModifiedCountAvailable && result.IsAcknowledged)
                    {
                        Log.LogTrace("异步更新单条数据时，新数据与元数据相同，数据没有发生改变。 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] \r\n更新结果为：[{@Result}]", typeof(T).FullName, entity, result);
                        return true;
                    }
                    Log.LogError("异步更新单条数据时，无数据改变。 类型：[{TypeName}]\r\n数据信息为：[{@Entity}] \r\n更新结果为：[{@Result}]", typeof(T).FullName, entity, result);
                    return false;
                });
            });
        }

        /// <summary>
        /// 更新多条数据 异步   （据说异步写入数据比同步的慢，有待测试）
        /// </summary>
        /// <param name="entities"></param>
        public async Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            bool flag = true;
            foreach (var item in entities)
            {
                if (!await UpdateAsync(item))
                {
                    flag = false;
                }
            }
            return flag;
        }
        #endregion

        #region 根据ID更新属性
        /// <summary>
        /// 根据ID更新一个属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public bool UpdatePropertyById(string id, string propertyName, object propertyValue)
        {
            if (!ObjectId.TryParse(id, out ObjectId objId))
            {
                // 做记录，更新的ID不存在
                Log.LogError("按ID更新单属性时参数错误，无法转换成ObjectId的Id值为：[{Id}]\r\n类型为：[{TypeName}]", id, typeof(T).FullName);
            }
            return UpdatePropertyById(objId, propertyName, propertyValue);
        }
        /// <summary>
        /// 根据ID更新一个属性
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public bool UpdatePropertyById(ObjectId objId, string propertyName, object propertyValue)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objId);
            UpdateDefinition<T> ud = Builders<T>.Update.Set(propertyName, propertyValue);
            UpdateResult result = null;

            SafeDBOperation(() =>
            {
                result = MongoCollection.UpdateOneAsync(filter, ud).Result;
            });

            if (result.ModifiedCount == 0 && result.MatchedCount == 1)
            {
                Log.LogWarning("按ID更新单属性操作取消，更新的属性与原属性相同。\r\nId值为：[{Id}]\r\n类型为：[{TypeName}]\r\n更新属性为：[{PropertyName}]\r\n更新后的值应为：[{PropertyValue}]\r\n操作结果为：[{@Result}]",
                    objId.ToString(), typeof(T).FullName, propertyName, propertyValue, result);
                return true;
            }
            if (1 != result.ModifiedCount)
            {
                // 更新失败，记录日志
                Log.LogError("按ID更新单属性操作取消，无数据被更新。\r\nId值为：[{Id}]\r\n类型为：[{TypeName}]\r\n更新属性为：[{PropertyName}]\r\n更新后的值应为：[{PropertyValue}]\r\n操作结果为：[{@Result}]",
                    objId.ToString(), typeof(T).FullName, propertyName, propertyValue, result);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 按ID更新多个属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="upProperties"></param>
        /// <returns></returns>
        public bool UpdatePropertiesById(string id, UpdateDefinition<T> upProperties)
        {
            if (!ObjectId.TryParse(id, out ObjectId objId))
            {
                // 做记录，更新的ID不存在
                Log.LogError("按ID更新多属性时参数错误，无法转换成ObjectId的Id值为：[{Id}]\r\n类型为：[{TypeName}]", id, typeof(T).FullName);
            }
            return UpdatePropertiesById(objId, upProperties);
        }
        /// <summary>
        /// 按ID更新多个属性
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="upProperties"></param>
        /// <returns></returns>
        public bool UpdatePropertiesById(ObjectId objId, UpdateDefinition<T> upProperties)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objId);
            UpdateResult result = null;
            SafeDBOperation(() =>
            {
                result = MongoCollection.UpdateOneAsync(filter, upProperties).Result;
            });

            if (result.ModifiedCount == 0 && result.MatchedCount == 1)
            {
                Log.LogWarning("按ID更新多属性操作取消，更新的属性与原属性相同。Id值为：[{Id}]\r\n类型为：[{TypeName}]\r\n更新属性信息为：[{@PropertyArray}]\r\n操作结果为：[{@Result}]",
                    objId.ToString(), typeof(T).FullName, upProperties, result);
                return true;
            }
            if (1 != result.ModifiedCount)
            {
                // 更新失败，记录日志
                Log.LogError("按ID更新多属性操作取消，无数据被更新。Id值为：[{Id}]\r\n类型为：[{TypeName}]\r\n更新属性信息为：[{@PropertyArray}]\r\n操作结果为：[{@Result}]", objId.ToString(), typeof(T).FullName, upProperties, result);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据指定条件更新多个属性
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="upProperties">更新属性信息</param>
        /// <param name="updateResult">执行结果</param>
        /// <returns></returns>
        public bool UpdateProperties(FilterDefinition<T> filter, UpdateDefinition<T> upProperties, out UpdateResult updateResult)
        {
            UpdateResult info = null;
            bool result = SafeDBOperation(() =>
            {
                info = MongoCollection.UpdateManyAsync(filter, upProperties).Result;
                return true;
            });
            updateResult = info;

            return result;
        }

        /// <summary>
        /// 根据条件更新多条数据
        /// </summary>
        /// <param name="updateQuery"></param>
        /// <param name="updateEntity"></param>
        public void UpdateMany(Expression<Func<T, bool>> updateQuery, Expression<Func<T, T>> updateEntity)
        {
            // 设置更新内容
            var ud = Builders<T>.Update;
            var updates = new List<UpdateDefinition<T>>();
            var body = (MemberInitExpression)updateEntity.Body;
            foreach (var item in body.Bindings)
            {
                var exp = (MemberAssignment)item;
                var val = (ConstantExpression)exp.Expression;

                updates.Add(ud.Set(exp.Member.Name, val.Value));
            }

            // 设置更新条件
            FilterDefinition<T> filter = updateQuery;

            // 更新数据
            UpdateResult result = null;
            SafeDBOperation(() =>
            {
                result = MongoCollection.UpdateOneAsync(filter, ud.Combine(updates)).Result;
            });
        }
        /// <summary>
        /// 根据id更新部分属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateEntity"></param>
        public void UpdateObject(ObjectId id, Expression<Func<T, T>> updateEntity)
        {
            // 设置更新条件
            Expression<Func<T, bool>> filter = e => e.Id == id;
            UpdateMany(filter, updateEntity);
        }
        /// <summary>
        /// 根据id更新部分属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateEntity"></param>
        public void UpdateObject(string id, Expression<Func<T, T>> updateEntity)
        {
            // 设置更新条件
            MongoIdModel mim = new MongoIdModel();
            ObjectId key = mim.ConvertID(id);
            UpdateObject(key, updateEntity);
        }
        #endregion
        #endregion
    }
}
