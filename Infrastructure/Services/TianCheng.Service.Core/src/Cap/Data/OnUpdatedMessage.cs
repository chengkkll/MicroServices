using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 数据更新信息
    /// </summary>
    public class OnUpdatedMessage
    {
        /// <summary>
        /// 业务数据Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 数据编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 数据名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 修改的数据类型
        /// </summary>
        public string TypeFullName { get; set; }
        /// <summary>
        /// 更新数据Json
        /// </summary>
        public string Json { get; set; }
    }
}
