﻿using System;

namespace TianCheng.Common
{
    /// <summary>
    /// 业务对象接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBusinessModel<T> : IIdModel<T>
    {
        #region 新增信息
        ///// <summary>
        ///// 创建人ID
        ///// </summary>
        //string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        string CreaterName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateDate { get; set; }
        #endregion

        #region 修改信息
        ///// <summary>
        ///// 更新人ID
        ///// </summary>
        //string UpdaterId { get; set; }
        /// <summary>
        /// 更新人名称
        /// </summary>
        string UpdaterName { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateDate { get; set; }
        #endregion

        #region 逻辑删除
        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        bool IsDelete { get; set; }
        #endregion
    }
}
