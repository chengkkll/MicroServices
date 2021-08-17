using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// 修改View对象结构
    /// </summary>
    public class ViewComponentsChange : IDocumentFilter
    {
        /// <summary>  
        /// 设置Controller的注释信息
        /// </summary>  
        /// <param name="swaggerDoc">swagger文档文件</param>  
        /// <param name="context">api接口集合</param>  
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // create
            foreach (var item in CreateViewAttribute.Load())
            {
                if (swaggerDoc.Components.Schemas.ContainsKey(item.Key))
                {
                    var orig = swaggerDoc.Components.Schemas[item.Key];
                    var createView = CreateViewChange(orig);
                    swaggerDoc.Components.Schemas.Add(item.Value, createView);
                }
            }
            // update
            foreach (var item in UpdateViewAttribute.Load())
            {
                if (swaggerDoc.Components.Schemas.ContainsKey(item.Key))
                {
                    var orig = swaggerDoc.Components.Schemas[item.Key];
                    var updateView = UpdateViewChange(orig);
                    swaggerDoc.Components.Schemas.Add(item.Value, updateView);
                }
            }
            // 如果tags没有使用，则删除
            for (int i = 0; i < swaggerDoc.Tags.Count; i++)
            {
                var tag = swaggerDoc.Tags[i];
                if (!swaggerDoc.Paths.Values.SelectMany(p => p.Operations.Values.SelectMany(o => o.Tags)).Any(t => t.Name == tag.Name))
                {
                    swaggerDoc.Tags.RemoveAt(i);
                    i--;
                }
            }

            // 获取注释中，使用Json忽略属性的属性值
            // key = 参数类型；  value = 类型中忽略的属性值列表
            Dictionary<string, List<string>> IgnoreDict = new Dictionary<string, List<string>>();
            foreach (ApiDescription desc in context.ApiDescriptions)
            {
                foreach (ApiParameterDescription para in desc.ParameterDescriptions)
                {
                    if (para.Type == null)
                    {
                        continue;
                    }
                    foreach (PropertyInfo prop in para.Type.GetProperties())
                    {
                        var data = prop.GetCustomAttributes<JsonIgnoreAttribute>();
                        if (data.Count() > 0)
                        {
                            string key = para.Type.Name;
                            if (!IgnoreDict.ContainsKey(para.Type.Name))
                            {
                                IgnoreDict.Add(key, new List<string>());
                            }

                            if (!IgnoreDict[key].Contains(prop.Name))
                            {
                                IgnoreDict[key].Add(prop.Name);
                            }
                        }
                    }
                }
            }
            // 删除忽略的属性值
            foreach (string key in IgnoreDict.Keys)
            {
                if (swaggerDoc.Components.Schemas.ContainsKey(key))
                {
                    var orig = swaggerDoc.Components.Schemas[key];
                    RemoveProperties(orig, IgnoreDict[key].ToArray());
                }
            }
        }
        /// <summary>
        /// 修改CreateView对象
        /// </summary>
        /// <param name="orig"></param>
        /// <returns></returns>
        private OpenApiSchema CreateViewChange(OpenApiSchema orig)
        {
            var schema = Newtonsoft.Json.JsonConvert.DeserializeObject<OpenApiSchema>(Newtonsoft.Json.JsonConvert.SerializeObject(orig));
            return RemoveProperties(schema, "id", "createrId", "createrName", "createDate", "updaterId", "updaterName", "updateDate", "processState", "releaseDate", "isDelete", "idString", "idEmpty");
        }
        /// <summary>
        /// 修改UpdateView对象
        /// </summary>
        /// <param name="orig"></param>
        /// <returns></returns>
        private OpenApiSchema UpdateViewChange(OpenApiSchema orig)
        {
            var schema = Newtonsoft.Json.JsonConvert.DeserializeObject<OpenApiSchema>(Newtonsoft.Json.JsonConvert.SerializeObject(orig));
            return RemoveProperties(schema, "createrId", "createrName", "createDate", "updaterId", "updaterName", "updateDate", "processState", "releaseDate", "isDelete", "idString", "idEmpty");
        }

        /// <summary>
        /// 在OpenApiSchema中删除一组属性
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="removeProperties"></param>
        /// <returns></returns>
        private OpenApiSchema RemoveProperties(OpenApiSchema orig, params string[] removeProperties)
        {
            foreach (string propery in removeProperties)
            {
                RemoveProperty(orig, propery);
            }
            return orig;
        }

        /// <summary>
        /// 在OpenApiSchema中删除一个属性
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="removeProperty"></param>
        private void RemoveProperty(OpenApiSchema orig, string removeProperty)
        {
            string item = string.Empty;
            foreach (string key in orig.Properties.Keys)
            {
                if (key.Equals(removeProperty, StringComparison.OrdinalIgnoreCase))
                {
                    item = key;
                    break;
                }
            }
            if (!string.IsNullOrWhiteSpace(item))
            {
                orig.Properties.Remove(item);
            }
        }
    }
}
