using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 存储一致性的数据库信息
    /// </summary>
    public class CapDBOptions
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
