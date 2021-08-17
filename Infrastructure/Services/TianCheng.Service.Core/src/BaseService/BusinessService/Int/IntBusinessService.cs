using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// Mongo业务操作
    /// </summary>
    public class IntBusinessService<DAL, DO> : BusinessService<DAL, DO, int>
        where DO : IntIdModel, new()
        where DAL : IDBOperation<DO, int>, IDBOperation<DO>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public IntBusinessService(DAL dal) : base(dal)
        {
        }
        #endregion
    }
}
