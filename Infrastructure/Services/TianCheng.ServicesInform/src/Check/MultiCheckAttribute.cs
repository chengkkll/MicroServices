using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.ServicesInform
{
    /// <summary>
    /// 同时多个服务检测
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [AttributeUsage(AttributeTargets.Method)]
    public class MultiCheckAttribute : Attribute
    {
        public string Key
        {
            get
            {
                return GetKey(CommandName);
            }
        }
        [JsonProperty]
        public string CommandName { get; set; }
        [JsonProperty]

        public string HttpMethod { get; set; }

        [JsonProperty]
        public string RouteController { get; set; }

        [JsonProperty]
        public string RouteMethod { get; set; }

        [JsonProperty]
        public string CurrentProject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        public MultiCheckAttribute(string commandName)
        {
            CommandName = commandName;
        }

        static public string GetKey(string commandName)
        {
            return $"ServicesInform::{commandName}";
        }
    }
}
