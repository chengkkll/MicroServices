using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 更新操作的扩展服务
    /// </summary>
    static public class UpdateServiceExt
    {
        /// <summary>
        /// 填充修改相关信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tokenInfo"></param>
        static public dynamic FillUpdate<T>(this T entity, TokenBase tokenInfo)
        {
            if (entity is IUpdateIntModel model && tokenInfo is IIntIdToken token)
            {
                model.UpdaterId = token.EmployeeId;
                model.UpdaterName = tokenInfo.Name;
                model.UpdateDate = DateTime.Now;
                return token.EmployeeId;
            }
            if (entity is ICreateStringModel smodel && tokenInfo is IStringIdToken stoken)
            {
                smodel.CreaterId = stoken.EmployeeId;
                smodel.CreaterName = tokenInfo.Name;
                smodel.CreateDate = DateTime.Now;
                return stoken.EmployeeId;
            }
            return null;
        }
    }
}
