using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 发布信息
    /// </summary>
    public interface IPublishStringModel
    {
        #region 发布信息
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublish { get; set; }

        /// <summary>
        /// 发布人Id
        /// </summary>
        /// <remarks></remarks>
        public string PublisherId { get; set; }

        /// <summary>
        /// 发布人名称
        /// </summary>
        /// <remarks></remarks>
        public string PublisherName { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        /// <remarks></remarks>
        public DateTime? PublishDate { get; set; }
        #endregion
    }
}
