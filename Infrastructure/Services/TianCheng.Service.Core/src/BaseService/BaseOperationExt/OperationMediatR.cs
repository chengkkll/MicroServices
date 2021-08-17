using System;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    #region 新增和修改的MediatR事件定义
    #region Check
    #region CreateCheck
    /// <summary>
    /// 创建对象前的检测事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class CreateCheckEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 创建对象前的检测事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class CreateCheckEventHandler<DO> : MediatR.INotificationHandler<CreateCheckEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(CreateCheckEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region UpdateCheck
    /// <summary>
    /// 更新对象前的检测事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class UpdateCheckEvent<DO> : MediatR.INotification
    {
        public DO NewEntity { get; set; }
        public DO OldEntity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 更新对象前的检测事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class UpdateCheckEventHandler<DO> : MediatR.INotificationHandler<UpdateCheckEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(UpdateCheckEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region SaveCheck
    /// <summary>
    /// 保存对象前的检测事件（新增和修改时均会调用）
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class SaveCheckEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 创建对象前的检测事件处理（新增和修改时均会调用）
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class SaveCheckEventHandler<DO> : MediatR.INotificationHandler<SaveCheckEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(SaveCheckEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion
    #endregion

    #region 保存前置操作
    #region Creating
    /// <summary>
    /// 创建对象的前置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class CreatingEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 创建对象的前置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class CreatingEventHandler<DO> : MediatR.INotificationHandler<CreatingEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(CreatingEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region Updating
    /// <summary>
    /// 更新对象的前置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class UpdatingEvent<DO> : MediatR.INotification
    {
        public DO NewEntity { get; set; }
        public DO OldEntity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 更新对象的前置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class UpdatingEventHandler<DO> : MediatR.INotificationHandler<UpdatingEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(UpdatingEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region Saving
    /// <summary>
    /// 保存对象的前置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class SavingEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 保存对象的前置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class SavingEventHandler<DO> : MediatR.INotificationHandler<SavingEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(SavingEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion
    #endregion

    #region 保存后置操作
    #region Created
    /// <summary>
    /// 创建对象的后置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class CreatedEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 创建对象的后置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class CreatedEventHandler<DO> : MediatR.INotificationHandler<CreatedEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(CreatedEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region Updated
    /// <summary>
    /// 更新对象的后置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class UpdatedEvent<DO> : MediatR.INotification
    {
        public DO NewEntity { get; set; }
        public DO OldEntity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 更新对象的后置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class UpdatedEventHandler<DO> : MediatR.INotificationHandler<UpdatedEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(UpdatedEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region Saved
    /// <summary>
    /// 保存对象的后置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class SavedEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 保存对象的后置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class SavedEventHandler<DO> : MediatR.INotificationHandler<SavedEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(SavedEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #endregion
    #endregion

    #region 删除事件处理
    #region Check
    #region DeleteCheck
    /// <summary>
    /// 逻辑删除的检测事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class DeleteCheckEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 逻辑删除的检测事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class DeleteCheckEventHandler<DO> : MediatR.INotificationHandler<DeleteCheckEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(DeleteCheckEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region UnDeleteCheck
    /// <summary>
    /// 恢复逻辑删除的检查事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class UnDeleteCheckEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 恢复逻辑删除的检查事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class UnDeleteCheckEventHandler<DO> : MediatR.INotificationHandler<UnDeleteCheckEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(UnDeleteCheckEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region RemoveCheck
    /// <summary>
    /// 物理删除的检测事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class RemoveCheckEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 物理删除的检测事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class RemoveCheckEventHandler<DO> : MediatR.INotificationHandler<RemoveCheckEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(RemoveCheckEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region DeleteRemoveCheck
    /// <summary>
    /// 所有删除的检测事件（逻辑删除与物理删除时均会调用）
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class DeleteRemoveCheckEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 所有删除的检测事件处理（逻辑删除与物理删除时均会调用）
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class DeleteRemoveCheckEventHandler<DO> : MediatR.INotificationHandler<DeleteRemoveCheckEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(DeleteRemoveCheckEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion
    #endregion

    #region 删除前置操作
    #region Deleting
    /// <summary>
    /// 逻辑删除的前置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class DeletingEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 逻辑删除的前置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class DeletingEventHandler<DO> : MediatR.INotificationHandler<DeletingEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(DeletingEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region UnDeleting
    /// <summary>
    /// 恢复逻辑删除的前置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class UnDeletingEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 恢复逻辑删除的前置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class UnDeletingEventHandler<DO> : MediatR.INotificationHandler<UnDeletingEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(UnDeletingEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region Removing
    /// <summary>
    /// 物理删除的前置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class RemovingEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 物理删除的前置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class RemovingEventHandler<DO> : MediatR.INotificationHandler<RemovingEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(RemovingEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion
    #endregion

    #region 删除后置操作
    #region Deleted
    /// <summary>
    /// 逻辑删除的后置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class DeletedEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 逻辑删除的后置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class DeletedEventHandler<DO> : MediatR.INotificationHandler<DeletedEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(DeletedEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region UnDeleted
    /// <summary>
    /// 恢复逻辑删除的后置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class UnDeletedEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 恢复逻辑删除的后置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class UnDeletedEventHandler<DO> : MediatR.INotificationHandler<UnDeletedEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(UnDeletedEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion

    #region Removed
    /// <summary>
    /// 物理删除的后置事件
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public class RemovedEvent<DO> : MediatR.INotification
    {
        public DO Entity { get; set; }

        public TokenBase LogonInfo { get; set; }
    }
    /// <summary>
    /// 物理删除的后置事件处理
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    /// <remarks>如果检测失败直接抛出异常即可</remarks>
    public abstract class RemovedEventHandler<DO> : MediatR.INotificationHandler<RemovedEvent<DO>>
    {
        public abstract System.Threading.Tasks.Task Handle(RemovedEvent<DO> notification, System.Threading.CancellationToken cancellationToken);
    }
    #endregion
    #endregion
    #endregion
}
