using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Service.Core;

namespace TianCheng.Organization.Services.Common
{
    /// <summary>
    /// 菜单修改事件
    /// </summary>
    public class MenuServiceExt : IServiceExtOption
    {
        /// <summary>
        /// 设置扩展处理
        /// </summary>
        public void SetOption()
        {
            // 更新的后置处理
            MenuService.OnUpdated += RoleService.OnMenuUpdated;
        }
    }
}
