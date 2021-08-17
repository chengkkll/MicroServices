﻿using AutoMapper;
using System;

namespace TianCheng.Common
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
            // 时间与字符串的处理
            CreateMap<string, DateTime>().ConvertUsing(new StringToDateTimeConverter());
            CreateMap<DateTime, string>().ConvertUsing(new DateTimeToStringConverter());
            CreateMap<string, DateTime?>().ConvertUsing(new StringToDateTimeNullConverter());
            CreateMap<DateTime?, string>().ConvertUsing(new DateTimeNullToStringConverter());
            CreateMap<string, Guid>().ConvertUsing(new StringToGuidConverter());
            CreateMap<Guid, string>().ConvertUsing(new GuidToStringConverter());
            // MongoDB的ID 与字符串的处理
            CreateMap<string, MongoDB.Bson.ObjectId>().ConvertUsing(new StringToObjectIdConverter());
            CreateMap<MongoDB.Bson.ObjectId, string>().ConvertUsing(new ObjectIdToStringConverter());
            // 返回列表转换
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>)).ConvertUsing(typeof(PagedResultTypeConverter<,>));
        }
    }
}
