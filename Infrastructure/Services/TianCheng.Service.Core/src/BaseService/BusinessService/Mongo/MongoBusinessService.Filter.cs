using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TianCheng.Common;
using TianCheng.DAL;
using System.Threading.Tasks;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// Mongo业务操作
    /// </summary>
    public class MongoBusinessService<DAL, DO, QO> : BusinessService<DAL, DO, ObjectId, QO>
        where DO : MongoIdModel, new()
        where DAL : IDBOperation<DO, ObjectId>, IDBOperation<DO>, IDBOperationFilter<DO, QO>
        where QO : QueryObject, new()
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public MongoBusinessService(DAL dal) : base(dal)
        {

        }
        #endregion

        #region SingleById
        /// <summary>
        /// 根据id查询对象信息
        /// </summary>
        /// <typeparam name="DAL"></typeparam>
        /// <typeparam name="DO"></typeparam>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO SingleById(string id)
        {
            return Dal.SingleById(IdInstance.ConvertID(id));
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
        public View SingleById<View>(string id)
        {
            DO entity = SingleById(id);
            return ObjectTran.Tran<View>(entity);
        }
        #endregion

        #region 物理删除
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public Task<ResultView> Remove(string id, ControllerBase controller)
        {
            return Remove(SingleById(id), controller);
        }
        #endregion
    }
}
