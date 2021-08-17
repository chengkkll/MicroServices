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
        #region Single
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PO SingleByStringId(string id)
        {
            return SingleById(IdInstance.ConvertID(id));
        }
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public PO SingleByStringId(string id, IDbTransaction tran = null, int? commandTimeout = null)
        {
            return SingleById(IdInstance.ConvertID(id), tran, commandTimeout);
        }
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PO SingleById(IdType id)
        {
            return SingleById(id, null, null);
        }
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <returns></returns>
        public virtual PO SingleById(IdType id, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 检查参数
            if (!IdInstance.CheckId(id))
            {
                NpgLog.Logger.Error("按id查询时，id参数错误。{Id}", id);
                throw new ArgumentException("查询参数无效");
            }
            DynamicParameters param = new DynamicParameters();
            param.Add("id", id);

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            PO result;
            try
            {
                result = connection.Query<PO>(DefaultSelectByIdSQL, param, tran, commandTimeout: commandTimeout).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按id查询异常，sql：{DefaultSelectByIdSQL}   查询id：{id}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return result;
        }

        /// <summary>
        /// 根据条件查询一个对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual PO SingleBySql(string sql, DynamicParameters parameters = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            PO result;
            try
            {
                result = connection.Query<PO>(sql, parameters, tran, commandTimeout: commandTimeout).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按条件查询一个对象异常，sql：{sql}   查询条件：{parameters.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return result;
        }
        #endregion

        #region HasRepeat
        /// <summary>
        /// 查看指定属性值在表中是否有重复项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        /// <param name="ignoreDelete"></param>
        /// <param name="deleteField"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool HasRepeatByStringId(string id, string prop, string val, bool ignoreDelete = true, string deleteField = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            return HasRepeat(IdInstance.ConvertID(id), prop, val, ignoreDelete, deleteField, tran, commandTimeout);
        }
        /// <summary>
        /// 查看指定属性值在表中是否有重复项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        /// <param name="ignoreDelete"></param>
        /// <returns></returns>
        public bool HasRepeat(IdType id, string prop, string val, bool ignoreDelete = true)
        {
            return HasRepeat(id, prop, val, ignoreDelete, "is_delete");
        }
        /// <summary>
        /// 查看指定属性值在表中是否有重复项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        /// <param name="ignoreDelete"></param>
        /// <param name="deleteField"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool HasRepeat(IdType id, string prop, string val, bool ignoreDelete = true, string deleteField = "is_delete", IDbTransaction tran = null, int? commandTimeout = null)
        {
            string sql = ignoreDelete ? $"{DefaultCountSQL} where {prop}=@Val" : $"{DefaultCountSQL} where {prop}=@Val and id=@id and {deleteField}=false";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Val", val);
            parameters.Add("id", id);

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            int count;
            try
            {
                count = connection.ExecuteScalar<int>(sql, parameters, transaction: tran, commandTimeout: commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按sql查询异常  \r\nsql：{sql}  \r\n查询参数：{parameters?.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return count > 0;
        }
        #endregion

        #region SQL
        /// <summary>
        /// 查询满足条件的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        internal IEnumerable<PO> SearchSql(string sql = "", DynamicParameters parameters = null, int pageIndex = -1, int pageSize = -1, string order = "", IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 检查参数
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = DefaultSearchSQL;
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                sql += $" order by {order}";
            }
            if (pageSize > -1)
            {
                sql += $" limit {pageSize}";
            }
            if (pageIndex > -1)
            {
                sql += $" offset {pageIndex * pageSize}";
            }

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            IEnumerable<PO> result;
            try
            {
                result = connection.Query<PO>(sql, parameters, transaction: tran, commandTimeout: commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按sql查询异常  \r\nsql：{sql}  \r\n页号：{pageIndex}  \r\n每页数据量：{pageSize}  \r\n查询参数：{parameters?.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return result;
        }
        /// <summary>
        /// 查询满足条件的数据
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<DO> SearchSql<DO>(string sql = "", DynamicParameters parameters = null, int pageIndex = -1, int pageSize = -1, string order = "", IDbTransaction tran = null, int? commandTimeout = null)
        {
            var data = SearchSql(sql, parameters, pageIndex, pageSize, order, tran, commandTimeout);
            if (data == null)
            {
                return default;
            }
            return data.AutoMapper<IEnumerable<DO>>();
        }
        #endregion

        #region SearchAll
        /// <summary>
        /// 查询所有的数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        internal IEnumerable<PO> SearchAll(string order = "", IDbTransaction tran = null, int? commandTimeout = null)
        {
            return SearchSql(DefaultSearchMainSQL, null, -1, -1, order, tran, commandTimeout);
        }
        /// <summary>
        /// 查询所有的数据
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<DO> SearchAll<DO>(string order = "", IDbTransaction tran = null, int? commandTimeout = null)
        {
            var data = SearchAll(order, tran, commandTimeout);
            if (data == null)
            {
                return default;
            }
            return data.AutoMapper<IEnumerable<DO>>();
        }
        #endregion

        #region Count
        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return CountSql();
        }
        /// <summary>
        /// 查询满足条件的记录数量
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int CountSql(string sql = "", DynamicParameters parameters = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 检查参数
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = DefaultCountSQL;
            }

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            int count;
            try
            {
                count = connection.ExecuteScalar<int>(sql, parameters, transaction: tran, commandTimeout: commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按sql查询异常  \r\nsql：{sql}  \r\n查询参数：{parameters?.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return count;
        }
        #endregion
    }
}
