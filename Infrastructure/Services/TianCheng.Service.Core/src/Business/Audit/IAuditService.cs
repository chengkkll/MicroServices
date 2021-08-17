using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 审核服务接口
    /// </summary>
    public interface IAuditService<T, IdType, DAL> : ICapSubscribe, IServiceLogger
        where DAL : IDBOperation<T, IdType>, IDBOperation<T>
    {
        /// <summary>
        /// 获取业务编码
        /// </summary>
        public string BusinessCode { get; }
        /// <summary>
        /// 获取数据库访问操作
        /// </summary>
        /// <returns></returns>
        public DAL Dal { get; }
    }

    /// <summary>
    /// 审核服务接口
    /// </summary>
    public interface IAuditService<T, DAL> : ICapSubscribe, IServiceLogger
        where DAL : IDBOperation<T>
    {
        /// <summary>
        /// 获取业务编码
        /// </summary>
        public string BusinessCode { get; }
        /// <summary>
        /// 获取数据库访问操作
        /// </summary>
        /// <returns></returns>
        public DAL Dal { get; }
    }
}
