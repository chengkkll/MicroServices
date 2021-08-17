using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgSqlByEF
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public class NpgSqlDAL<C, T, Q> : NpgSqlOperation<C, T, Q, int>
        where C : NpgSqlContext, new()
        where Q : QueryObject
        where T : class, IIdModel<int>, new()
    {
        #region 构造方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public NpgSqlDAL(C context) : base(context)
        {

        }
        #endregion
    }
}
