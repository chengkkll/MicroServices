using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TianCheng.DAL
{
    /// <summary>
    /// 需要Id的通用的操作接口定义
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public interface IDBOperation<DO, IdType>
    {
        #region 查询处理
        /// <summary>
        /// 根据id来查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO SingleById(IdType id);
        ///// <summary>
        ///// 判断指定属性值是否有重复的
        ///// </summary>
        ///// <param name="objectId">当前对象ID</param>
        ///// <param name="prop">属性名</param>
        ///// <param name="val">属性值</param>
        ///// <param name="ignoreDelete">是否忽略逻辑删除数据</param>
        ///// <returns></returns>
        //bool HasRepeat(IdType objectId, string prop, string val, bool ignoreDelete);
        #endregion

        #region 更新
        /// <summary>
        /// 根据id更新部分属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateEntity"></param>
        void UpdateObject(IdType id, Expression<Func<DO, DO>> updateEntity);
        #endregion

        #region 物理删除操作
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        bool RemoveMany(IEnumerable<IdType> ids);
        /// <summary>
        /// 根据ID 物理删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveObject(IdType id);
        #endregion
    }
}
