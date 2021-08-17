using TianCheng.Common;
using TianCheng.DAL;
using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// Id为int类型的Controller
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class IntController<DO> : BController<DO, QueryObject, int>
        where DO : IBusinessModel<int>, new()
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public IntController(ViewBusinessService<DO, QueryObject, int> service) : base(service)
        {
        }
        #endregion
    }

    /// <summary>
    /// Id为int类型的Controller
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="QO"></typeparam>
    public class IntController<DO, QO> : BController<DO, QO, int>
        where DO : IBusinessModel<int>, new()
        where QO : QueryObject
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public IntController(ViewBusinessService<DO, QO, int> service) : base(service)
        {
        }
        #endregion
    }
}
