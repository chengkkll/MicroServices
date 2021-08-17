using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.CAP;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 基本增删改操作
    /// </summary>
    /// <typeparam name="DAL"></typeparam>
    /// <typeparam name="DO"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public class BusinessService<DAL, DO, IdType> : BaseBusinessService<DAL, DO, IdType>
        where DO : IIdModel<IdType>, new()
        where DAL : IDBOperation<DO, IdType>, IDBOperation<DO>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public BusinessService(DAL dal) : base(dal)
        {
        }
        #endregion

        #region 创建
        /// <summary>
        /// 创建一个对象信息
        /// </summary>
        /// <param name="info">新增对象</param>
        /// <param name="controller">当前操作的控制器</param>
        /// <returns></returns>
        public virtual ResultView Create(DO info, ControllerBase controller)
        {
            return Create(info, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 创建一个对象信息
        /// </summary>
        /// <typeparam name="View">传入的对象类型</typeparam>
        /// <param name="view">传入的新增对象</param>
        /// <param name="controller">当前操作的控制器(主要用于获取登录等信息)</param>
        /// <returns></returns>
        public virtual ResultView Create<View>(View view, ControllerBase controller)
        {
            DO info = ObjectTran.Tran<DO>(view);
            return Create(info, controller);
        }
        /// <summary>
        /// 创建一个对象信息
        /// </summary>
        /// <param name="info">新增对象</param>
        /// <param name="logonInfo">登录人信息</param>
        /// <returns></returns>
        public virtual ResultView Create(DO info, TokenBase logonInfo)
        {
            if (info == null)
            {
                ApiException.ThrowBadRequest("新增的对象不能为空");
            }
            Log.LogTrace("新增一个对象信息 ==> 类型为：{EntityType}  操作人信息：{LogonInfo}  实体对象值：{EntityObject}", typeof(DO).FullName, logonInfo, info);

            try
            {
                // 新增数据前的预制数据
                info.FillCreate(logonInfo); // 添加新增数据
                info.FillUpdate(logonInfo); // 添加更新数据
                info.FillAudit();           // 审核处理

                #region 保存验证
                // 保存的数据校验
                SavingCheck(info, logonInfo);
                #endregion

                #region 保存的前置处理
                // 新增的前置操作，可以被重写
                Creating(info, logonInfo);
                // 新增/保存的通用前置操作，可以被重写
                Saving(info, logonInfo);
                #endregion

                // 持久化数据
                Dal.InsertObject(info);

                #region 保存后置处理
                // 新增的通用后置操作，可以被重写
                Created(info, logonInfo);
                // 新增/保存的通用后置操作，可以被重写
                Saved(info, logonInfo);
                #endregion

                // 返回保存结果
                return ResultView.Success(info.IdString);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "新增一个对象信息异常 ==> 类型为：{EntityType}  操作人信息：{LogonInfo}  新增对象：{EntityObject}", typeof(DO).FullName, logonInfo, info);
                throw;
            }
        }
        #endregion

        #region 修改
        /// <summary>
        /// 更新一个对象信息
        /// </summary>
        /// <param name="info">新增对象</param>
        /// <param name="controller">当前操作的控制器</param>
        /// <returns></returns>
        public virtual ResultView Update(DO info, ControllerBase controller)
        {
            return Update(info, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 更新一个对象信息
        /// </summary>
        /// <typeparam name="View">传入的对象类型</typeparam>
        /// <param name="view">传入的更新对象</param>
        /// <param name="controller">当前操作的控制器(主要用于获取登录等信息)</param>
        /// <returns></returns>
        public virtual ResultView Update<View>(View view, ControllerBase controller)
        {
            DO info = ObjectTran.Tran<DO>(view);
            return Update(info, controller);
        }
        /// <summary>
        /// 更新一个对象信息
        /// </summary>
        /// <param name="info">更新对象</param>
        /// <param name="logonInfo">登录人信息</param>
        /// <returns></returns>
        public virtual ResultView Update(DO info, TokenBase logonInfo)
        {
            if (info == null)
            {
                ApiException.ThrowBadRequest("更新对象不能为空");
            }
            Log.LogTrace("更新一个对象信息 ==> 类型为：{EntityType}  操作人信息：{LogonInfo}  更新对象：{EntityObject}", typeof(DO).FullName, logonInfo, info);

            try
            {
                // 根据ID获取原数据信息
                var old = SingleById(info.Id);
                // 检查是否占用
                old.CheckOccupy(logonInfo);

                // 更新数据前的预制数据
                info.FillCreate(old);               // 添加新增数据
                info.FillUpdate(logonInfo);         // 添加更新数据
                info.FillClearOccupy();             // 清除掉占用状态
                info.FillAudit(old);                // 审核处理
                info.FillDelete(old, logonInfo);    // 删除处理

                #region 保存验证
                // 保存的数据校验，可以被重写
                SavingCheck(info, logonInfo);
                #endregion

                #region 保存的前置处理        
                // 更新的前置操作，可以被重写
                Updating(info, old, logonInfo);
                // 新增/保存的通用前置操作，可以被重写
                Saving(info, logonInfo);
                #endregion

                // 持久化数据
                Dal.UpdateObject(info);

                #region 保存后置处理
                // 更新的后置操作，可以被重写
                Updated(info, old, logonInfo);
                // 新增/保存的通用后置操作，可以被重写
                Saved(info, logonInfo);
                // 发送更新的消息队列
                OnCapByUpdated(info, old, logonInfo);
                #endregion

                // 返回保存结果
                return ResultView.Success(info.IdString);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "更新一个对象信息异常 ==> 类型为：{EntityType}  操作人信息：{LogonInfo}  更新对象：{EntityObject}", typeof(DO).FullName, logonInfo, info);
                throw;
            }
        }
        #endregion

        #region 物理删除
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public Task<ResultView> Remove(IdType id, ControllerBase controller)
        {
            return Remove(SingleById(id), controller);
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="info">要删除的数据id</param>
        /// <param name="controller">登录的用户信息</param>
        /// <returns></returns>
        public Task<ResultView> Remove(DO info, ControllerBase controller)
        {
            // 为了做删除的前后置操作，所以先获取要删除的数据，再做删除处理
            return Remove(info, controller.GetIntTokenInfo());
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public async Task<ResultView> Remove(DO info, TokenBase logonInfo)
        {
            if (info == null)
            {
                throw ApiException.EmptyData("无法找到要删除的数据");
            }
            string id = info.Id.ToString();
            Log.LogTrace("物理删除 ==>类型为：{EntityType} 删除Id为：{EntityId} 操作人信息：{LogonInfo}", typeof(DO).FullName, info.Id, logonInfo);
            try
            {
                var mediator = ServiceLoader.GetService<IMediator>();

                // 判断对应数据是否可以删除.
                DeleteRemoveCheck(info);
                await mediator?.Publish(new DeleteRemoveCheckEvent<DO>() { Entity = info, LogonInfo = logonInfo });
                OnDeleteRemoveCheck?.Invoke(info, logonInfo);
                RemoveCheck(info);
                // 验证是否可以删除的事件多播处理
                await mediator?.Publish(new RemoveCheckEvent<DO>() { Entity = info, LogonInfo = logonInfo });
                OnRemoveCheck?.Invoke(info, logonInfo);
                // 物理删除的前置操作
                DeleteRemoving(info, logonInfo);
                Removing(info, logonInfo);
                // 物理删除时的前置事件多播处理
                await mediator?.Publish(new RemovingEvent<DO>() { Entity = info, LogonInfo = logonInfo });
                OnRemoving?.Invoke(info, logonInfo);

                // 持久化数据
                Dal.RemoveObject(info);

                // 物理删除的后置处理
                Removed(info, logonInfo);
                DeleteRemoved(info, logonInfo);
                // 物理删除时的后置事件多播处理
                await mediator?.Publish(new RemovedEvent<DO>() { Entity = info, LogonInfo = logonInfo });
                OnRemoved?.Invoke(info, logonInfo);
                // 返回保存结果
                return ResultView.Success(info.Id.ToString());
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "物理删除异常 ==>类型为：{EntityType} 删除Id为：{EntityId} 操作人信息：{LogonInfo}", typeof(DO).FullName, info.Id, logonInfo);
                throw;
            }
        }
        #endregion
    }
}
