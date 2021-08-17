using AutoMapper;
using System;
using TianCheng.Common;

namespace WebApi.Model
{
    /// <summary>
    /// 基础信息的AutoMapper转换
    /// </summary>
    public class ModelProfile : Profile, IAutoMapperProfile
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ModelProfile()
        {
            Register();
        }
        /// <summary>
        /// 注册需要自动AutoMapper的对象信息
        /// </summary>
        public void Register()
        {
            CreateMap<DemoInfo, DemoView>();
            CreateMap<DemoView, DemoInfo>();
        }
    }
}
