using Microsoft.EntityFrameworkCore;

namespace TianCheng.DAL.MySqlByEF
{
    /// <summary>
    /// 数据库与对象配置接口
    /// </summary>
    public interface IMySqlConfiguration
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        void Configure(ModelBuilder modelBuilder);
    }
}
