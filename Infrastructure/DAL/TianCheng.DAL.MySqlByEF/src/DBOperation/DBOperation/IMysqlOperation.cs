using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TianCheng.DAL.MySqlByEF
{
    /// <summary>
    /// 对象数据库操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public interface IMysqlOperation<T, IdType> : IDBOperation<T>
    {
        #region 查询处理
        /// <summary>
        /// 根据Id获取对象实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T SingleById(IdType id);
        #endregion

        #region 数据插入
        #region 异步插入操作
        /// <summary>
        /// 异步插入单条数据 
        /// </summary>
        /// <param name="entity"></param>
        Task InsertAsync(T entity);

        /// <summary>
        /// 异步插入多条数据
        /// </summary>
        /// <param name="entities"></param>
        Task InsertManyAsync(IEnumerable<T> entities);
        #endregion
        #endregion

        #region 物理删除
        /// <summary>
        ///  物理删除对象列表 
        /// </summary>
        /// <param name="entityList"></param>
        bool RemoveMany(IEnumerable<T> entityList);

        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool RemoveMany(IEnumerable<IdType> ids);

        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        bool RemoveObject(IdType id);
        #endregion
    }
}
