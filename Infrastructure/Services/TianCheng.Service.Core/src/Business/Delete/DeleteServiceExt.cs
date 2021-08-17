using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using TianCheng.Common;
using TianCheng.DAL;
using TianCheng.Service.Core;
using TianCheng.DAL.MongoDB;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 逻辑删除的服务扩展处理
    /// </summary>
    static public class DeleteIntServiceExt
    {
        #region 逻辑删除
        #region int 
        #region 逻辑删除一组数据
        /// <summary>
        /// 根据ID列表 逻辑删除数据一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        static public bool Delete<T, DAL>(this IDeleteService<T, DAL> service, IEnumerable<int> ids, TokenBase logonInfo)
            where T : IntIdModel, IDeleteIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            try
            {
                foreach (var id in ids)
                {
                    service.Delete(id, logonInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "按ID列表逻辑删除时错误。ID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", ids.ToJson(), typeof(T).FullName);
                throw;
            }
        }
        #endregion

        #region 逻辑删除一个对象
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="T">删除的实体类型</typeparam>
        /// <typeparam name="DAL">执行数据库操作的对象</typeparam>
        /// <param name="service">实体业务服务对象</param>
        /// <param name="id">删除的实体Id</param>
        /// <param name="controller">对外接口控制器，用于获取登录信息</param>
        /// <returns></returns>
        static public ResultView Delete<T, DAL>(this IDeleteService<T, DAL> service, int id, ControllerBase controller)
            where T : IntIdModel, IDeleteIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            return service.Delete(id, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="T">删除的实体类型</typeparam>
        /// <typeparam name="DAL">执行数据库操作的对象</typeparam>
        /// <param name="service">实体业务服务对象</param>
        /// <param name="id">删除的实体Id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        static public ResultView Delete<T, DAL>(this IDeleteService<T, DAL> service, int id, TokenBase logonInfo)
            where T : IntIdModel, IDeleteIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            // 为了做删除的前后置操作，所以先获取要删除的数据，再做删除处理
            var entity = service.Dal.SingleById(id);
            if (entity == null && entity.Id != id)
            {
                service.Log.LogError("逻辑删除错误，无法根据Id找到数据 ==>类型为：{EntityType} 删除Id为：{EntityId} 操作人信息：{LogonInfo}", typeof(T).FullName, id, logonInfo);
                throw ApiException.EmptyData("无法找到要删除的数据");
            }
            return service.Delete(entity, logonInfo);
        }
        #endregion
        #endregion

        #region mongo
        #region 逻辑删除一组数据
        /// <summary>
        /// 根据ID列表 逻辑删除数据一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        static public bool Delete<T, DAL>(this IDeleteService<T, DAL> service, IEnumerable<string> ids, TokenBase logonInfo)
            where T : MongoIdModel, IDeleteStringModel, new()
            where DAL : MongoOperation<T>
        {
            try
            {
                foreach (var id in ids)
                {
                    service.Delete(id, logonInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "按ID列表逻辑删除时错误。ID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", ids.ToJson(), typeof(T).FullName);
                throw;
            }
        }
        #endregion

        #region 逻辑删除一个对象
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="T">删除的实体类型</typeparam>
        /// <typeparam name="DAL">执行数据库操作的对象</typeparam>
        /// <param name="service">实体业务服务对象</param>
        /// <param name="id">删除的实体Id</param>
        /// <param name="controller">对外接口控制器，用于获取登录信息</param>
        /// <returns></returns>
        static public ResultView Delete<T, DAL>(this IDeleteService<T, DAL> service, string id, ControllerBase controller)
            where T : MongoIdModel, IDeleteStringModel, new()
            where DAL : MongoOperation<T>
        {
            return service.Delete(id, controller.GetStringTokenInfo());
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="T">删除的实体类型</typeparam>
        /// <typeparam name="DAL">执行数据库操作的对象</typeparam>
        /// <param name="service">实体业务服务对象</param>
        /// <param name="id">删除的实体Id</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        static public ResultView Delete<T, DAL>(this IDeleteService<T, DAL> service, string id, TokenBase logonInfo)
            where T : MongoIdModel, IDeleteStringModel, new()
            where DAL : MongoOperation<T>
        {
            // 为了做删除的前后置操作，所以先获取要删除的数据，再做删除处理
            var entity = service.Dal.SingleById(id);
            if (entity == null && entity.IdEmpty)
            {
                service.Log.LogError("逻辑删除错误，无法根据Id找到数据 ==>类型为：{EntityType} 删除Id为：{EntityId} 操作人信息：{LogonInfo}", typeof(T).FullName, id, logonInfo);
                throw ApiException.EmptyData("无法找到要删除的数据");
            }
            return service.Delete(entity, logonInfo);
        }
        #endregion
        #endregion

        #region 逻辑删除一组对象
        /// <summary>
        ///  逻辑删除对象列表 
        /// </summary>
        /// <param name="entityList"></param>
        static public bool Delete<T, DAL>(this IDeleteService<T, DAL> service, IEnumerable<T> entityList, TokenBase logonInfo)
            where T : IDeleteModel, new()
            where DAL : IDBOperation<T>
        {
            try
            {
                foreach (var entity in entityList)
                {
                    service.Delete(entity, logonInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "按对象列表逻辑删除时错误。对象列表为：[{Data}]\r\n类型为：[{TypeName}]", entityList.ToJson(), typeof(T).FullName);
                throw;
            }
        }
        #endregion

        #region Core
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="T">删除的实体类型</typeparam>
        /// <typeparam name="DAL">执行数据库操作的对象</typeparam>
        /// <param name="service">实体业务服务对象</param>
        /// <param name="info">删除的实体</param>
        /// <param name="logonInfo">登录的用户信息</param>
        /// <returns></returns>
        static public ResultView Delete<T, DAL>(this IDeleteService<T, DAL> service, T info, TokenBase logonInfo)
            where T : IDeleteModel, new()
            where DAL : IDBOperation<T>
        {
            if (info == null)
            {
                throw ApiException.EmptyData("无法找到要删除的数据");
            }

            service.Log.LogTrace("逻辑删除 ==> 类型为：{EntityType} 删除对象为：{EntityInfo} 操作人信息：{LogonInfo}", typeof(T).FullName, info, logonInfo);
            try
            {
                // 检查是否占用，如果占用会抛出异常
                info.CheckOccupy(logonInfo);

                // 判断对应数据是否可以删除.
                var type = service.GetType();
                type.GetMethod("DeleteRemoveCheck", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info });
                type.GetMethod("DeleteCheck", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info });
                // 逻辑删除的前置操作
                type.GetMethod("DeleteRemoving", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info, logonInfo });
                type.GetMethod("Deleting", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info, logonInfo });

                // 设置逻辑删除状态
                info.FillDelete(logonInfo);

                // 持久化数据
                service.Dal.UpdateObject(info);

                // 逻辑删除的后置处理
                type.GetMethod("Deleted", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info, logonInfo });
                type.GetMethod("DeleteRemoved", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info, logonInfo });

                // 返回保存结果
                return ResultView.Success();
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "逻辑删除异常 ==>类型为：{EntityType} 删除对象为：{EntityInfo} 操作人信息：{LogonInfo}", typeof(T).FullName, info, logonInfo);
                throw;
            }
        }
        #endregion
        #endregion

        #region 还原删除

        #region 还原删除一组数据
        /// <summary>
        ///  还原删除对象列表 
        /// </summary>
        /// <param name="entityList"></param>
        static public bool Undelete<T, DAL>(this IDeleteService<T, DAL> service, IEnumerable<T> entityList, TokenBase logonInfo)
            where T : IDeleteModel, new()
            where DAL : IDBOperation<T>
        {
            try
            {
                // 逐个还原删除
                foreach (var entity in entityList)
                {
                    service.Undelete(entity, logonInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "按对象列表还原删除时错误。对象列表为：[{Data}]\r\n类型为：[{TypeName}]", entityList.ToJson(), typeof(T).FullName);
                throw;
            }
        }
        #endregion

        #region int
        #region 根据Id还原一组数据
        /// <summary>
        /// 根据ID列表 还原删除数据一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        static public bool Undelete<T, DAL>(this IDeleteService<T, DAL> service, IEnumerable<int> ids, TokenBase logonInfo)
            where T : IntIdModel, IDeleteIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            try
            {
                // 逐个还原
                foreach (var id in ids)
                {
                    service.Undelete(id, logonInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "按ID列表还原删除时错误。ID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", ids.ToJson(), typeof(T).FullName);
                throw;
            }
        }
        #endregion

        #region 还原一条数据
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        static public ResultView Undelete<T, DAL>(this IDeleteService<T, DAL> service, int id, ControllerBase controller)
            where T : IntIdModel, IDeleteIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            return service.Undelete(id, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        static public ResultView Undelete<T, DAL>(this IDeleteService<T, DAL> service, int id, TokenBase logonInfo)
            where T : IntIdModel, IDeleteIntModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            var entity = service.Dal.SingleById(id);
            if (entity == null && entity.Id != id)
            {
                service.Log.LogError("还原逻辑删除错误，无法根据Id找到数据 ==>类型为：{EntityType} 删除Id为：{EntityId} 操作人信息：{LogonInfo}", typeof(T).FullName, id, logonInfo);
                throw ApiException.EmptyData("无法找到要还原删除的数据");
            }

            return service.Undelete(entity, logonInfo);
        }
        #endregion
        #endregion

        #region Mongo
        #region 根据Id还原一组数据
        /// <summary>
        /// 根据ID列表 还原删除数据一组数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        static public bool Undelete<T, DAL>(this IDeleteService<T, DAL> service, IEnumerable<string> ids, TokenBase logonInfo)
            where T : MongoIdModel, IDeleteStringModel, new()
            where DAL : MongoOperation<T>
        {
            try
            {
                // 逐个还原
                foreach (var id in ids)
                {
                    service.Undelete(id, logonInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "按ID列表还原删除时错误。ID列表为：[{IdArray}]\r\n类型为：[{TypeName}]", ids.ToJson(), typeof(T).FullName);
                throw;
            }
        }
        #endregion

        #region 还原一条数据
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        static public ResultView Undelete<T, DAL>(this IDeleteService<T, DAL> service, string id, ControllerBase controller)
            where T : MongoIdModel, IDeleteStringModel, new()
            where DAL : MongoOperation<T>
        {
            return service.Undelete(id, controller.GetStringTokenInfo());
        }
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        static public ResultView Undelete<T, DAL>(this IDeleteService<T, DAL> service, string id, TokenBase logonInfo)
            where T : MongoIdModel, IDeleteStringModel, new()
            where DAL : MongoOperation<T>
        {
            var entity = service.Dal.SingleById(id);
            if (entity == null && entity.IdEmpty)
            {
                service.Log.LogError("还原逻辑删除错误，无法根据Id找到数据 ==>类型为：{EntityType} 删除Id为：{EntityId} 操作人信息：{LogonInfo}", typeof(T).FullName, id, logonInfo);
                throw ApiException.EmptyData("无法找到要还原删除的数据");
            }

            return service.Undelete(entity, logonInfo);
        }
        #endregion
        #endregion

        #region Core
        /// <summary>
        /// 取消逻辑删除
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        static public ResultView Undelete<T, DAL>(this IDeleteService<T, DAL> service, T info, TokenBase logonInfo)
            where T : IDeleteModel, new()
            where DAL : IDBOperation<T>
        {
            // 判断id是否有对应数据
            if (info == null)
            {
                throw ApiException.EmptyData("无法找到要还原的数据");
            }

            service.Log.LogTrace("还原删除数据 ==>类型为：{EntityType} 还原对象为：{EntityInfo} 操作人信息：{LogonInfo}", typeof(T).FullName, info, logonInfo);
            try
            {
                // 取消逻辑删除的前置操作
                var type = service.GetType();
                type.GetMethod("UnDeleting", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info, logonInfo });

                // 设置取消删除状态
                info.FillUndelete();
                // 设置更新信息
                info.FillUpdate(logonInfo);

                // 持久化数据
                service.Dal.UpdateObject(info);

                // 还原的后置操作
                type.GetMethod("UnDeleted", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(service, new object[] { info, logonInfo });

                // 返回保存结果
                return ResultView.Success();
            }
            catch (Exception ex)
            {
                service.Log.LogError(ex, "还原删除数据异常 ==>类型为：{EntityType} 还原为：{EntityInfo} 操作人信息：{LogonInfo}", typeof(T).FullName, info, logonInfo);
                throw;
            }
        }
        #endregion
        #endregion

        #region 设置对象信息
        /// <summary>
        /// 设置删除相关信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="logonInfo"></param>
        static public dynamic FillDelete<T>(this T entity, TokenBase logonInfo)
        {
            dynamic id = logonInfo.GetEmployeeId;
            // 设置逻辑删除状态
            switch (entity)
            {
                case IDeleteIntModel intEntity:
                    intEntity.DeleteDate = DateTime.Now;
                    intEntity.DeleteId = id;
                    intEntity.DeleteName = logonInfo.Name;
                    intEntity.IsDelete = true;
                    return id;
                case IDeleteStringModel strEntity:
                    strEntity.DeleteDate = DateTime.Now;
                    strEntity.DeleteId = id;
                    strEntity.DeleteName = logonInfo.Name;
                    strEntity.IsDelete = true;
                    return id;
                default:
                    return id;
            }
        }
        /// <summary>
        /// 修改时填充删除信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="oldEntity"></param>
        static public dynamic FillDelete<T>(this T entity, T oldEntity, TokenBase logonInfo)
        {
            dynamic id = logonInfo.GetEmployeeId;

            if (entity is IDeleteIntModel model && oldEntity is IDeleteIntModel old)
            {
                if (model.IsDelete != old.IsDelete && model.IsDelete == true)
                {
                    model.DeleteId = id;
                    model.DeleteName = logonInfo.Name;
                    model.DeleteDate = DateTime.Now;
                }
                else if (model.IsDelete == false)
                {
                    model.DeleteId = null;
                    model.DeleteName = string.Empty;
                    model.DeleteDate = null;
                }
                return id;
            }

            if (entity is IDeleteStringModel sm && oldEntity is IDeleteStringModel so)
            {
                if (sm.IsDelete != so.IsDelete && sm.IsDelete == true)
                {
                    sm.DeleteId = id;
                    sm.DeleteName = logonInfo.Name;
                    sm.DeleteDate = DateTime.Now;
                }
                else if (sm.IsDelete == false)
                {
                    sm.DeleteId = null;
                    sm.DeleteName = string.Empty;
                    sm.DeleteDate = null;
                }
            }
            return id;
        }

        /// <summary>
        /// 设置取消删除相关信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        static public void FillUndelete<T>(this T entity)
        {
            switch (entity)
            {
                case IDeleteIntModel intEntity:
                    intEntity.DeleteDate = null;
                    intEntity.DeleteId = null;
                    intEntity.DeleteName = string.Empty;
                    intEntity.IsDelete = false;
                    return;
                case IDeleteStringModel strEntity:
                    strEntity.DeleteDate = null;
                    strEntity.DeleteId = string.Empty;
                    strEntity.DeleteName = string.Empty;
                    strEntity.IsDelete = false;
                    return;
            }
        }
        #endregion
    }
}
