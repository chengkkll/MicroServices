using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.Redis;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 显示项（列表与查询条件）的显示控制   以Redis作为实现的服务方式
    /// </summary>
    public class ShowItemServiceRedis : StringRedis, IShowItemService
    {
        /// <summary>
        /// 操作的数据库序号
        /// </summary>
        protected override int DBIndex { get; } = 1;

        /// <summary>
        /// Key的格式
        /// </summary>
        protected override string KeyFormat { get; } = "ShowItem:{0}";

        #region 获取用户的个人设置
        // Strings      key：ShowItem:typeName:employeeId；   value：用户显示项

        /// <summary>
        /// 存储Key的格式
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private string GetKey(string typeName, string employeeId)
        {
            // key格式=ShowItem:typeName:employeeId 
            return $"{ typeName }:{employeeId}";
        }
        /// <summary>
        /// 保存显示项
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="show"></param>
        /// <param name="type"></param>
        /// <param name="employeeId"></param>
        public void Set<DO, QO>(ShowItem<DO, QO> show, string type, string employeeId) where QO : QueryObject, new()
        {
            Set(show.ToJson(), type, employeeId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="employeeId"></param>
        private void Set(string json, string type, string employeeId)
        {
            Set(GetKey(type, employeeId), json);
        }
        /// <summary>
        /// 设置当前用户的默认显示项
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="type">服务类型名称，用于唯一标识显示项信息</param>
        /// <param name="employeeId"></param>
        public void SetEmployeeDefault<DO, QO>(string type, string employeeId) where QO : QueryObject, new()
        {
            // 获取默认值
            string json = GetDefault<DO, QO>(type);
            // 设置到当前登录用户
            Set(json, type, employeeId);
        }
        /// <summary>
        /// 获取显示项
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public string Get<DO, QO>(string type, string employeeId) where QO : QueryObject, new()
        {
            string json = Get(GetKey(type, employeeId));
            if (string.IsNullOrWhiteSpace(json))
            {
                // 如果没有显示项，获取默认值
                json = GetDefault<DO,QO>(type);
                Set(GetKey(type, employeeId), json);
            }
            return json;
        }
        #endregion

        #region 默认值的设置和获取
        // Strings      key：ShowItem:typeName:Default；   value：用户显示项

        /// <summary>
        /// 默认值Key的格式
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private string GetDefaultKey(string typeName)
        {
            // key格式=ShowItem:typeName:Default 
            return $"{ typeName }:Default";
        }
        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="show"></param>
        /// <param name="type"></param>
        public void SetDefault<DO, QO>(ShowItem<DO, QO> show, string type) where QO : QueryObject, new()
        {
            string json = show.ToJson();

            Set(GetDefaultKey(type), json);
        }
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="type"></param>
        public string GetDefault<DO, QO>(string type) where QO : QueryObject, new()
        {
            string json = Get(GetDefaultKey(type));
            if (string.IsNullOrWhiteSpace(json))
            {
                // 如果没有显示项，设置默认值
                json = new ShowItem<DO, QO>().Default().ToJson();
                Set(GetDefaultKey(type), json);
            }
            return json;
        }
        #endregion
    }
}
