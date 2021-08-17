using TianCheng.Common;
using TianCheng.DAL;
using TianCheng.Service.Core;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 逻辑删除的接口
    /// </summary>
    public interface IDeleteService<T, DAL> : IServiceLogger
        where T : IDeleteModel
        where DAL : IDBOperation<T>
    {
        /// <summary>
        /// 获取数据库访问操作
        /// </summary>
        /// <returns></returns>
        public DAL Dal { get; }
    }
}
