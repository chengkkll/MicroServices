using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL.NpgSqlByEF
{
    /// <summary>
    /// 数据库与对象配置接口
    /// </summary>
    public interface INpgSqlConfiguration
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        void Configure(ModelBuilder modelBuilder);
    }
}
