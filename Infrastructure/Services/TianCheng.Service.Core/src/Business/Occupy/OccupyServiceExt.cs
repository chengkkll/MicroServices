using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 占用信息的扩展服务
    /// </summary>
    static public class OccupyServiceExt
    {
        #region 占用数据
        #region int
        /// <summary>
        /// 数据占用
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="id">需要占用的数据Id</param>
        /// <param name="controller">controller对象，用于获取登录信息</param>
        /// <returns></returns>
        public static ResultView Occupy<T, DAL>(this IOccupyService<T, DAL> service, int id, ControllerBase controller)
            where T : IntIdModel, IOccupyIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            return service.Occupy(id, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 数据占用
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="id">需要占用的数据Id</param>
        /// <param name="logonInfo">登录人信息</param>
        public static ResultView Occupy<T, DAL>(this IOccupyService<T, DAL> service, int id, TokenBase logonInfo)
            where T : IntIdModel, IOccupyIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            T entity = service.Dal.SingleById(id);
            if (entity == null && entity.IdEmpty)
            {
                service.Log.LogError("数据占用错误，无法根据Id找到数据 ==>类型为：{EntityType} 对象Id为：{Entity} 操作人信息：{LogonInfo}", typeof(T).FullName, id, logonInfo);
                throw ApiException.EmptyData("无法找到要删除的数据");
            }
            return service.Occupy(entity, logonInfo);
        }
        #endregion

        #region Mongo
        /// <summary>
        /// 数据占用
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="id">需要占用的数据Id</param>
        /// <param name="controller">controller对象，用于获取登录信息</param>
        /// <returns></returns>
        public static ResultView Occupy<T, DAL>(this IOccupyService<T, DAL> service, string id, ControllerBase controller)
            where T : MongoIdModel, IOccupyStringModel, new()
            where DAL : MongoOperation<T>
        {
            return service.Occupy(id, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 数据占用
        /// </summary>
        /// <typeparam name="T">操作的实体类型</typeparam>
        /// <typeparam name="DAL">实体类型对应的数据库操作</typeparam>
        /// <param name="service">操作数据的服务对象</param>
        /// <param name="id">需要占用的数据Id</param>
        /// <param name="logonInfo">登录人信息</param>
        public static ResultView Occupy<T, DAL>(this IOccupyService<T, DAL> service, string id, TokenBase logonInfo)
            where T : MongoIdModel, IOccupyStringModel, new()
            where DAL : MongoOperation<T>
        {
            T entity = service.Dal.SingleById(id);
            if (entity == null && entity.IdEmpty)
            {
                service.Log.LogError("数据占用错误，无法根据Id找到数据 ==>类型为：{EntityType} 对象Id为：{Entity} 操作人信息：{LogonInfo}", typeof(T).FullName, id, logonInfo);
                throw ApiException.EmptyData("无法找到要删除的数据");
            }
            return service.Occupy(entity, logonInfo);
        }
        #endregion

        #region Core
        /// <summary>
        /// 数据占用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="DAL"></typeparam>
        /// <param name="service"></param>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public static ResultView Occupy<T, DAL>(this IOccupyService<T, DAL> service, T info, TokenBase logonInfo)
            where T : IOccupyModel, new()
            where DAL : IDBOperation<T>
        {
            if (info == null)
            {
                throw ApiException.EmptyData("无法找到要占用的数据");
            }

            service.Log.LogTrace("占用一条数据 ==> 类型为：{EntityType}  占用人信息：{LogonInfo} 对象：{id}", typeof(T).FullName, logonInfo, info);
            try
            {
                // 设置占用数据
                info.FillOccupier(logonInfo);
                // 持久化数据
                service.Dal.UpdateObject(info);

                // 返回保存结果
                return ResultView.Success();
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "占用一条数据时发生异常 ==> 类型为：{EntityType}  占用人信息：{LogonInfo}  对象：{id}", typeof(T).FullName, logonInfo, info);
                throw;
            }
        }
        #endregion
        #endregion

        #region 清除占用
        /// <summary>
        /// 清除占用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="DAL"></typeparam>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ResultView ClearOccupy<T, DAL>(this IOccupyService<T, DAL> service, string id)
            where T : MongoIdModel, IOccupyStringModel, new()
            where DAL : MongoOperation<T>
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ApiException.ThrowBadRequest("占用id无效");
            }
            service.Log.LogTrace("清除占用一条数据 ==> 类型为：{EntityType}  对象Id:{id}", typeof(T).FullName, id);
            try
            {
                // 清除占用信息
                service.Dal.UpdateObject(id, u => new T()
                {
                    OccupierId = null,
                    OccupierName = "",
                    OccupyDate = null
                });

                // 返回保存结果
                return ResultView.Success(id.ToString());
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "清除占用一条数据异常 ==> 类型为：{EntityType}  对象Id：{id}", typeof(T).FullName, id);
                throw;
            }
        }

        /// <summary>
        /// 清除占用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ResultView ClearOccupy<T, DAL>(this IOccupyService<T, DAL> service, int id)
            where T : IntIdModel, IOccupyIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            if (id == 0)
            {
                ApiException.ThrowBadRequest("占用id无效");
            }
            service.Log.LogTrace("清除占用一条数据 ==> 类型为：{EntityType}  对象Id:{id}", typeof(T).FullName, id);
            try
            {
                service.Dal.UpdateObject(id, u => new T()
                {
                    OccupierId = null,
                    OccupierName = "",
                    OccupyDate = null
                });

                // 返回保存结果
                return ResultView.Success(id.ToString());
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "清除占用一条数据异常 ==> 类型为：{EntityType}  对象Id：{id}", typeof(T).FullName, id);
                throw;
            }
        }
        #endregion

        #region 数据检查与保存
        /// <summary>
        /// 检查实体对象是否被占用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tokenInfo"></param>
        public static void CheckOccupy<T>(this T entity, TokenBase tokenInfo)
        {
            if (entity is IOccupyIntModel model && tokenInfo is IIntIdToken token)
            {
                if (model.OccupierId != token.EmployeeId)
                {
                    ApiException.ThrowBadRequest($"数据被{model.OccupierName}占用，无法修改");
                }
                return;
            }

            if (entity is IOccupyStringModel sm && tokenInfo is IStringIdToken stoken)
            {
                if (sm.OccupierId != stoken.EmployeeId)
                {
                    ApiException.ThrowBadRequest($"数据被{sm.OccupierName}占用，无法修改");
                }
            }
        }
        #endregion

        #region 设置对象信息
        /// <summary>
        /// 设置删除相关信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="logonInfo"></param>
        static public dynamic FillOccupier<T>(this T entity, TokenBase logonInfo)
        {
            dynamic id = logonInfo.GetEmployeeId;
            // 设置逻辑删除状态
            switch (entity)
            {
                case IOccupyIntModel intEntity:
                    intEntity.OccupierId = id;
                    intEntity.OccupierName = logonInfo.Name;
                    intEntity.OccupyDate = DateTime.Now;
                    return id;
                case IOccupyStringModel strEntity:
                    strEntity.OccupierId = id;
                    strEntity.OccupierName = logonInfo.Name;
                    strEntity.OccupyDate = DateTime.Now;
                    return id;
                default:
                    return id;
            }
        }
        /// <summary>
        /// 修改实体对象是清除占用信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        static public void FillClearOccupy<T>(this T entity)
        {
            switch (entity)
            {
                case IOccupyIntModel model:
                    model.OccupierId = null;
                    model.OccupierName = string.Empty;
                    model.OccupyDate = null;
                    return;
                case IOccupyStringModel sm:
                    sm.OccupierId = string.Empty;
                    sm.OccupierName = string.Empty;
                    sm.OccupyDate = null;
                    break;
            }
        }
        #endregion

    }
}
