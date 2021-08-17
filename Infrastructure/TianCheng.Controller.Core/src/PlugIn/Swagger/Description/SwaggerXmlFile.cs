using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TianCheng.Common;
using TianCheng.Controller.Core.PlugIn.Swagger;

namespace TianCheng.Controller.Core
{
    /// <summary>
    /// 注释文件的查找和添加
    /// </summary>
    public class SwaggerXmlFile
    {
        /// <summary>
        /// 获取运行路径
        /// </summary>
        static private string ExecutingPath { get; } = System.Environment.CurrentDirectory;// Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// 获取xml文件存储的位置
        /// </summary>
        static private string LibraryComments { get; set; } = System.IO.Path.Combine(ExecutingPath, "LibraryComments");

        /// <summary>
        /// 获取说明的xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static internal List<string> GetXmlFile(string path = "")
        {
            // 获取指定目录 或 运行目录的xml文件
            path = string.IsNullOrWhiteSpace(path) ? LibraryComments : path;
            List<string> fileList = new DirectoryInfo(path).GetFiles("*.xml").Select(f => f.FullName).ToList();

            // 如果有子目录遍历子目录
            foreach (var dir in new DirectoryInfo(path).GetDirectories())
            {
                fileList.AddRange(GetXmlFile(dir.FullName));
            }
            return fileList.Distinct().ToList();
        }

        /// <summary>
        /// 拷贝引用程序集目录下的xml文件
        /// </summary>
        static internal void CopyXmlFile()
        {
            ISwaggerConfigurationService SwaggerConfigurationService = ServiceLoader.GetService<ISwaggerConfigurationService>();
            // 设置页面名称
            var curr = SwaggerConfigurationService.Current;
            string xmlpath = string.IsNullOrWhiteSpace(curr.XmlPath) ? "LibraryComments" : curr.XmlPath;
            LibraryComments = System.IO.Path.Combine(ExecutingPath, xmlpath);
            // 检查并创建目录
            if (!Directory.Exists(LibraryComments))
            {
                Directory.CreateDirectory(LibraryComments);
            }
            // 获取所有程序所在的目录位置
            IEnumerable<string> pathList = AssemblyHelper.GetAssemblyList()
                                                     .Where(a => !string.IsNullOrWhiteSpace(a.Location))
                                                     .Select(a => Path.GetDirectoryName(a.Location))
                                                     .Distinct();
            // 将所有可能的xml文件都拷贝到分析目录
            foreach (string path in pathList)
            {
                try
                {
                    foreach (string file in Directory.GetFiles(path, "*.xml"))
                    {
                        // 将xml文件拷贝到运行目录
                        File.Copy(file, System.IO.Path.Combine(LibraryComments, Path.GetFileName(file)), true);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 从说明文档中提取Controller的注视
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, string> ControllerDescription()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string file in GetXmlFile())
            {
                GetSummary(file, ref dict);
            }
            return dict;
        }

        static private void GetSummary(string file, ref Dictionary<string, string> dict)
        {
            var doc = XDocument.Load(file);
            if (dict == null)
            {
                dict = new Dictionary<string, string>();
            }
            if (doc == null || doc.Root == null || doc.Root.Element("members") == null)
            {
                return;
            }
            //foreach (var doc in doc)
            foreach (var member in doc.Root.Element("members").Elements("member"))
            {
                // 获取control名称
                string memName = member.Attribute("name").Value.ToString();
                if (!memName.EndsWith("Controller"))
                {
                    continue;
                }
                memName = memName[(memName.LastIndexOf(".") + 1)..].Replace("Controller", "");
                if (dict.ContainsKey(memName))
                {
                    continue;
                }

                // 获取control注释
                string summary = member.Element("summary").Value.ToString();
                if (string.IsNullOrWhiteSpace(summary))
                {
                    continue;
                }

                dict.Add(memName, summary.Trim());
            }
        }
    }
}
