using DotNetCore.CAP;
using System.Threading.Tasks;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 业务服务接口
    /// </summary>
    public interface IBusinessService
    {

    }
    /// <summary>
    /// 业务服务接口
    /// </summary>
    public interface IBusinessService<DO> : IBusinessService, IServiceRegister, ICapSubscribe
    {
        /// <summary>
        /// 获取所有的记录条数
        /// </summary>
        /// <returns></returns>
        public long Count();
    }

    /// <summary>
    /// 业务服务接口
    /// </summary>
    public interface IBusinessService<DO, IdType> : IBusinessService<DO>
    {
        #region SingleById
        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO SingleById(IdType id);
        /// <summary>
        /// 根据id查询对象信息  如果无查询结果会抛出异常
        /// </summary>
        /// <typeparam name="View"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        View SingleById<View>(IdType id);
        #endregion
    }
}
