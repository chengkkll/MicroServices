using TianCheng.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TianCheng.Common;
using Z.EntityFramework.Plus;

namespace TianCheng.DAL.NpgSqlByEF
{
    public partial class NpgSqlOperation<C, T, Q, IdType> :
        INpgSqlOperation<T, IdType>,
        INpgSqlOperationFilter<T, Q>,
        IDBOperationRegister,
        IDBOperationFilter<T, Q>
        where C : NpgSqlContext, new()
        where Q : QueryObject
        where T : class, IIdModel<IdType>, new()
    {
        #region 物理删除
        /// <summary>
        ///  物理删除对象列表 
        /// </summary>
        /// <param name="entityList"></param>
        public bool RemoveMany(IEnumerable<T> entityList)
        {
            return RemoveMany(entityList.Select(e => e.Id));
        }

        /// <summary>
        ///  物理删除对象 
        /// </summary>
        /// <param name="entity"></param>
        public bool RemoveObject(T entity)
        {
            if (entity.IdEmpty)
            {
                return false;
            }

            return RemoveObject(entity.Id);
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool RemoveMany(Expression<Func<T, bool>> predicate)
        {
            try
            {
                using C c = new C();
                c.Set<T>().Where(predicate).DeleteFromQuery();
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "按条件物理删除时错误。\r\n类型为：[{TypeName}]", typeof(T).FullName);
                throw;
            }
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool RemoveMany(IEnumerable<IdType> ids)
        {
            try
            {
                using C c = new C();
                c.Set<T>().Where(e => ids.Contains(e.Id)).DeleteFromQuery();
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "按ID列表物理删除时错误。ID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", ids.ToJson(), typeof(T).FullName);
                throw;
            }
        }

        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public bool RemoveObject(IdType id)
        {
            try
            {
                using C c = new C();
                c.Set<T>().Where(e => e.Id.Equals(id)).DeleteFromQuery();
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "按ID物理删除时错误。Id值为：[{Id}]\r\n类型为：[{TypeName}]", id, typeof(T).FullName);
                throw;
            }
        }
        #endregion

        #region 删除表（集合）
        /// <summary>
        /// 删除表（集合）
        /// </summary>
        public void Drop()
        {
            RemoveMany(e => true);
        }
        #endregion
    }
}