using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using TianCheng.Service.Core;
using TianCheng.Common;
using DotNetCore.CAP;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 部门管理    [ Service ]
    /// </summary>
    public class DepartmentService : MongoBusinessService<DepartmentDAL, DepartmentInfo, DepartmentQuery>,
        IDeleteService<DepartmentInfo, DepartmentDAL>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public DepartmentService(DepartmentDAL dal) : base(dal)
        {

        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 获取根部门
        /// </summary>
        /// <returns></returns>
        public DepartmentView FirstRoot()
        {
            var info = Dal.Queryable().Where(e => string.IsNullOrEmpty(e.ParentId) && e.IsDelete == false).FirstOrDefault();
            return ObjectTran.Tran<DepartmentView>(info);
        }
        /// <summary>
        /// 获取所有的子部门ID（包含指定部门，及逻辑删除的子部门）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal List<string> GetSubDepartmentId(string id)
        {
            var list = Dal.Queryable().Where(e => e.ParentsIds.Contains(id))
                                   .Select(e => e.Id).ToList();
            return ObjectTran.Tran<List<string>>(list);
        }
        #endregion

        #region 新增 / 修改方法
        /// <summary>
        /// 保存的校验
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void SavingCheck(DepartmentInfo info, TokenBase logonInfo)
        {
            // 数据验证
            if (string.IsNullOrWhiteSpace(info.Name))
            {
                throw ApiException.BadRequest("请指定部门的名称");
            }
        }
        /// <summary>
        /// 保存前，完善数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void Saving(DepartmentInfo info, TokenBase logonInfo)
        {
            // 完善上级部门名称
            if (!string.IsNullOrEmpty(info.ParentId))
            {
                var parent = SingleById(info.ParentId);
                info.ParentName = parent.Name;
                info.ParentsIds = parent.ParentsIds;
                info.RelatedIds = parent.RelatedIds;
            }
            else
            {
                info.ParentId = string.Empty;
                info.ParentName = string.Empty;
                info.ParentsIds = new List<string>();
                info.RelatedIds = new List<string>();
            }

            // 上级部门不能是自己的子部门
            if (!string.IsNullOrEmpty(info.ParentId) && GetSubDepartmentId(info.Id.ToString()).Contains(info.ParentId))
            {
                throw ApiException.BadRequest("上级部门不能是" + info.Name + "的子部门");
            }
        }
        /// <summary>
        /// 保存后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void Saved(DepartmentInfo info, TokenBase logonInfo)
        {
            // 保存后，将当前对象id加入到父id列表中。
            // 有可能修改上级关系，所以放到Saved中而不是Created中。
            if (!info.RelatedIds.Contains(info.IdString))
            {
                info.RelatedIds.Add(info.IdString);
                Dal.UpdateObject(info);
            }
        }
        /// <summary>
        /// 更新的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        protected override void Updated(DepartmentInfo info, DepartmentInfo old, TokenBase logonInfo)
        {
            // 获取消息总线对象。
            ICapPublisher capBus = ServiceLoader.GetService<ICapPublisher>();
            DepartmentUpdateView uv = new DepartmentUpdateView()
            {
                NewDepartment = ObjectTran.Tran<DepartmentView>(info),
                OldDepartment = ObjectTran.Tran<DepartmentView>(old),
                LogonInfo = (StringTokenInfo)logonInfo
            };
            // 发送消息处理
            capBus.Publish(MqBus.DepartmentMq.Update, uv);
        }
        #endregion

        #region 删除方法
        /// <summary>
        /// 删除时的检查
        /// </summary>
        /// <param name="info"></param>
        protected override void DeleteRemoveCheck(DepartmentInfo info)
        {
            string depId = info.Id.ToString();
            // 如果有下级部门不允许删除
            if (Dal.Queryable().Where(e => e.ParentId == depId).Count() > 0)
            {
                throw ApiException.RemoveUsed("删除的部门下有子部门信息，不允许删除。");
            }
            // 如果部门下有员工不允许删除
            EmployeeService employeeService = ServiceLoader.GetService<EmployeeService>();
            if (employeeService.CountByDepartmentId(depId) > 0)
            {
                throw ApiException.RemoveUsed("删除的部门下有员工信息，不允许删除。");
            }
        }
        #endregion

        #region 部门内的员工处理
        /// <summary>
        /// 判断指定的用户是否属于指定的部门(可以是子部门)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="departmentId"></param>
        /// <param name="employeeInfo">返回用户对象</param>
        /// <returns>是否属于指定的部门下</returns>
        public bool HasEmployee(string employeeId, string departmentId, out EmployeeInfo employeeInfo)
        {
            // 获取用户信息
            EmployeeService employeeService = ServiceLoader.GetService<EmployeeService>();
            employeeInfo = employeeService.SingleById(employeeId);
            // 判断用户是否在指定的部门下
            if (employeeInfo == null || employeeInfo.Department == null || string.IsNullOrWhiteSpace(employeeInfo.Department.Id))
            {
                return false;
            }
            // 如果用户属于指定部门直接返回
            if (departmentId == employeeInfo.Department.Id)
            {
                return true;
            }
            // 获取指定部门下所有子部门列表，判断用户是否在子部门中。
            List<string> allDepIds = GetSubDepartmentId(departmentId);
            return allDepIds.Contains(employeeInfo.Department.Id);
        }
        #endregion
    }
}
