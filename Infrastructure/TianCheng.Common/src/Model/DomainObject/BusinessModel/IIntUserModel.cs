using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIntUserModel<T> : IBusinessModel<T>
    {
        /// <summary>
        /// 创建人ID
        /// </summary>
        int CreaterId { get; set; }

        /// <summary>
        /// 更新人ID
        /// </summary>
        int UpdaterId { get; set; }
    }
}
