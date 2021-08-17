using Consul;

namespace TianCheng.ConsulHelper
{
    /// <summary>
    /// 服务信息扩展方法
    /// </summary>
    static public class ServiceEntryExt
    {
        /// <summary>
        /// 获取服务器地址
        /// </summary>
        /// <param name="serviceEntry"></param>
        /// <returns></returns>
        static public string ServiceHostAddress(this ServiceEntry serviceEntry)
        {
            if (serviceEntry.Service == null)
            {
                return string.Empty;
            }
            if (serviceEntry.Service.Port == 0)
            {
                return serviceEntry.Service.Address;
            }

            return $"{serviceEntry.Service.Address}:{serviceEntry.Service.Port}";
        }
    }
}
