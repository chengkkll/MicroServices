using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 菜单的更新信息
    /// </summary>
    public class MenuUpdateView
    {
        /// <summary>
        /// 新菜单信息
        /// </summary>
        public MenuMainView NewMenu { get; set; }
        /// <summary>
        /// 旧菜单信息
        /// </summary>
        public MenuMainView OldMenu { get; set; }

        /// <summary>
        /// 操作账号信息
        /// </summary>
        public StringTokenInfo LogonInfo { get; set; }
    }
}
