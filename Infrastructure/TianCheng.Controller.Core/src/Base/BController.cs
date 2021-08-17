using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 基础的Controller
    /// </summary>
    /// <typeparam name="S"></typeparam>
    public class BController<S> : LogController
        where S : IBusinessService
    {
        #region 构造方法
        /// <summary>
        /// 默认操作的对象的服务
        /// </summary>
        protected readonly S Service;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public BController(S service)
        {
            Service = service;
        }
        #endregion
    }
}
