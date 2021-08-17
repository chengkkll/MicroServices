using System.Linq;
using TianCheng.Common;

namespace TianCheng.ConsulHelper
{

    /// <summary>
    /// 以Restful的形式进行Http调用
    /// </summary>
    public class RestfulApiCall : IRestfullApiCall
    {
        /// <summary>
        /// 以restufl的形式调用
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="serviceName"></param>
        /// <param name="restfulPath"></param>
        /// <param name="balancer"></param>
        /// <param name="postObject"></param>
        /// <returns></returns>
        public string Call(string httpMethod, string serviceName, string restfulPath, ILoadBalancer balancer = null, object postObject = null)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                return string.Empty;
            }

            // 根据ServiceName获取微服务地址信息
            var loader = ServiceLoader.GetService<IConsulServiceDiscovery>();
            var service = loader.GetServices(serviceName, balancer);
            // 组织请求的api地址
            string apiUrl = restfulPath.StartsWith("/") ? service.ServiceHostAddress() + restfulPath : $"{service.ServiceHostAddress()}/{restfulPath}";
            if (service.Service.Tags.Contains("http"))
            {
                apiUrl = "http://" + apiUrl;
            }
            else
            {
                apiUrl = "https://" + apiUrl;
            }
            // 执行Restful的调用
            return LoadRestfulApi.Load(httpMethod, apiUrl, postObject).Result;
        }
    }
}
