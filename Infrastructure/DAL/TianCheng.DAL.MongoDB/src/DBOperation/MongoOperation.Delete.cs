using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.Common;

namespace TianCheng.DAL.MongoDB
{
    ///// <summary>
    ///// MongoDB 持久化操作
    ///// </summary>
    //public partial class MongoOperation<T, Q> : IDBOperationRegister,
    //    IMongoDBOperation<T>,
    //    IDBOperationFilter<T, Q>
    //    where Q : QueryObject
    //    where T : MongoIdModel
    //{

    //    #region 逻辑删除数据
    //    /// <summary>
    //    /// 根据ID 逻辑删除数据
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    public bool DeleteById(string id)
    //    {
    //        if (!ObjectId.TryParse(id, out ObjectId objId))
    //        {
    //            // 做记录，删除的ID不存在
    //            Log.LogError("按ID逻辑删除时参数错误，无法转换成ObjectId的Id值为：[{Id}]\r\n类型为：[{TypeName}]", id, typeof(T).FullName);
    //        }
    //        return DeleteByTypeId(objId);
    //    }
    //    /// <summary>
    //    /// 根据ID 逻辑删除数据
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool DeleteByTypeId(ObjectId objId)
    //    {
    //        FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objId);
    //        UpdateDefinition<T> ud = Builders<T>.Update.Set("IsDelete", true);

    //        UpdateProperties(filter, ud, out UpdateResult result);
    //        if (result.ModifiedCount == 0 && result.MatchedCount == 1)
    //        {
    //            Log.LogWarning("按ID逻辑删除操作取消，数据已被逻辑删除，无需再次逻辑删除。Id值为：[{Id}]\r\n类型为：[{TypeName}]\r\n操作结果为：[{@Result}]", objId.ToString(), typeof(T).FullName, result);
    //            return true;
    //        }
    //        if (1 != result.ModifiedCount)
    //        {
    //            // 更新失败，记录日志
    //            Log.LogError("按ID逻辑删除时失败，无数据被更新。Id值为：[{Id}]\r\n类型为：[{TypeName}]\r\n操作结果为：[{@Result}]", objId.ToString(), typeof(T).FullName, result);
    //            return false;
    //        }
    //        return true;
    //    }
    //    /// <summary>
    //    /// 根据ID列表 逻辑删除一组数据
    //    /// </summary>
    //    /// <param name="ids"></param>
    //    /// <returns></returns>
    //    public bool DeleteByIdList(IEnumerable<string> ids)
    //    {
    //        List<ObjectId> objIdList = new List<ObjectId>();
    //        foreach (string id in ids)
    //        {
    //            if (!ObjectId.TryParse(id, out ObjectId objId))
    //            {
    //                // 做记录，删除的ID不存在
    //                Log.LogError("按ID逻辑删除时参数错误，无法转换成ObjectId的Id值为：[{Id}]\r\nID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", id, ids.ToJson(), typeof(T).FullName);
    //                continue;
    //            }
    //            objIdList.Add(objId);
    //        }

    //        return DeleteByTypeIdList(objIdList);
    //    }
    //    /// <summary>
    //    /// 根据ID列表 逻辑删除一组数据
    //    /// </summary>
    //    /// <param name="ids"></param>
    //    /// <returns></returns>
    //    public bool DeleteByTypeIdList(IEnumerable<ObjectId> ids)
    //    {
    //        FilterDefinition<T> filter = Builders<T>.Filter.AnyIn("_id", ids);
    //        UpdateDefinition<T> ud = Builders<T>.Update.Set("IsDelete", true);

    //        UpdateProperties(filter, ud, out UpdateResult result);      // 执行逻辑删除操作

    //        if (result.ModifiedCount != ids.Count() && result.MatchedCount == ids.Count())
    //        {
    //            Log.LogWarning("按ID逻辑删除操作完成，其中部分数据在操作前已被逻辑删除。Id列表为：[{IdArray}]\r\n类型为：[{TypeName}]\r\n操作结果为：[{@Result}]", ids.ToString(), typeof(T).FullName, result);
    //            return true;
    //        }
    //        if (ids.Count() != result.ModifiedCount)
    //        {
    //            // 不完全更新，记录日志
    //            Log.LogWarning("根据ID列表 逻辑删除一组数据 ==> 已删除数据：{DeletedCount}条,按条件应删除{PlanCount}条\r\n 类型：[{TypeName}]\r\n查询Id列表为：[{FilterIdArray}] ", result.MatchedCount, ids.Count(), typeof(T).FullName, ids.ToJson());
    //            return false;
    //        }
    //        return true;
    //    }
    //    #endregion

