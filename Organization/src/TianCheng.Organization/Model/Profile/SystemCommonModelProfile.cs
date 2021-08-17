using AutoMapper;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 系统基础信息的AutoMapper转换
    /// </summary>
    public class SystemCommonModelProfile : Profile, IAutoMapperProfile
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SystemCommonModelProfile()
        {
            Register();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Register()
        {
            CreateMap<EmployeeInfo, EmployeeView>();
            CreateMap<EmployeeView, EmployeeInfo>(MemberList.None);
            CreateMap<EmployeeInfo, SelectView>().ConvertUsing(new EmployeeInfoToSelectView());
            CreateMap<EmployeeInfo, EmployeeSelectView>().ConvertUsing(new EmployeeInfoToEmployeeSelectView());

            CreateMap<FunctionModuleInfo, FunctionModuleView>();
            CreateMap<FunctionGroupInfo, FunctionGroupView>();
            CreateMap<FunctionInfo, FunctionView>();
            CreateMap<FunctionModuleView, FunctionModuleInfo>(MemberList.None);
            CreateMap<FunctionGroupView, FunctionGroupInfo>(MemberList.None);
            CreateMap<FunctionView, FunctionInfo>(MemberList.None);

            //CreateMap<MenuMainInfo, MenuMainView>();
            CreateMap<MenuInfo, MenuMainView>(MemberList.None);
            CreateMap<MenuInfo, MenuSubView>();

            CreateMap<MenuInfo, MenuView>();
            CreateMap<MenuView, MenuInfo>(MemberList.None);

            //CreateMap<MenuMainView, MenuMainInfo>(MemberList.None);
            //CreateMap<MenuSubInfo, MenuSubView>();
            //CreateMap<MenuSubView, MenuSubInfo>().ForMember(dest => dest.Type, opt => opt.Ignore());

            CreateMap<RoleInfo, RoleView>();
            CreateMap<RoleView, RoleInfo>().ConvertUsing(new RoleViewToInfo());
            CreateMap<RoleInfo, SelectView>();
            CreateMap<RoleInfo, RoleSimpleView>();

            CreateMap<DepartmentInfo, DepartmentView>().ForMember(dest => dest.SubList, opt => opt.Ignore());
            CreateMap<DepartmentView, DepartmentInfo>().ConvertUsing(new DepartmentViewToInfo());
            CreateMap<DepartmentInfo, SelectView>();
        }
    }
}
