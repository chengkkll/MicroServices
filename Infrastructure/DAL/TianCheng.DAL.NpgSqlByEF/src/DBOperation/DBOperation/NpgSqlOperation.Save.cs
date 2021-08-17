using TianCheng.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        #region Save
        /// <summary>
        /// 保存对象
        /// 根据ID是否为空来判断是新增还是修改操作
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
        public virtual void InsertObject(T entity)
        {
            try
            {
                using C c = new C();
                c.Set<T>().Add(entity);
                c.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "插入对象时错误。类型：[{TypeName}]\r\n数据信息为：[{@Entity}] ", typeof(T).FullName, entity);
                throw;
            }
        }

        /// <summary>
        /// 插入多条新数据
        /// </summary>
        /// <param name="entities"></param>
        public void InsertMany(IEnumerable<T> entities)
        {
            try
            {
                using C c = new C();
                c.Set<T>().AddRange(entities);
                c.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "插入多个对象时错误。类型：[{TypeName}]\r\n数据信息为：[{@Entities}] ", typeof(T).FullName, entities);
                throw;
            }
        }

        #region 异步插入操作
        /// <summary>
        /// 异步插入单条数据 
        /// </summary>
        /// <param name="entity"></param>
        public async Task InsertAsync(T entity)
        {
            try
            {
                using C c = new C();
                await c.Set<T>().AddAsync(entity);
                await c.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "异步插入对象时错误。类型：[{TypeName}]\r\n数据信息为：[{@Entity}] ", typeof(T).FullName, entity);
                throw;
            }
        }

        /// <summary>
        /// 异步插入多条数据
        /// </summary>
        /// <param name="entities"></param>
        public async Task InsertManyAsync(IEnumerable<T> entities)
        {
            try
            {
                using C c = new C();
                await c.Set<T>().AddRangeAsync(entities);
                await c.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "异步插入多个对象时错误。类型：[{TypeName}]\r\n数据信息为：[{@Entities}] ", typeof(T).FullName, entities);
                throw;
            }
        }
        #endregion
        #endregion

        #region 数据更新
        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void UpdateObject(T entity)
        {
            try
            {
                using C c = new C();
                c.Update(entity);
                c.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "更新对象时错误。类型：[{TypeName}]\r\n数据信息为：[{@Entity}] ", typeof(T).FullName, entity);
                throw;
            }
        }
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateMany(IEnumerable<T> entities)
        {
            try
            {
                using C c = new C();
                c.Set<T>().UpdateRange(entities);
                c.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "更新多个对象时错误。类型：[{TypeName}]\r\n更新信息为：[{@Entities}] ", typeof(T).FullName, entities);
                throw;
            }
        }

        /// <summary>
        /// 根据条件更新多条数据
        /// </summary>
        /// <param name="updateQuery"></param>
        /// <param name="updateEntity"></param>
        public void UpdateMany(Expression<Func<T, bool>> updateQuery, Expression<Func<T, T>> updateEntity)
        {
            try
            {
                using C c = new C();
                c.Set<T>().Where(updateQuery).UpdateFromQuery(updateEntity);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "根据条件更新多个对象时错误。类型：[{TypeName}]\r\n更新信息为：[{@Change}] ", typeof(T).FullName, updateEntity);
                throw;
            }
        }

        /// <summary>
        /// 根据id更新部分属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateEntity"></param>
        public void UpdateObject(IdType id, Expression<Func<T, T>> updateEntity)
        {
            try
            {
                using C c = new C();
                c.Set<T>().Where(e => e.Id.Equals(id)).UpdateFromQuery(updateEntity);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "根据条件更新多个对象时错误。类型：[{TypeName}]\r\n更新信息为：[{@Change}] ", typeof(T).FullName, updateEntity);
                throw;
            }
        }
        #endregion
    }
}