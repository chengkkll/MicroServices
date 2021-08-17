using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStringUserModel<T> : IBusinessModel<T>
    {
        /// <summary>
        /// 创建人ID
        /// </summary>
        string CreaterId { get; set; }
        /// <summary>
        /// 更新人ID
        /// </summary>
        string UpdaterId { get; set; }
    }
}
