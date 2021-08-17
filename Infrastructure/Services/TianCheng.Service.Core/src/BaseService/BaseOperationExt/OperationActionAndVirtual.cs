using DotNetCore.CAP;
using System;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 服务对应的事件、虚方法处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperationActionAndVirtual<T> : ICapSubscribe
    {
        #region 删除事件处理
        /// <summary>
        /// 逻辑删除的前置事件处理
        /// </summary>
        static public Action<T, TokenBase> OnDeleting;
        /// <summary>
        /// 逻辑删除后的事件处理
        /// </summary>
        static public Action<T, TokenBase> OnDeleted;

        /// <summary>
        /// 取消逻辑删除的前置事件处理
        /// </summary>
        static public Action<T, TokenBase> OnUnDeleting;
        /// <summary>
        /// 取消逻辑删除后的事件处理
        /// </summary>
        static public Action<T, TokenBase> OnUnDeleted;

        /// <summary>
        /// 物理删除的前置事件处理
        /// </summary>
        static public Action<T, TokenBase> OnRemoving;
        /// <summary>
        /// 物理删除后的事件处理
        /// </summary>
        static public Action<T, TokenBase> OnRemoved;

        /// <summary>
        /// 逻辑删除的 删除前检测
        /// </summary>
        static public Action<T, TokenBase> OnDeleteCheck;
        /// <summary>
        /// 物理删除的 删除前检测
        /// </summary>
        static public Action<T, TokenBase> OnRemoveCheck;
        /// <summary>
        /// 逻辑与物理删除的 删除前检测
        /// </summary>
        static public Action<T, TokenBase> OnDeleteRemoveCheck;
        #endregion

        #region 新增和修改事件处理
        /// <summary>
        /// 新增前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
        /// </summary>
        static public Action<T, TokenBase> OnCreateCheck;
        /// <summary>
        /// 新增前置事件
        /// </summary>
        static public Action<T, TokenBase> OnCreating;
        /// <summary>
        /// 新增后置事件
        /// </summary>
        static public Action<T, TokenBase> OnCreated;

        /// <summary>
        /// 更新前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
        /// </summary>
        static public Action<T, T, TokenBase> OnUpdateCheck;
        /// <summary>
        /// 更新前置事件
        /// </summary>
        static public Action<T, T, TokenBase> OnUpdating;
        /// <summary>
        /// 更新后置事件
        /// </summary>
        static public Action<T, T, TokenBase> OnUpdated;

        /// <summary>
        /// 保存前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存
        /// </summary>
        static public Action<T, TokenBase> OnSaveCheck;
        /// <summary>
        /// 保存前置事件
        /// </summary>
        static public Action<T, TokenBase> OnSaving;
        /// <summary>
        /// 保存后置事件
        /// </summary>
        static public Action<T, TokenBase> OnSaved;
        #endregion

        #region 派生类中可重写的保存相关方法
        /// <summary>
        /// 更新的前置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Creating(T info, TokenBase logonInfo) { }
        /// <summary>
        /// 更新的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Created(T info, TokenBase logonInfo) { }

        /// <summary>
        /// 更新的前置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Updating(T info, T old, TokenBase logonInfo) { }

        /// <summary>
        /// 更新的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Updated(T info, T old, TokenBase logonInfo) { }

        /// <summary>
        /// 保存的前置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Saving(T info, TokenBase logonInfo) { }

        /// <summary>
        /// 保存的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Saved(T info, TokenBase logonInfo) { }

        /// <summary>
        /// 保存前的数据校验
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void SavingCheck(T info, TokenBase logonInfo) { }

        /// <summary>
        /// 是否可以执行新增操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        protected virtual bool IsExecuteCreate(T info, TokenBase logonInfo) { return true; }

        /// <summary>
        /// 是否可以执行保存操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        protected virtual bool IsExecuteUpdate(T info, T old, TokenBase logonInfo) { return true; }
        #endregion

        #region 派生类中可重写的方法
        /// <summary>
        /// 删除的通用验证处理
        /// </summary>
        /// <param name="info"></param>
        protected virtual void DeleteRemoveCheck(T info)
        {
            //如果验证有问题抛出异常
        }
        /// <summary>
        /// 逻辑删除的验证处理
        /// </summary>
        /// <param name="info"></param>
        protected virtual void DeleteCheck(T info)
        {
            //如果验证有问题抛出异常
        }
        /// <summary>
        /// 物理删除的验证处理
        /// </summary>
        /// <param name="info"></param>
        protected virtual void RemoveCheck(T info)
        {
            //如果验证有问题抛出异常
        }

        /// <summary>
        /// 删除的通用前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void DeleteRemoving(T info, TokenBase logonInfo)
        {
        }
        /// <summary>
        /// 逻辑删除的前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Deleting(T info, TokenBase logonInfo)
        {
        }
        /// <summary>
        /// 取消逻辑删除的前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void UnDeleting(T info, TokenBase logonInfo)
        {
        }
        /// <summary>
        /// 物理删除的前置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void Removing(T info, TokenBase logonInfo)
        {
        }

        /// <summary>
        /// 逻辑删除的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo">登录的用户信息</param>
        protected virtual void Deleted(T info, TokenBase logonInfo) { }
        /// <summary>
        /// 取消逻辑删除的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo">登录的用户信息</param>
        protected virtual void UnDeleted(T info, TokenBase logonInfo) { }
        /// <summary>
        /// 物理删除的后置操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo">登录的用户信息</param>
        protected virtual void Removed(T info, TokenBase logonInfo) { }
        /// <summary>
        /// 删除的通用后置处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected virtual void DeleteRemoved(T info, TokenBase logonInfo)
        {
        }
        #endregion

        #region Cap 消息处理
        #region 数据更新处理
        /// <summary>
        /// 数据更新后是否发送Cap消息队列
        /// </summary>
        protected virtual bool SendCapByUpdated { get; } = false;
        /// <summary>
        /// 数据更新时的消息队里处理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="old"></param>
        /// <param name="logonInfo"></param>
        protected virtual void OnCapByUpdated(T info, T old, TokenBase logonInfo)
        {
            if (!SendCapByUpdated) return;
            string code1 = info is ICode cInfo ? cInfo.Code : string.Empty;
            string code2 = old is ICode cOld ? cOld.Code : string.Empty;
            string name1 = info is IName nInfo ? nInfo.Name : string.Empty;
            string name2 = old is IName nOld ? nOld.Name : string.Empty;

            // 判断数据是否有改变
            if (code1 != code2 || name1 != name2)
            {
                string id = string.Empty;
                //string typeKey = string.Empty;
                if (info is IIdModel model)
                {
                    id = model.IdString;
                    //typeKey = model.TypeKey();
                }
                // 发送更新消息
                ICapPublisher capBus = ServiceLoader.GetService<ICapPublisher>();
                capBus.Publish(Cap.Data.CodeNameUpdated, typeof(T), new OnUpdatedMessage()
                {
                    Id = id,
                    Code = code1,
                    Name = name1,
                    TypeFullName = typeof(T).FullName
                });
            }

        }
        #endregion
        #endregion
    }
}
