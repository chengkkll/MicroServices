using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using TianCheng.Common;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using TianCheng.Service.Core;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 角色的消息总线处理程序
    /// </summary>
    public class RoleServiceCap : BaseCapSubscribe
    {
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="updateView"></param>
        [CapSubscribe(MqBus.RoleMq.Update, Group = "tiancheng.org.role.update.employee")]
        public void UpdateEmployee(RoleUpdateView updateView)
        {
            // 只针对名称名称进行修改
            if (updateView.NewRole.Name != updateView.OldRole.Name)
            {
                Log.LogTrace("用户{UserName}更新角色时：[消息队列] 更新用户的角色名称。原角色名称：{OldName},新角色名称{@NewName}", updateView.LogonInfo.Name, updateView.OldRole.Name, updateView.NewRole.Name);

                EmployeeDAL dal = ServiceLoader.GetService<EmployeeDAL>();
                // 更新部门信息
                dal.UpdateRoleInfo(updateView.NewRole.Id, updateView.NewRole.Name);
            }
        }
    }
}
