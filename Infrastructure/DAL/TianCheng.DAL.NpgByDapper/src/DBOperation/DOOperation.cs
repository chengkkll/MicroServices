using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    public abstract class DOOperation<DO, PO, Q, IdType> : BaseOperation<PO, Q, IdType>
         where PO : IPrimaryKey<IdType>, new()
         where Q : QueryObject
    {
        #region 查询
        /// <summary>
        /// 设置统一的查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual string ConditionSQL(Q query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据查询条件组织SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="where"></param>
        /// <param name="defFront"></param>
        /// <returns></returns>
        protected string CombineSQL(Q query, string where = "", string defFront = "")
        {
            if (string.IsNullOrWhiteSpace(defFront))
            {
                defFront = DefaultSearchSQL;
            }

            if (string.IsNullOrWhiteSpace(where))
            {
                where = ConditionSQL(query);
            }

            string order = query.Sort.OrderSQL();
            string limit = query.Page.LimitSQL();
            string offset = query.Page.OffsetSQL();

            return $"{defFront}{where}{order}{limit}{offset};";
        }

        public IEnumerable<IdType> SearchId(Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            string defFrontSQL = $"select id from {TableName} where 1=1";
            string where = ConditionSQL(query);
            string order = query.Sort.OrderSQL();
            if (query.Page.Size <= 0)
            {
                query.Page.Size = 100;
            }
            string limit = query.Page.LimitSQL();
            string sql = $"{defFrontSQL}{where}{order}{limit};";
            // 组织查询条件
            DynamicParameters parameters = ToDynamicParameters(query);

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            IEnumerable<IdType> result;
            try
            {
                result = connection.Query<IdType>(sql, parameters, transaction: tran, commandTimeout: commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按sql查询异常  \r\nsql：{sql}  \r\n查询参数：{parameters?.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return result;
        }

        #region 下拉列表数据查询
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectView> Select(Q query, string defFrontSQL = "", IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 获取SQL
            if (string.IsNullOrWhiteSpace(defFrontSQL))
            {
                defFrontSQL = $"select id,code,name from {TableName} where 1=1";
            }
            string where = ConditionSQL(query);
            string order = query.Sort.OrderSQL();
            if (query.Page.Size <= 0)
            {
                query.Page.Size = 100;
            }
            string limit = query.Page.LimitSQL();
            string sql = $"{defFrontSQL}{where}{order}{limit};";
            // 组织查询条件
            DynamicParameters parameters = ToDynamicParameters(query);

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            List<SelectView> result = new List<SelectView>();
            try
            {
                var data = connection.Query(sql, parameters, transaction: tran, commandTimeout: commandTimeout);
                foreach (var item in data)
                {
                    result.Add(new SelectView() { Id = item.id.ToString(), Code = item.code, Name = item.name });
                }
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按sql查询异常  \r\nsql：{sql}  \r\n查询参数：{parameters?.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return result;
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 查询分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public PagedResult<DO> SearchPages(Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 获取SQL
            string where = ConditionSQL(query);
            string sql = $"{CombineSQL(query, where, DefaultSearchSQL)} {CombineSQL(query, where, DefaultCountSQL)}";

            // 组织查询条件
            DynamicParameters parameters = ToDynamicParameters(query);

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            IEnumerable<PO> data = null;
            int count = 0;
            try
            {
                var gridReader = connection.QueryMultiple(sql, parameters, transaction: tran, commandTimeout: commandTimeout);
                if (!gridReader.IsConsumed)
                {
                    data = gridReader.Read<PO>();
                    count = gridReader.ReadFirstOrDefault<int>();
                }
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"按sql查询异常  \r\nsql：{sql} \r\n查询参数：{parameters?.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);

            return new PagedResult<DO>(data.AutoMapper<IEnumerable<DO>>(), new PagedResultPagination()
            {
                PageIndex = query.Page.Index,
                PageSize = query.Page.Size,
                TotalPage = (int)Math.Ceiling((double)count / (double)query.Page.Size),
                TotalRecords = count
            });
        }
        #endregion

        #region Search
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="Q"></typeparam>
        /// <param name="conditionSql"></param>
        /// <param name="query"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<DO> Search(Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            var data = Search(ConditionSQL(query), query, tran, commandTimeout);
            if (data == null)
            {
                return default;
            }
            return data.AutoMapper<IEnumerable<DO>>();
        }
        /// <summary>
        /// 将查询条件转成Dapper查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected DynamicParameters ToDynamicParameters(Q query)
        {
            DynamicParameters param = new DynamicParameters();
            foreach (PropertyInfo p in (typeof(Q)).GetProperties())
            {
                if (p.Name == "Sort" || p.Name == "Page") continue;
                if (p.GetCustomAttributes(typeof(Newtonsoft.Json.JsonIgnoreAttribute), true).Length == 0)
                {
                    param.Add(p.Name, p.GetValue(query));
                }
            }
            return param;
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="conditionSql"></param>
        /// <param name="query"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        internal IEnumerable<PO> Search(string conditionSql, Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            string sql = $"{DefaultSearchSQL} {conditionSql}";

            return SearchSql(sql, ToDynamicParameters(query), query.Page.Index, query.Page.Size, query.Sort.OrderSQL(), tran, commandTimeout);
        }
        #endregion

        #region Single
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <param name="id"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DO SingleDOByStringId(string id, IDbTransaction tran = null, int? commandTimeout = null)
        {
            PO t = SingleByStringId(id, tran, commandTimeout);
            if (t == null)
            {
                return default;
            }
            return t.AutoMapper<DO>();
        }
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <param name="id"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DO SingleDOById(IdType id, IDbTransaction tran = null, int? commandTimeout = null)
        {
            PO t = base.SingleById(id, tran, commandTimeout);
            if (t == null)
            {
                return default;
            }
            return t.AutoMapper<DO>();
        }
        #endregion

        #region First
        /// <summary>
        /// 获取第一个满足条件的数据
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <param name="condition"></param>
        /// <param name="query"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DO First(Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            PO t = First(ConditionSQL(query), query, tran, commandTimeout);
            return t.AutoMapper<DO>();
        }
        /// <summary>
        /// 获取第一个满足条件的数据
        /// </summary>
        /// <param name="conditionSql"></param>
        /// <param name="parameters"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        internal PO First(string conditionSql, Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            query.Page.Index = 0;
            query.Page.Size = 1;
            PO t = Search(conditionSql, query, tran, commandTimeout).FirstOrDefault();
            if (t == null)
            {
                return default;
            }
            return t;
        }
        #endregion

        #region Count
        /// <summary>
        /// 查询满足条件的记录数量
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="query"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Count(Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            return Count(ConditionSQL(query), query, tran, commandTimeout);
        }
        /// <summary>
        /// 查询满足条件的记录数量
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="conditionSql"></param>
        /// <param name="query"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Count(string conditionSql, Q query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            string sql = $"{DefaultCountSQL} {conditionSql}";

            DynamicParameters param = new DynamicParameters();
            foreach (PropertyInfo p in (typeof(Q)).GetProperties())
            {
                if (p.Name == "Sort" || p.Name == "Page") continue;
                if (p.GetCustomAttributes(typeof(Newtonsoft.Json.JsonIgnoreAttribute), true).Length == 0)
                {
                    param.Add(p.Name, p.GetValue(query));
                }
            }

            return CountSql(sql, param, tran, commandTimeout);
        }
        #endregion

        #endregion
    }
}
