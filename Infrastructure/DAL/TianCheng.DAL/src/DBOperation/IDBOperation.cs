using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL
{
    /// <summary>
    /// 数据库的通用的操作接口定义
    /// </summary>
    public interface IDBOperation<DO>
    {
        #region 查询处理
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [Obsolete("尽量避免使用")]
        List<DO> Search();
        /// <summary>
        /// 获取当前对象的查询链式接口
        /// </summary>
        /// <returns></returns>
        IQueryable<DO> Queryable();
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="filterFactory"></param>
        /// <returns></returns>
        List<DO> Search(Expression<Func<DO, bool>> filterFactory);
        /// <summary>
        /// 查询记录数量
        /// </summary>
        /// <returns></returns>
        long Count();
        #endregion

        #region 数据保存
        #region Save
        /// <summary>
        /// 保存对象，根据ID是否为空来判断是新增还是修改操作
        /// </summary>
        /// <param name="entity"></param>
        void Save(DO entity);
        #endregion

        #region 数据的新增
        /// <summary>
        /// 插入单条新数据
        /// </summary>
        /// <param name="entity"></param>
        void InsertObject(DO entity);
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="entities"></param>
        void InsertMany(IEnumerable<DO> entities);
        #endregion

        #region 数据更新
        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void UpdateObject(DO entity);
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void UpdateMany(IEnumerable<DO> entities);
        /// <summary>
        /// 根据条件更新多条数据
        /// </summary>
        /// <param name="updateQuery"></param>
        /// <param name="updateEntity"></param>
        void UpdateMany(Expression<Func<DO, bool>> updateQuery, Expression<Func<DO, DO>> updateEntity);
        #endregion
        #endregion

        #region 数据删除操作

        #region 物理删除数据
        /// <summary>
        /// 根据对象物理删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool RemoveObject(DO entity);
        #endregion

        #region 删除表（集合）
        /// <summary>
        /// 删除表（集合）
        /// </summary>
        void Drop();
        #endregion

        #endregion
    }
}
