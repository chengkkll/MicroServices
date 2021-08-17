using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// 设置Controller的注释信息
    /// </summary>
    public class ControllerDescription : IDocumentFilter
    {
        readonly ISwaggerConfigurationService SwaggerConfigurationService;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="swaggerConfigurationService"></param>
        public ControllerDescription(ISwaggerConfigurationService swaggerConfigurationService)
        {
            SwaggerConfigurationService = swaggerConfigurationService;
        }
        /// <summary>  
        /// 设置Controller的注释信息
        /// </summary>  
        /// <param name="swaggerDoc">swagger文档文件</param>  
        /// <param name="context">api接口集合</param>  
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // 读取项目配置的xml。然后分析，并设置Controller的备注
            Dictionary<string, OpenApiTag> dict = new Dictionary<string, OpenApiTag>();
            Dictionary<string, string> source = SwaggerXmlFile.ControllerDescription();
            // 先按排序规则添加
            foreach (string order in SwaggerConfigurationService.Current.OrderBy)
            {
                foreach (var item in source)
                {
                    if (item.Key.ToLower().Contains(order.ToLower()))
                    {
                        dict.Add(item.Key, new OpenApiTag { Name = item.Key, Description = item.Value });
                    }
                }
            }

            // 其它未指定的排序的重新添加一遍
            foreach (var item in source)
            {
                if (!dict.ContainsKey(item.Key))
                {
                    dict.Add(item.Key, new OpenApiTag { Name = item.Key, Description = item.Value });
                }
            }
            swaggerDoc.Tags = dict.Values.ToList();
        }
    }
}
