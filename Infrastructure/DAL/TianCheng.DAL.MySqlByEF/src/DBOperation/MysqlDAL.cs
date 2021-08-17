using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.MySqlByEF
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    public class MysqlDAL<C, T, Q> : MysqlOperation<C, T, Q, int>
        where C : MysqlContext, new()
        where Q : QueryObject
        where T : class, IIdModel<int>, new()
    {
        #region 构造方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public MysqlDAL(C context) : base(context)
        {

        }
        #endregion
    }
}
