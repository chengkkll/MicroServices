using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// Mongo业务操作 扩展方法
    /// </summary>
    static public class MongoBusinessServiceExt
    {
        /// <summary>
        /// 根据id查询对象信息
        /// </summary>
        /// <typeparam name="DAL"></typeparam>
        /// <typeparam name="DO"></typeparam>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static public DO SingleById<DAL, DO>(this BusinessService<DAL, DO, ObjectId> service, string id)
            where DO : MongoIdModel, new()
            where DAL : IDBOperation<DO, ObjectId>, IDBOperation<DO>
        {
            return service.SingleById(BusinessService<DAL, DO, ObjectId>.IdInstance.ConvertID(id));
        }

        /// <summary>
        /// 根据id查询对象信息
        /// </summary>
        /// <typeparam name="DAL"></typeparam>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="View"></typeparam>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static public View SingleById<DAL, DO, View>(this BusinessService<DAL, DO, ObjectId> service, string id)
            where DO : MongoIdModel, new()
            where DAL : IDBOperation<DO, ObjectId>, IDBOperation<DO>
        {
            DO entity = service.SingleById(id);
            return ObjectTran.Tran<View>(entity);
        }
    }
}
