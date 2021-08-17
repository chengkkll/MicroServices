using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// Http调用接口
    /// </summary>
    public interface IRestfullApiCall
    {
        /// <summary>
        /// 调用接口
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="serviceName"></param>
        /// <param name="restfulPath"></param>
        /// <param name="balancer"></param>
        /// <param name="postObject"></param>
        /// <returns></returns>
        string Call(string httpMethod, string serviceName, string restfulPath, ILoadBalancer balancer = null, object postObject = null);
    }
}
