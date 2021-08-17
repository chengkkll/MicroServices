using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 占用服务接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="DAL"></typeparam>
    public interface IOccupyService<T, DAL> : IServiceLogger
         where T : IOccupyModel
         where DAL : IDBOperation<T>
    {
        /// <summary>
        /// 获取数据库访问操作
        /// </summary>
        /// <returns></returns>
        public DAL Dal { get; }
    }
}
