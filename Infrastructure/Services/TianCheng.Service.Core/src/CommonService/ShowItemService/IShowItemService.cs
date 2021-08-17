using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 显示项（列表与查询条件）的显示控制
    /// </summary>
    public interface IShowItemService
    {
        /// <summary>
        /// 设置显示项
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="show"></param>
        /// <param name="type"></param>
        /// <param name="employeeId"></param>
        public void Set<DO, QO>(ShowItem<DO, QO> show, string type, string employeeId) where QO : QueryObject, new();

        /// <summary>
        /// 获取显示项
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="type"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public string Get<DO, QO>(string type, string employeeId) where QO : QueryObject, new();
        /// <summary>
        /// 设置当前用户的默认显示项
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="type"></param>
        /// <param name="employeeId"></param>
        public void SetEmployeeDefault<DO, QO>(string type, string employeeId) where QO : QueryObject, new();

        /// <summary>
        /// 设置显示项的默认值
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="show"></param>
        /// <param name="type"></param>
        public void SetDefault<DO, QO>(ShowItem<DO, QO> show, string type) where QO : QueryObject, new();
        /// <summary>
        /// 获取显示项的默认值
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="type"></param>
        public string GetDefault<DO, QO>(string type) where QO : QueryObject, new();
    }
}
