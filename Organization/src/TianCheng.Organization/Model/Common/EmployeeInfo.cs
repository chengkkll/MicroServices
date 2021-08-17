using System;
using System.ComponentModel.DataAnnotations;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public class EmployeeInfo : MongoIdModel, ICreateStringModel, IUpdateStringModel, IDeleteStringModel, IOccupyStringModel
    {
        /// <summary>
        /// 旧系统ID   主要用于数据导入时
        /// </summary>
        public string OldId { get; set; }

        #region 基本信息
        /// <summary>
        /// 员工编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请填写员工名称")]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }
        #endregion

        #region 联系信息
        /// <summary>
        /// 手机电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 座机电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        #endregion

        #region 登录信息
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required(ErrorMessage = "请填写登录账号")]
        public string LogonAccount { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required(ErrorMessage = "请填写登录密码")]
        public string LogonPassword { get; set; }
        /// <summary>
        /// 登录错误的次数
        /// </summary>
        public int LogonWrongNum { get; set; }
        /// <summary>
        /// 锁定账号不允许登录
        /// </summary>
        public bool LogonLock { get; set; }
        /// <summary>
        /// 登录类型
        /// </summary>
        public LoginVerifierType LoginType { get; set; }
        #endregion

        #region 登录令牌信息
        /// <summary>
        /// 动态令牌Id
        /// </summary>
        public string SecureKeyId { get; set; }
        /// <summary>
        /// 动态令牌号
        /// </summary>
        public string SecureKeyMark { get; set; }
        #endregion

        #region 部门信息

        private SelectView _Department = new SelectView();
        /// <summary>
        /// 部门信息
        /// </summary>
        public SelectView Department
        {
            get { return _Department; }
            set
            {
                if (value == null)
                {
                    value = new SelectView();
                }
                _Department = value;
            }
        }
        private ParentDepartment _ParentDep = new ParentDepartment();
        /// <summary>
        /// 上级部门信息
        /// </summary>
        public ParentDepartment ParentDepartment
        {
            get { return _ParentDep; }
            set
            {
                if (value == null)
                {
                    value = new ParentDepartment();
                }
                _ParentDep = value;
            }
        }
        #endregion

        #region 角色信息
        /// <summary>
        /// 角色信息
        /// </summary>
        /// <summary>
        /// 角色信息
        /// </summary>
        public SelectView Role { get; set; } = new SelectView();

        #endregion

        #region 扩展信息
        /// <summary>
        /// 性别
        /// </summary>
        public UserGender Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 血型
        /// </summary>
        public string BloodType { get; set; }
        /// <summary>
        /// 职称 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        #endregion

        #region 状态
        /// <summary>
        /// 状态
        /// </summary>
        public UserState State { get; set; }
        /// <summary>
        /// 状态文本
        /// </summary>
        public string StateText
        {
            get
            {
                if (this == null)
                {
                    return string.Empty;
                }

                return State switch
                {
                    UserState.Disable => "禁用",
                    UserState.Enable => "正常",
                    UserState.LogonLock => "登录已锁",
                    _ => string.Empty,
                };
            }
        }

        /// <summary>
        /// 是否为系统级别数据
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 员工类型
        /// </summary>
        public EmployeeType Type { get; set; }
        #endregion

        /// <summary>
        /// 扩展ID 用于员工信息的扩展
        /// </summary>
        public string ExtId { get; set; }

        #region 新增信息
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreaterName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        #endregion 

        #region 修改信息
        /// <summary>
        /// 修改人Id
        /// </summary>
        public string UpdaterId { get; set; }
        /// <summary>
        /// 更新人名称
        /// </summary>
        public string UpdaterName { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        #endregion 

        #region 逻辑删除
        /// <summary>
        /// 是否已逻辑删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        public string DeleteId { get; set; }
        /// <summary>
        /// 删除人名称
        /// </summary>
        public string DeleteName { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteDate { get; set; }
        #endregion

        #region 占用信息
        /// <summary>
        /// 占用人Id
        /// </summary>
        public string OccupierId { get; set; }
        /// <summary>
        /// 占用人名称
        /// </summary>
        public string OccupierName { get; set; }
        /// <summary>
        /// 占用时间
        /// </summary>
        public DateTime? OccupyDate { get; set; }
        #endregion
    }
}
