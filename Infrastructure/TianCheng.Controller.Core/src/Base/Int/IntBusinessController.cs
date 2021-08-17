using TianCheng.Common;
using TianCheng.DAL;
using TianCheng.Service.Core;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// Id为int类型的业务操作
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class IntBusinessController<DO> : DefaultBusinessController<DO, DO, QueryObject, int>
        where DO : IBusinessModel<int>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public IntBusinessController(ViewBusinessService<DO, QueryObject, int> service) : base(service)
        {

        }
    }

    /// <summary>
    /// Id为int类型的业务操作
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="QO"></typeparam>
    public class IntBusinessController<DO, QO> : DefaultBusinessController<DO, DO, QO, int>
        where DO : IBusinessModel<int>, new()
        where QO : QueryObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public IntBusinessController(ViewBusinessService<DO, QO, int> service) : base(service)
        {

        }
    }

    /// <summary>
    /// Id为int类型的业务操作
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="VO"></typeparam>
    /// <typeparam name="QO"></typeparam>
    public class IntBusinessController<DO, VO, QO> : DefaultBusinessController<DO, VO, QO, int>
        where DO : IBusinessModel<int>, new()
        where QO : QueryObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public IntBusinessController(ViewBusinessService<DO, QO, int> service) : base(service)
        {

        }
    }
}