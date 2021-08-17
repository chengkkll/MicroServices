using System;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// 数据库配置异常
    /// </summary>
    public class NpgConfigurationException : Exception
    {
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="message">异常信息</param>
        public NpgConfigurationException(string message = "无法找到数据库配置信息") : base(message)
        {
            _logger = Serilog.Log.ForContext<NpgConfigurationException>();

            _logger.Error("{Messages}", message);
        }
    }
}
