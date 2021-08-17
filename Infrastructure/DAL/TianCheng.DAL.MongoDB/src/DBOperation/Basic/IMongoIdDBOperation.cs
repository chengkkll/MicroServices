using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL
{
    /// <summary>
    /// Id为Mongo ObjectId类型的数据库操作接口
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public interface IMongoIdDBOperation<DO>
    {
        #region 查询处理
        /// <summary>
        /// 根据id来查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO SingleById(string id);
        /// <summary>
        /// 根据id来查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO SingleById(ObjectId id);
        /// <summary>
        /// 判断指定属性是否有重复的
        /// </summary>
        /// <param name="objectId">当前对象ID</param>
        /// <param name="prop">属性名</param>
        /// <param name="val">属性值</param>
        /// <param name="ignoreDelete">是否忽略逻辑删除数据</param>
        /// <returns></returns>
        bool HasRepeat(ObjectId objectId, string prop, string val, bool ignoreDelete);
        #endregion

        #region 物理删除操作
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        bool RemoveMany(IEnumerable<string> ids);
        /// <summary>
        /// 根据ID 物理删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveObject(string id);
        #endregion
    }
}
