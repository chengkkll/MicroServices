using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// MongoDB
        /// </summary>
        MongoDB = 1,
        /// <summary>
        /// MsSql
        /// </summary>
        MsSql = 2,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 4,
        /// <summary>
        /// PostgreSql
        /// </summary>
        PostgreSql = 8,
        /// <summary>
        /// Redis
        /// </summary>
        Redis = 16
    }
}
