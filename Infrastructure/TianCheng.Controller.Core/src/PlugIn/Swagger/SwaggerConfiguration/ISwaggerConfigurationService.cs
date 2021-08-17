using System.Collections.Generic;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// Swagger配置获取接口
    /// </summary>
    public interface ISwaggerConfigurationService
    {
        /// <summary>
        /// 获取所有的Swagger配置（可能包含多个微服务的Swagger配置）
        /// </summary>
        public List<SwaggerDocInfo> Data { get; }
        /// <summary>
        /// 获取所有的Swagger配置（可能包含多个微服务的Swagger配置）
        /// </summary>
        public Dictionary<string, SwaggerDocInfo> SwaggerConfigDict { get; }
        /// <summary>
        /// 获取当前运行项目对应的Swagger配置
        /// </summary>
        public SwaggerDocInfo Current { get; }
    }
}
