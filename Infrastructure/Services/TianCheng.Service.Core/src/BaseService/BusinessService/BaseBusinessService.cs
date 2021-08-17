using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 基础的业务操作
    /// </summary>
    public class BaseBusinessService : IBusinessService, IServiceRegister, ICapSubscribe, IServiceLogger
    {
        #region 构造方法
        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        public ILogger Log { get; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseBusinessService()
        {
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(GetType());
        }
        #endregion
    }
    /// <summary>
    /// 基本的业务操作 - 需要Id的操作
    /// </summary>
    /// <typeparam name="DAL"></typeparam>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public class BaseBusinessService<DAL, DO, IdType> : OperationActionAndVirtual<DO>, IBusinessService<DO, IdType>, IServiceLogger
        where DO : IIdModel<IdType>, new()
        where DAL : IDBOperation<DO, IdType>, IDBOperation<DO>
    {
        #region 构造方法
        /// <summary>
        /// 仅用于id操作的一个实例
        /// </summary>
        internal static readonly IIdModel<IdType> IdInstance = new DO();
        /// <summary>
        /// 数据操作对象
        /// </summary>
        public DAL Dal { get; }
        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        public ILogger Log { get; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public BaseBusinessService(DAL dal)
        {
            Dal = dal;
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(GetType());
        }
        #endregion

        #region SingleById
        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DO SingleById(IdType id)
        {
            // 检查ID是否有效
            if (!IdInstance.CheckId(id))
            {
                Log.LogWarning("按ID查询对象时，ID无效。{EntityType}{EntityId}", typeof(DO).FullName, id);
                throw ApiException.BadRequest("请求的id无效");
            }
            // 根据ID获取信息
            return Dal.SingleById(id);
        }
        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual View SingleById<View>(IdType id)
        {
            DO entity = SingleById(id);
            return ObjectTran.Tran<View>(entity);
        }
        #endregion

        #region Count
        /// <summary>
        /// 获取所有的记录条数
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return Dal.Count();
        }
        #endregion
    }
}
