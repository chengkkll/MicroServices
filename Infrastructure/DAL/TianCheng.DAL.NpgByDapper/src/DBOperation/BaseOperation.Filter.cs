using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Npgsql;
using Dapper;
using TianCheng.Common;
using System.Linq.Expressions;

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
        public int Count(Q query)
        {
            throw new NotImplementedException();
        }

        public void InsertMany(IEnumerable<PO> entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PO> Queryable()
        {
            throw new NotImplementedException();
        }

        public List<PO> Search(Q query)
        {
            throw new NotImplementedException();
        }

        public List<PO> Search()
        {
            throw new NotImplementedException();
        }

        public List<PO> Search(Expression<Func<PO, bool>> filterFactory)
        {
            throw new NotImplementedException();
        }

        public PagedResult<PO> SearchPages(Q query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectView> Select(Q query)
        {
            throw new NotImplementedException();
        }

        public void UpdateMany(IEnumerable<PO> entities)
        {
            throw new NotImplementedException();
        }

        public void UpdateMany(Expression<Func<PO, bool>> updateQuery, Expression<Func<PO, PO>> updateEntity)
        {
            throw new NotImplementedException();
        }

        void IDBOperation<PO>.UpdateObject(PO entity)
        {
            throw new NotImplementedException();
        }
    }
}
