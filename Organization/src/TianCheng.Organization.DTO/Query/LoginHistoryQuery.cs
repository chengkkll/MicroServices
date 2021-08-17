using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 登录历史查询条件
    /// </summary>
    public class LoginHistoryQuery : QueryObject
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Key { get; set; }
    }
}
