using Microsoft.Extensions.Logging;
using TianCheng.DAL;
using TianCheng.Common;

namespace TianCheng.DAL.MySqlByEF
{
    /// <summary>
    /// Mysql数据库通用操作基类
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public partial class MysqlOperation<C, T, Q, IdType> :
        IMysqlOperation<T, IdType>,
        IMysqlOperationFilter<T, Q>,
        IDBOperationRegister,
        IDBOperationFilter<T, Q>
        where C : MysqlContext, new()
        where Q : QueryObject
        where T : class, IIdModel<IdType>, new()
    {
        #region 构造方法
        /// <summary>
        /// 数据库操作上下文
        /// </summary>
        public C Context { get; private set; }

        /// <summary>
        /// 默认日志操作对象
        /// </summary>
        protected readonly ILogger Log;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        public MysqlOperation(C context)
        {
            Log = ServiceLoader.GetService<ILoggerFactory>()?.CreateLogger(GetType());
            Context = context;
            // 取消跟踪
            Context.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;

            // 目前只有一个数据库，所以在这里做统一设置
            Context.ToMaster();
        }
        #endregion
    }
}
