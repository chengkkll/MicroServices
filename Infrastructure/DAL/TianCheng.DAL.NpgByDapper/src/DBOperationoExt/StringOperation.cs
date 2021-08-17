using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// Id类型为string类型的数据库操作
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public abstract class StringOperation<D, P, Q> : DOOperation<D, P, Q, string>
        where P : StringPrimaryKey, new()
        where Q : QueryObject
    {

    }

    /// <summary>
    /// Id类型为string类型的数据库操作
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public abstract class StringOperation<D, P> : DOOperation<D, P, QueryObject, string>
        where P : StringPrimaryKey, new()
    {

    }

    /// <summary>
    /// Id类型为string类型的数据库操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StringOperation<T> : BaseOperation<T, QueryObject, string>
        where T : StringPrimaryKey, new()
    {

    }
}
