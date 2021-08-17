using Microsoft.EntityFrameworkCore;
using System;
using System.Transactions;

namespace TianCheng.DAL.MySqlByEF
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public static void Invoke<Context>(Action<Context> action) where Context : DbContext, new()
        {
            using Context context = new Context();
            using var tran = context.Database.BeginTransaction();
            try
            {
                action.Invoke(context);
                tran.Commit();
            }
            catch (Exception ex)
            {
                MySqlLog.Logger.Warning(ex, "执行一组数据库操作时出错");
                throw;
            }
        }
    }
}
