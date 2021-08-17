using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.Common;
using Microsoft.Extensions.Logging;
using TianCheng.DAL;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 审核的通用扩展方法
    /// </summary>
    static public class AuditServiceExt
    {
        #region 送审
        /// <summary>
        /// 送审操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="dataId">需要审核的数据Id</param>
        /// <param name="tokenInfo">登录人信息</param>
        /// <param name="isCheck">是否检测</param>
        public static void AuditStart<T, DAL>(this IAuditService<T, DAL> service, int dataId, IntTokenInfo tokenInfo, bool isCheck = true)
            where T : IntIdModel, IAuditModel, new()
            where DAL : IDBOperation<T>
        {
            // 送审前的检查
            if (isCheck)
            {
                if (service.Dal.Queryable().Count(e => e.AuditState == AuditState.Aduditing && e.Id == dataId) > 0)
                {
                    throw ApiException.BadRequest("数据已送审");
                }
                if (service.Dal.Queryable().Count(e => e.AuditState == AuditState.Approved && e.Id == dataId) > 0)
                {
                    throw ApiException.BadRequest("数据已完成审核流程");
                }
            }

            // 获取消息总线对象。
            ICapPublisher capBus = ServiceLoader.GetService<ICapPublisher>();
            // 发送消息处理
            capBus.Publish(Cap.Workflow.Start, new AuditFlowMessage()
            {
                BusinessCode = service.BusinessCode,
                LoginId = tokenInfo.EmployeeId,
                LoginName = tokenInfo.Name,
                DataId = dataId,
                Operation = WorkflowOperationType.Start
            });
        }
        #endregion

        #region 审核
        /// <summary>
        /// 发布审核处理
        /// </summary>
        /// <param name="service"></param>
        /// <param name="info"></param>
        /// <param name="tokenInfo"></param>
        /// <param name="isCheck">是否检测</param>
        public static void AuditExecute<T, DAL>(this IAuditService<T, DAL> service, AuditResult info, IntTokenInfo tokenInfo, bool isCheck = true)
            where T : IntIdModel, IAuditModel, new()
            where DAL : IDBOperation<T>
        {
            // 权限检测
            if (isCheck)
            {
                if (service.Dal.Queryable().Count(e => e.AuditIds.Contains(tokenInfo.EmployeeId + ",") && e.Id == info.DataId) == 0)
                {
                    throw ApiException.BadRequest("您无权对数据进行审核，请与管理员联系。");
                }
            }

            // 获取消息总线对象。
            ICapPublisher capBus = ServiceLoader.GetService<ICapPublisher>();
            // 发送消息处理
            capBus.Publish(Cap.Workflow.Audit, new AuditFlowMessage()
            {
                BusinessCode = service.BusinessCode,
                LoginId = tokenInfo.EmployeeId,
                LoginName = tokenInfo.Name,
                DataId = info.DataId,
                Content = info.Content,
                Operation = info.Approval ? WorkflowOperationType.Approved : WorkflowOperationType.Rejection
            });
        }
        #endregion

        //#region 催办
        ///// <summary>
        ///// 审核催办
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="DAL"></typeparam>
        ///// <param name="service"></param>
        ///// <param name="dataId">业务Id</param>
        ///// <param name="login">业务Id</param>
        //static public void AuditUrge<T, DAL>(this IAuditService<T, DAL> service, int dataId, IntTokenInfo login)
        //     where T : IntIdModel, IAuditModel, new()
        //    where DAL : IDBOperation<T>, IDBOperation<T, int>
        //{
        //    // 获取业务数据
        //    var info = service.Dal.SingleById(dataId);
        //    // 每个环节最多催办一次
        //    if (info.IsUrge)
        //    {
        //        throw ApiException.BadRequest("已经催办过，每个环节最多只能催办一次");
        //    }
        //    // 获取消息总线对象。
        //    ICapPublisher capBus = ServiceLoader.GetService<ICapPublisher>();
        //    // 发送审核信息
        //    foreach (var id in info.AuditIds.ToIdList())
        //    {
        //        // 发送消息处理
        //        capBus.Publish(Cap.Notice.Message, new NoticeInfo()
        //        {
        //            Type = NoticeType.AuditUrge,
        //            PublisherId = login.Id,
        //            PublisherName = login.Name,
        //            PublisherDate = DateTime.Now,
        //            ReceiverId = id,
        //            LoginId = login.Id,
        //            LoginName = login.Name,
        //            BusinessCode = service.BusinessCode,
        //        });
        //    }
        //    // 发送抄送催办信息
        //    foreach (var id in info.CopyIds.ToIdList())
        //    {
        //        // 发送消息处理
        //        capBus.Publish(Cap.Notice.Message, new NoticeInfo()
        //        {
        //            Type = NoticeType.AuditCopyUrge,
        //            PublisherId = login.Id,
        //            PublisherName = login.Name,
        //            PublisherDate = DateTime.Now,
        //            ReceiverId = id,
        //            LoginId = login.Id,
        //            LoginName = login.Name,
        //            BusinessCode = service.BusinessCode,
        //        });
        //    }
        //    // 更新催办状态
        //    service.Dal.Update(e => e.Id == dataId, u => new T() { IsUrge = true });
        //}
        //#endregion

        #region 操作后的回调
        /// <summary>
        /// 审核的回调的通用处理
        /// </summary>
        /// <param name="flowMessage"></param>
        static public void AuditCallbackCommon<T, DAL>(this IAuditService<T, DAL> service, AuditCallbackMessage flowMessage)
            where T : IntIdModel, IAuditModel, new()
            where DAL : IDBOperation<T>, IDBOperation<T, int>
        {
            service.Log.LogDebug($"AuditCallbakc : {flowMessage.BusinessCode}");
            var info = service.Dal.SingleById(flowMessage.DataId);
            info.AuditState = flowMessage.State;
            info.AuditIds = flowMessage.AuditIds;
            info.CopyIds = flowMessage.CopyIds;
            if (flowMessage.UpdateUrge)
            {
                info.IsUrge = false;
            }
            service.Dal.UpdateObject(info);
        }
        #endregion

        /// <summary>
        /// 新增时填充审核相关信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        static public void FillAudit<T>(this T entity)
        {
            if (entity is IAuditModel model)
            {
                model.AuditState = AuditState.Edit;
            }
        }

        /// <summary>
        /// 修改时填充审核信息以及相关校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="oldEntity"></param>
        static public void FillAudit<T>(this T entity, T oldEntity)
        {
            if (entity is IAuditModel model && oldEntity is IAuditModel old)
            {
                // 验证：已送审后无法修改
                if (old.AuditState == AuditState.Aduditing)
                {
                    throw ApiException.BadRequest("已送审的数据无法再修改");
                }
                // 审核后修改将取消掉已审核的状态
                if (old.AuditState == AuditState.Approved)
                {
                    model.AuditState = AuditState.Edit;
                }
            }
        }
    }
}
