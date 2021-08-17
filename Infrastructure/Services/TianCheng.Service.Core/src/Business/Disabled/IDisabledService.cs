using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 禁用服务接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="DAL"></typeparam>
    public interface IDisabledService<T, DAL> : IServiceLogger
         where T : IDisabledModel
         where DAL : IDBOperation<T>
    {
        /// <summary>
        /// 获取数据库访问操作
        /// </summary>
        /// <returns></returns>
        public DAL Dal { get; }
    }
}
