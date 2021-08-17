using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using TianCheng.Common;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// 获取Swagger配置信息的服务
    /// </summary>
    public class SwaggerConfigurationService : ISwaggerConfigurationService
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public List<SwaggerDocInfo> Data
        {
            get
            {
                // 由于调用次数少，所以每次使用都强制获取一次最新的值
                return Reload();
            }
        }

        /// <summary>
        /// 获取当前运行项目对应的Swagger配置
        /// </summary>
        public Dictionary<string, SwaggerDocInfo> SwaggerConfigDict
        {
            get
            {
                return Data.ToDict();
            }
        }

        /// <summary>
        /// 获取当前运行项目对应的Swagger配置
        /// </summary>
        public SwaggerDocInfo Current
        {
            get
            {
                var data = Data;
                if (data == null)
                {
                    return new SwaggerDocInfo();
                }
                return data.Current();
            }
        }
        /// <summary>
        /// 使用的section名称
        /// </summary>
        private readonly string SectionName;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="sectionName"></param>
        public SwaggerConfigurationService(string sectionName)
        {
            SectionName = sectionName;
        }

        /// <summary>
        /// 从新加载配置
        /// </summary>
        private List<SwaggerDocInfo> Reload()
        {
            // 获取配置信息
            var data = Configuration.Get<List<SwaggerDocInfo>>(SectionName);
            if (data == null)
            {
                return new List<SwaggerDocInfo>();
            }
            return data;
        }
    }
}
