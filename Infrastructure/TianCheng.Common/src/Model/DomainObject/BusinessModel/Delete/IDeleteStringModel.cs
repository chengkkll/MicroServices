using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 逻辑删除数据信息接口
    /// </summary>
    public interface IDeleteStringModel : IDeleteModel
    {
        #region 逻辑删除信息
        /// <summary>
        /// 是否已逻辑删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        public string DeleteId { get; set; }
        /// <summary>
        /// 删除人名称
        /// </summary>
        public string DeleteName { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteDate { get; set; }
        #endregion
    }
}
