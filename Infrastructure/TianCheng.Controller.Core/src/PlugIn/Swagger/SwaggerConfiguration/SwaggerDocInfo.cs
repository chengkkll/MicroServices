using System.Collections.Generic;

namespace TianCheng.Controller.Core.PlugIn.Swagger
{
    /// <summary>
    /// Swagger文档信息
    /// </summary>
    public class SwaggerDocInfo
    {
        /// <summary>
        /// 文档编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 项目所在的命名空间。会按命名空间查询
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 说明  位于标题下面显示的小字信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 服务条款  用于显示服务条款的连接地址
        /// </summary>
        public string TermsOfService { get; set; }
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// 联系信息的Url地址
        /// </summary>
        public string ContactUrl { get; set; }
        /// <summary>
        /// 许可协议，许可协议链接的显示文本
        /// </summary>
        public string LicenseName { get; set; }
        /// <summary>
        /// 许可协议，对应的连接地址
        /// </summary>
        public string LicenseUrl { get; set; }

        /// <summary>
        /// 读取的XML文件相对运行的位置
        /// </summary>
        public string XmlPath { get; set; }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string DocumentTitle { get; set; }
        /// <summary>
        /// 保存swagger的配置文件路径  
        /// </summary>
        /// <remarks> 可在页面右上角选择 </remarks>
        public string EndpointUrl { get; set; }
        /// <summary>
        /// swagger配置文件名称   可在页面右上角选择
        /// </summary>
        /// <remarks> 可在页面右上角选择 </remarks>
        public string EndpointDesc { get; set; }
        /// <summary>
        /// 接口显示的顺序  
        /// </summary>
        /// <remarks> 按指定的命名空间排序 </remarks>
        public List<string> OrderBy { get; set; } = new List<string>();

        /// <summary>
        /// 默认文档内容
        /// </summary>
        static public SwaggerDocInfo Default
        {
            get
            {
                return new SwaggerDocInfo
                {
                    Version = "v1",
                    Title = "演示系统接口文档",
                    Description = "RESTful API for 演示系统接",
                    TermsOfService = "None",
                    ContactName = "",
                    ContactEmail = "",
                    EndpointUrl = "/swagger/v1/swagger.json",
                    EndpointDesc = "演示系统 API V1",
                    XmlPath = "LibraryComments"
                };
            }
        }
    }
}
