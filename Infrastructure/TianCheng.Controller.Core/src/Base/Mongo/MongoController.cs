using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// Id为Mongo的ObjectId类型的Controller
    /// </summary>
    /// <typeparam name="S">对象操作服务</typeparam>
    public class MongoController<S> : BController<S>
        where S : IBusinessService
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public MongoController(S service) : base(service)
        {
        }
        #endregion
    }
}
