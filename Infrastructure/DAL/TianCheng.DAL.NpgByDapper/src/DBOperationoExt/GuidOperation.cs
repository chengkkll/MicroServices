using System;
using System.Collections.Generic;
using System.Data;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// Id为Guid类型的数据库操作
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public abstract class GuidOperation<DO, PO, QO> : DOOperation<DO, PO, QO, Guid>
         where PO : GuidPrimaryKey, new()
         where QO : QueryObject
    {

    }

    /// <summary>
    /// Id为Guid类型的数据库操作
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public abstract class GuidOperation<DO, PO> : GuidOperation<DO, PO, QueryObject>
             where PO : GuidPrimaryKey, new()
    {

    }

    /// <summary>
    /// Id为Guid类型的数据库操作
    /// </summary>
    /// <typeparam name="PO"></typeparam>
    public abstract class GuidOperation<PO> : BaseOperation<PO, QueryObject, Guid>
        where PO : GuidPrimaryKey, new()
    {

    }
}
