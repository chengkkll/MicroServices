using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Service.Core
{
    /// <summary>
    /// Consul信息
    /// </summary>
    public class CapConsulOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string CurrentScheme { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CurrentNodeHostName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentNodePort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DiscoveryServerHostName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DiscoveryServerPort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NodeName { get; set; }
    }
}
