using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// Id类型为int类型的数据库操作
    /// </summary>
    /// <typeparam name="PO"></typeparam>
    public abstract class IntOperation<DO, PO, Q> : DOOperation<DO, PO, Q, int>
        where PO : IntPrimaryKey, new()
        where Q : QueryObject
    {

    }

    ///// <summary>
    ///// Id类型为int类型的数据库操作
    ///// </summary>
    ///// <typeparam name="P"></typeparam>
    //public abstract class IntOperation<D, P> : DOOperation<D, P, QueryObject, int>
    //    where P : IntPrimaryKey, new()
    //{

    //}

    /// <summary>
    /// Id类型为int类型的数据库操作
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public abstract class IntOperation<D, Q> : DOOperation<D, D, Q, int>
        where D : IPrimaryKey<int>, new()
        where Q : QueryObject, new()
    {

    }

    ///// <summary>
    ///// Id类型为int类型的数据库操作
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public abstract class IntOperation<T> : BaseOperation<T, QueryObject, int>
    //    where T : IntPrimaryKey, new()
    //{

    //}
}
