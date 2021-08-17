using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// PostgreSql数据库操作处理
    /// </summary>
    /// <typeparam name="PO">数据库存储对象</typeparam>
    /// <typeparam name="Q">查询对象</typeparam>
    /// <typeparam name="IdType">id类型</typeparam>
    public abstract partial class BaseOperation<PO, Q, IdType> : IDBOperationRegister,
        IDBOperation<PO>,
        IDBOperationFilter<PO, Q>
        where PO : IPrimaryKey<IdType>, new()
        where Q : QueryObject
    {
        #region 物理删除
        public bool RemoveObject(PO entity)
        {
            return Remove(entity, null, null);
        }
        /// <summary>
        ///  物理删除对象 
        /// </summary>
        /// <param name="entity"></param>
        public bool Remove(PO entity, IDbTransaction tran = null, int? commandTimeout = null)
        {
            return RemoveByTypeId(entity.Id, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        public bool RemoveById(string id)
        {
            return RemoveById(id, null, null);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public bool RemoveById(string id, IDbTransaction tran = null, int? commandTimeout = null)
        {
            return RemoveByTypeId(IdInstance.ConvertID(id), tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveByTypeId(IdType id)
        {
            return RemoveByTypeId(id, null, null);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public bool RemoveByTypeId(IdType id, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 验证参数
            if (!IdInstance.CheckId(id))
            {
                throw new ArgumentException("物理删除数据时，id参数无效");
            }
            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行删除命令
                connection.ExecuteScalar<IdType>(DefaultRemoveByIdSQL, new { id }, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Remove操作错误  \r\nsql：{DefaultRemoveByIdSQL}   \r\n操作id：{id}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
            return true;
        }

        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Remove(IEnumerable<PO> entities, IDbTransaction tran = null, int? commandTimeout = null)
        {
            return RemoveByTypeIdList(entities.Select(e => e.Id), tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool RemoveByIdList(IEnumerable<string> ids)
        {
            return RemoveByIdList(ids, null, null);
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool RemoveByIdList(IEnumerable<string> ids, IDbTransaction tran = null, int? commandTimeout = null)
        {
            return RemoveByTypeIdList(ids.Select(e => IdInstance.ConvertID(e)), tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        public bool RemoveByTypeIdList(IEnumerable<IdType> ids)
        {
            return RemoveByTypeIdList(ids);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public bool RemoveByTypeIdList(IEnumerable<IdType> ids, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 验证参数
            if (ids.Count() == 1)
            {
                RemoveByTypeId(ids.FirstOrDefault(), tran, commandTimeout);
                return true;
            }
            foreach (IdType id in ids)
            {
                if (!IdInstance.CheckId(id))
                {
                    throw new ArgumentException("物理删除一组数据时，id列表参数无效");
                }
            }


            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();
            // 设置sql
            string sql = DefaultRemoveByIdSQL;
            var param = new DynamicParameters();
            param.Add("id", ids.FirstOrDefault());

            for (int i = 1; i < ids.Count(); i++)
            {
                sql += $" or id=@id{i}";
                param.Add($"@id{i}", ids.ElementAt(i));
            }

            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行删除命令
                connection.ExecuteScalar<IdType>(sql, param, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Remove操作错误  \r\nsql：{sql}   \r\n操作id：{ids}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
            return true;
        }

        #endregion

        #region 逻辑删除
        /// <summary>
        ///  物理删除对象 
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(PO entity, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            DeleteByTypeId(entity.Id, field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public void DeleteById(string id, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            DeleteByTypeId(IdInstance.ConvertID(id), field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public void DeleteByTypeId(IdType id, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 验证参数
            if (!IdInstance.CheckId(id))
            {
                throw new ArgumentException("逻辑删除时id参数无效");
            }
            string sql = DefaultDeleteByIdSQL;
            if (field != "is_delete" && !string.IsNullOrWhiteSpace(field))
            {
                sql = sql.Replace("is_delete", field);
            }

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行逻辑删除命令
                connection.ExecuteScalar<IdType>(sql, new { id }, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Delete操作错误  \r\nsql：{sql}   \r\n操作id：{id}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
        }

        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void Delete(IEnumerable<PO> entities, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            DeleteByTypeIdList(entities.Select(e => e.Id), field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void DeleteByIdList(IEnumerable<string> ids, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            DeleteByTypeIdList(ids.Select(e => IdInstance.ConvertID(e)), field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public void DeleteByTypeIdList(IEnumerable<IdType> ids, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 优化
            if (ids.Count() == 1)
            {
                DeleteByTypeId(ids.FirstOrDefault(), tran: tran, commandTimeout: commandTimeout);
                return;
            }
            // 验证参数
            foreach (IdType id in ids)
            {
                if (!IdInstance.CheckId(id))
                {
                    throw new ArgumentException("逻辑删除时id参数无效。");
                }
            }

            // 设置sql
            string sql = DefaultDeleteByIdSQL;
            if (field != "is_delete" && !string.IsNullOrWhiteSpace(field))
            {
                sql = sql.Replace("is_delete", field);
            }
            var param = new DynamicParameters();
            param.Add("id", ids.FirstOrDefault());
            for (int i = 1; i < ids.Count(); i++)
            {
                sql += $" or id=@id{i}";
                param.Add($"@id{i}", ids.ElementAt(i));
            }

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();
            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行删除命令
                connection.ExecuteScalar<IdType>(sql, param, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Delete操作异常  \r\nsql：{sql}   \r\n操作id：{ids.ToJson()}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
        }
        #endregion

        #region 恢复删除
        /// <summary>
        ///  恢复已删除对象
        /// </summary>
        /// <param name="entity"></param>
        public void Undelete(PO entity, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            UndeleteByTypeId(entity.Id, field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID恢复已删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public void UndeleteById(string id, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            UndeleteByTypeId(IdInstance.ConvertID(id), field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="id">删除的ID</param>
        /// <returns>返回已删除的对象信息</returns>
        public void UndeleteByTypeId(IdType id, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 验证参数
            if (!IdInstance.CheckId(id))
            {
                throw new ArgumentException("恢复已删除数据时，id参数无效");
            }
            // 设置sql
            string sql = DefaultUndeleteByIdSQL;
            if (field != "is_delete" && !string.IsNullOrWhiteSpace(field))
            {
                sql = sql.Replace("is_delete", field);
            }

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();
            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行逻辑删除命令
                connection.ExecuteScalar<IdType>(sql, new { id }, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Undelete操作错误  \r\nsql：{sql}   \r\n操作id：{id}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
        }

        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void Undelete(IEnumerable<PO> entities, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            UndeleteByTypeIdList(entities.Select(e => e.Id), field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID列表 物理删除一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void UndeleteByIdList(IEnumerable<string> ids, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            UndeleteByTypeIdList(ids.Select(e => IdInstance.ConvertID(e)), field, tran, commandTimeout);
        }
        /// <summary>
        /// 根据ID 物理删除数据
        /// </summary>
        /// <param name="ids">恢复逻辑删除的ID列表</param>
        /// <param name="field">逻辑删除字段名</param>
        /// <param name="tran">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public void UndeleteByTypeIdList(IEnumerable<IdType> ids, string field = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 优化
            if (ids.Count() == 1)
            {
                UndeleteByTypeId(ids.FirstOrDefault(), tran: tran, commandTimeout: commandTimeout);
                return;
            }
            // 验证参数
            foreach (IdType id in ids)
            {
                if (!IdInstance.CheckId(id))
                {
                    throw new ArgumentException("恢复已删除数据时，id参数无效。");
                }
            }

            // 设置sql
            string sql = DefaultDeleteByIdSQL;
            if (field != "is_delete" && !string.IsNullOrWhiteSpace(field))
            {
                sql = sql.Replace("is_delete", field);
            }
            var param = new DynamicParameters();
            param.Add("id", ids.FirstOrDefault());
            for (int i = 1; i < ids.Count(); i++)
            {
                sql += $" or id=@id{i}";
                param.Add($"@id{i}", ids.ElementAt(i));
            }

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();
            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行删除命令
                connection.ExecuteScalar<IdType>(sql, param, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Undelete操作错误  \r\nsql：{sql}   \r\n操作id：{ids.ToJson()}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
        }
        #endregion

        #region 清空表
        /// <summary>
        /// 清空表
        /// </summary>
        public void Drop()
        {
            Drop(null, null);
        }
        /// <summary>
        /// 清空表
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        public void Drop(IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 设置sql
            string sql = $"TRUNCATE {TableName} RESTART IDENTITY;";

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();
            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行删除命令
                connection.ExecuteScalar<IdType>(sql, transaction: tran, commandTimeout: commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"清空表错误  \r\nsql：{sql} ");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
        }
        #endregion
    }
}
