using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    public class MenuView : NameViewModel
    {
        /// <summary>
        /// 菜单序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 菜单描述
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 菜单的定位
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 主菜单id
        /// </summary>
        public string MainId { get; set; }
    }
}
