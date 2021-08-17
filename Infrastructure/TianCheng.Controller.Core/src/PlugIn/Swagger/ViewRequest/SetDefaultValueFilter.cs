using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 设置默认值
    /// </summary>
    public class SetDefaultValueFilter : ISchemaFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            foreach (PropertyInfo propertyInfo in context.Type.GetProperties())
            {
                DefaultValueAttribute defaultAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();

                if (defaultAttribute != null)
                {
                    foreach (KeyValuePair<string, OpenApiSchema> property in schema.Properties)
                    {
                        if (ToCamelCase(propertyInfo.Name) == property.Key)
                        {
                            property.Value.Example = property.Value.Default;
                            break;
                        }
                    }
                }
            }
        }

        private string ToCamelCase(string name)
        {
            return char.ToLowerInvariant(name[0]) + name[1..];
        }
    }
}
