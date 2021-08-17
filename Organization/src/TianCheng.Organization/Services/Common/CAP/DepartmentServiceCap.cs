using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using TianCheng.Common;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using TianCheng.Service.Core;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 部门的消息总线处理程序
    /// </summary>
    public class DepartmentServiceCap : BaseCapSubscribe
    {
        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="updateView"></param>
        [CapSubscribe(MqBus.DepartmentMq.Update, Group = "tiancheng.org.dep.update.sub_dep")]
        public void UpdateSubDepartment(DepartmentUpdateView updateView)
        {
            // 只针对部门名称进行修改
            if (updateView.NewDepartment.Name != updateView.OldDepartment.Name)
            {
                Log.LogTrace("用户{UserName}更新部门时：[消息队列] 更新下级部门。原部门信息：{@OldDepartment}新部门信息{@NewDepartment}", updateView.LogonInfo.Name, updateView.OldDepartment, updateView.NewDepartment);
                DepartmentDAL Dal = ServiceLoader.GetService<DepartmentDAL>();
                // 如果当前部门名称改变，会同时修改其子部门下的关联上级部门名称
                Dal.UpdateParentsDepartmentName(updateView.NewDepartment);
            }
        }

        /// <summary>
        /// 更新员工信息
        /// </summary>
        /// <param name="updateView"></param>
        [CapSubscribe(MqBus.DepartmentMq.Update, Group = "tiancheng.org.dep.update.employee")]
        public void UpdateEmployee(DepartmentUpdateView updateView)
        {
            // 部门信息改变，更新所有的员工信息
            if (updateView.NewDepartment.Name != updateView.OldDepartment.Name ||
                updateView.NewDepartment.ParentId != updateView.OldDepartment.ParentId ||
                updateView.NewDepartment.ParentName != updateView.OldDepartment.ParentName)
            {
                Log.LogTrace("用户{UserName}更新部门信息时：[消息队列] 更新员工信息。原部门信息：{@OldDepartment}新部门信息{@NewDepartment}", updateView.LogonInfo.Name, updateView.OldDepartment, updateView.NewDepartment);
                EmployeeDAL Dal = ServiceLoader.GetService<EmployeeDAL>();
                Dal.UpdateDepartmentInfo(updateView.NewDepartment);
            }
        }
    }
}
