using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// 数据库操作对象需要制定操作的表名
    /// </summary>
    public class NpgTableNameNullException : Exception
    {
        private readonly Serilog.ILogger _logger;

        public NpgTableNameNullException() : base("请指定表名")
        {
            _logger = Serilog.Log.ForContext<NpgTableNameNullException>();

            _logger.Error(this, "{Messages}", "请指定表名");
        }

        public NpgTableNameNullException(string message) : base(message)
        {
            _logger = Serilog.Log.ForContext<NpgTableNameNullException>();

            _logger.Error(this, "{Messages}", message);
        }
    }
}