    //    #region 取消逻辑删除数据
    //    /// <summary>
    //    /// 根据ID 还原被逻辑删除的数据
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    public bool UndeleteById(string id)
    //    {
    //        if (!ObjectId.TryParse(id, out ObjectId objId))
    //        {
    //            // 记录日志：逻辑删除的ID错误
    //            Log.LogError("按ID取消逻辑删除时参数错误，无法转换成ObjectId。\r\nId值为：[{Id}]\t类型为：[{TypeName}]", id, typeof(T).FullName);
    //        }
    //        return UndeleteByTypeId(objId);
    //    }
    //    /// <summary>
    //    /// 根据ID 还原被逻辑删除的数据
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool UndeleteByTypeId(ObjectId objId)
    //    {
    //        FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objId);
    //        UpdateDefinition<T> ud = Builders<T>.Update.Set("IsDelete", false);

    //        UpdateProperties(filter, ud, out UpdateResult result);      // 执行逻辑删除操作
    //        if (result.ModifiedCount != 1 && result.MatchedCount == 1)
    //        {
    //            Log.LogWarning("按ID逻辑删除操作完成，数据在操作前已被逻辑删除。Id值为：[{Id}]\r\n类型为：[{TypeName}]\r\n操作结果为：[{@Result}]", objId.ToString(), typeof(T).FullName, result);
    //            return true;
    //        }
    //        if (1 != result.ModifiedCount)
    //        {
    //            // 更新失败，记录日志
    //            Log.LogError("按ID取消逻辑删除时失败，无数据被更新。Id值为：[{Id}]\r\n类型为：[{TypeName}]\r\n操作结果为：[{@Result}]", objId.ToString(), typeof(T).FullName, result);
    //            return false;
    //        }
    //        return true;
    //    }
    //    /// <summary>
    //    /// 根据ID列表 还原被逻辑删除的一组数据
    //    /// </summary>
    //    /// <param name="ids"></param>
    //    /// <returns></returns>
    //    public bool UndeleteByIdList(IEnumerable<string> ids)
    //    {
    //        List<ObjectId> objIdList = new List<ObjectId>();
    //        foreach (string id in ids)
    //        {
    //            if (!ObjectId.TryParse(id, out ObjectId objId))
    //            {
    //                // 做记录，删除的ID不存在
    //                Log.LogError("按ID取消逻辑删除时参数错误，无法转换成ObjectId的Id值为：[{Id}]\r\nID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", id, ids.ToJson(), typeof(T).FullName);
    //                continue;
    //            }
    //            objIdList.Add(objId);
    //        }

    //        return UndeleteByTypeIdList(objIdList);
    //    }
    //    /// <summary>
    //    /// 根据ID列表 还原被逻辑删除的一组数据
    //    /// </summary>
    //    /// <param name="ids"></param>
    //    /// <returns></returns>
    //    public bool UndeleteByTypeIdList(IEnumerable<ObjectId> ids)
    //    {
    //        FilterDefinition<T> filter = Builders<T>.Filter.AnyIn("_id", ids);          // 设置还原的过滤条件
    //        UpdateDefinition<T> ud = Builders<T>.Update.Set("IsDelete", false);         // 设置还原逻辑删除

    //        UpdateProperties(filter, ud, out UpdateResult result);                      // 执行还原逻辑删除操作
    //        if (result.ModifiedCount != ids.Count() && result.MatchedCount == ids.Count())
    //        {
    //            Log.LogWarning("按ID还原删除操作完成，其中部分数据在操作前是非删除状态。Id列表为：[{V}]\r\n类型为：[{TypeName}]\r\n操作结果为：[{@Result}]", ids.ToString(), typeof(T).FullName, result);
    //            return true;
    //        }
    //        if (ids.Count() != result.ModifiedCount)
    //        {
    //            // 还原不完全时，记录日志
    //            Log.LogWarning("根据ID列表 取消逻辑删除一组数据 ==> 已删除数据：{DeletedCount}条,按条件应删除{PlanCount}条。\r\n 类型：[{TypeName}]\r\n查询条件为：[{FilterIdArray}] ", result.MatchedCount, ids.Count(), typeof(T).FullName, ids.ToJson());
    //            return false;
    //        }
    //        return true;
    //    }
    //    #endregion


    //}
}
