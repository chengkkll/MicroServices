using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Services.AuthJwt
{
    /// <summary>
    /// Token结果
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Token值
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 到期时间 
        /// </summary>
        public DateTime? Expires { get; set; }
        /// <summary>
        /// Scheme
        /// </summary>
        public string Scheme { get; set; }
    }
}
