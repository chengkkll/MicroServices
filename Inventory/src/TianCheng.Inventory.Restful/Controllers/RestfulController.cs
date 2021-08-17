using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using TianCheng.Common;

namespace TianCheng.Inventory.Restful.Controllers
{
    /// <summary>
    /// Restful 接口
    /// </summary>
    [Produces("application/json")]
    [Route("Restful")]
    [ApiController]
    public class RestfulController : ControllerBase
    {
        /// <summary>
        /// 获取当前请求的服务地址
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "库存管理-接口验证" })]
        [HttpGet("")]
        public string Get()
        {
            // 获取当前服务地址和端口
            var features = ServiceLoader.ApplicationBuilder.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            return $"TianCheng.Inventory Api Service Address ==> [{address}]";
        }
    }
}
