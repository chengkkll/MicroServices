using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 新增操作的扩展服务
    /// </summary>
    static public class CreateServiceExt
    {
        /// <summary>
        /// 新增时填充新增相关信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tokenInfo"></param>
        static public void FillCreate<T>(this T entity, TokenBase tokenInfo)
        {
            if (entity is ICreateIntModel model && tokenInfo is IntTokenInfo token)
            {
                model.CreaterId = token.EmployeeId;
                model.CreaterName = token.Name;
                model.CreateDate = DateTime.Now;
                return;
            }
            if (entity is ICreateStringModel smodel && tokenInfo is StringTokenInfo stoken)
            {
                smodel.CreaterId = stoken.EmployeeId;
                smodel.CreaterName = stoken.Name;
                smodel.CreateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 修改时填充新增信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="oldEntity"></param>
        static public void FillCreate<T>(this T entity, T oldEntity)
        {
            if (entity is ICreateIntModel model && oldEntity is ICreateIntModel old)
            {
                model.CreaterId = old.CreaterId;
                model.CreaterName = old.CreaterName;
                model.CreateDate = old.CreateDate;
                return;
            }
            if (entity is ICreateStringModel sm && oldEntity is ICreateStringModel so)
            {
                sm.CreaterId = so.CreaterId;
                sm.CreaterName = so.CreaterName;
                sm.CreateDate = so.CreateDate;
            }
        }
    }
}
