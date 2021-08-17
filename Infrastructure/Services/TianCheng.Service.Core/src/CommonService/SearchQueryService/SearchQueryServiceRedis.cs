using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.Redis;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// 用户查询条件的Redis实现
    /// </summary>
    public class SearchQueryServiceRedis : StringRedis, ISearchQueryService
    {
        // Strings      key：SearchQuery:typeName:employeeId；   value：用户查询条件

        /// <summary>
        /// 操作的数据库序号
        /// </summary>
        protected override int DBIndex { get; } = 1;

        /// <summary>
        /// Key的格式
        /// </summary>
        protected override string KeyFormat { get; } = "SearchQuery:{0}";

        /// <summary>
        /// 存储Key的格式
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private string GetKey(string typeName, string employeeId)
        {
            // key格式=SearchQuery:typeName:employeeId 
            return $"{ typeName }:{employeeId}";
        }
        /// <summary>
        /// 设置用户查询条件
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="employeeId"></param>
        public void Set<DO, QO>(QO query, string type, string employeeId) where QO : QueryObject
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                // todo : 应该记录日志
                return;
            }

            Set(GetKey(type, employeeId), query.ToJson());
            //string json = query.ToJson();
            //using RedisClient client = new RedisClient(Connection.ServerAddress);
            //client.Set(GetKey(type, employeeId), json);
        }

        /// <summary>
        /// 获取用户的查询条件
        /// </summary>
        /// <typeparam name="DO"></typeparam>
        /// <typeparam name="QO"></typeparam>
        /// <param name="type"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public string Get<DO, QO>(string type, string employeeId) where QO : QueryObject
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                // todo : 应该记录日志
                return string.Empty;
            }
            return Get(GetKey(type, employeeId));
            //using RedisClient client = new RedisClient(Connection.ServerAddress);
            //return client.Get<string>(GetKey(type, employeeId));
        }

    }
}
