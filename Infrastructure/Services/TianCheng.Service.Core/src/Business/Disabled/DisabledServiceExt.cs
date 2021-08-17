using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 禁用服务扩展操作
    /// </summary>
    static public class DisabledServiceExt
    {
        #region 设置禁用状态
        #region int
        /// <summary>
        /// 设置禁用状态
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="dataId">需要禁用的数据Id</param>
        /// <param name="controller">controller对象，用于获取登录信息</param>
        /// <returns></returns>
        public static ResultView Disable<T, DAL>(this IDisabledService<T, DAL> service, int dataId, ControllerBase controller)
            where T : IntIdModel, IDisabledIntModel, new()
            where DAL : IDBOperation<T>
        {
            return service.Disable(dataId, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 设置禁用状态
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="dataId">需要禁用的数据Id</param>
        /// <param name="tokenInfo">操作人信息</param>
        public static ResultView Disable<T, DAL>(this IDisabledService<T, DAL> service, int dataId, TokenBase tokenInfo)
            where T : IntIdModel, IDisabledIntModel, new()
            where DAL : IDBOperation<T>
        {
            if (dataId == 0)
            {
                ApiException.ThrowBadRequest("禁用id无效");
            }
            service.Log.LogTrace("禁用一条数据 ==> 类型为：{EntityType}  禁用人信息：{LogonInfo} 对象Id:{id}", typeof(T).FullName, tokenInfo, dataId);

            try
            {
                var token = (IIntIdToken)tokenInfo;

                // 按指定的Id更新数据,如果数据已经禁用，不更新
                service.Dal.UpdateMany(t => t.Id == dataId && t.IsDisabled == false, u => new T()
                {
                    IsDisabled = true,
                    DisablerId = token.EmployeeId,
                    DisablerName = tokenInfo.Name,
                    DisabledDate = DateTime.Now
                });

                // 返回保存结果
                return ResultView.Success(dataId.ToString());
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "禁用一条数据时发生异常 ==> 类型为：{EntityType}  禁用人信息：{LogonInfo}  对象Id：{id}", typeof(T).FullName, tokenInfo, dataId);
                throw;
            }
        }
        #endregion

        #region mongo
        /// <summary>
        /// 设置禁用状态
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="id">需要禁用的数据Id</param>
        /// <param name="controller">controller对象，用于获取登录信息</param>
        /// <returns></returns>
        public static ResultView Disable<T, DAL>(this IDisabledService<T, DAL> service, string id, ControllerBase controller)
            where T : MongoIdModel, IDisabledStringModel, new()
            where DAL : MongoOperation<T>
        {
            return service.Disable(id, controller.GetStringTokenInfo());
        }
        /// <summary>
        /// 设置禁用状态
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="id">需要禁用的数据Id</param>
        /// <param name="tokenInfo">操作人信息</param>
        public static ResultView Disable<T, DAL>(this IDisabledService<T, DAL> service, string id, TokenBase tokenInfo)
            where T : MongoIdModel, IDisabledStringModel, new()
            where DAL : MongoOperation<T>
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ApiException.ThrowBadRequest("禁用id无效");
            }
            service.Log.LogTrace("禁用一条数据 ==> 类型为：{EntityType}  禁用人信息：{LogonInfo} 对象Id:{id}", typeof(T).FullName, tokenInfo, id);
            try
            {
                dynamic employeeId = tokenInfo.GetEmployeeId;
                // 按指定的Id更新数据,如果数据已经禁用，不更新
                var info = service.Dal.SingleById(id);
                if(info.IsDisabled == false)
                {
                    info.IsDisabled = true;
                    info.DisablerId = employeeId;
                    info.DisablerName = tokenInfo.Name;
                    info.DisabledDate = DateTime.Now;
                    service.Dal.UpdateObject(info);
                }

                // 返回保存结果
                return ResultView.Success(employeeId);
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "禁用一条数据时发生异常 ==> 类型为：{EntityType}  禁用人信息：{LogonInfo}  对象Id：{id}", typeof(T).FullName, tokenInfo, id);
                throw;
            }
        }
        #endregion
        #endregion

        #region 启用
        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="dataId">需要启用的数据Id</param>
        /// <param name="controller">controller对象，用于获取登录信息</param>
        /// <returns></returns>
        public static ResultView Enable<T, DAL>(this IDisabledService<T, DAL> service, int dataId, ControllerBase controller)
            where T : IntIdModel, IDisabledIntModel, new()
            where DAL : IDBOperation<T>
        {
            return service.Disable(dataId, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="dataId">需要启用的数据Id</param>
        /// <param name="tokenInfo">操作人信息</param>
        public static ResultView Enable<T, DAL>(this IDisabledService<T, DAL> service, int dataId, TokenBase tokenInfo)
            where T : IntIdModel, IDisabledIntModel, new()
            where DAL : IDBOperation<T>
        {
            if (dataId == 0)
            {
                ApiException.ThrowBadRequest("启用id无效");
            }
            service.Log.LogTrace("启用一条数据 ==> 类型为：{EntityType}  启用人信息：{LogonInfo} 对象Id:{id}", typeof(T).FullName, tokenInfo, dataId);

            try
            {
                // 按指定的Id更新数据,如果数据已经启用，不更新
                service.Dal.UpdateMany(t => t.Id == dataId && t.IsDisabled == true, u => new T()
                {
                    IsDisabled = false,
                    DisablerId = null,
                    DisablerName = string.Empty,
                    DisabledDate = null,
                });

                // 返回保存结果
                return ResultView.Success(dataId.ToString());
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "启用一条数据时发生异常 ==> 类型为：{EntityType}  启用人信息：{LogonInfo}  对象Id：{id}", typeof(T).FullName, tokenInfo, dataId);
                throw;
            }
        }
        #endregion
    }
}
