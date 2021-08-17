using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using System.Xml.Linq;
using TianCheng.Common;
using TianCheng.Organization.DAL;
using TianCheng.Organization.Model;
using TianCheng.Service.Core;
using TianCheng.Services.AuthJwt;

namespace TianCheng.Organization.Services
{
    /// <summary>
    /// 功能点 [ Service ]
    /// </summary>
    public class FunctionService : MongoBusinessService<FunctionDAL, FunctionModuleInfo, FunctionQuery>
    {
        #region 构造方法
        /// <summary>
        /// 配置文件读取服务
        /// </summary>
        private readonly FunctionConfigureService configService;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        ///// <param name="logger"></param>
        ///// <param name="host"></param>
        public FunctionService(FunctionDAL dal) : base(dal)
        {
            configService = ServiceLoader.GetService<FunctionConfigureService>();
        }
        #endregion

        #region 更新权限
        /// <summary>
        /// 初始化功能点数据
        /// </summary>
        /// <returns></returns>
        public Task<ResultView> InitOrganization(TokenBase logonInfo = null)
        {
            // 1、获取模块信息
            List<FunctionModuleInfo> modules = GetModule();
            // 
            return Main(modules, logonInfo);
        }

        /// <summary>
        /// 通过上传程序集，来更新一个模块功能点
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="docs"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public Task<ResultView> AppendModule(List<IFormFile> assemblies, List<IFormFile> docs, string code, string name, TokenBase logonInfo = null)
        {
            // 1、整理添加模块信息
            List<FunctionModuleInfo> modules = GetModule(assemblies, docs, code, name, out string path);
            // 
            return Main(modules, logonInfo, path);
        }
        /// <summary>
        /// 操作的主流程
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="logonInfo"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Task<ResultView> Main(List<FunctionModuleInfo> modules, TokenBase logonInfo, string path = "")
        {
            // 1、填充分组信息
            FillGroup(modules);
            // 2、填充功能点信息
            FillFunctions(modules);
            // 3、删除空的分组信息
            RemoveEmptyGroup(modules);
            // 4、持久化保存模块信息
            SaveToDB(modules, logonInfo);
            // 5、清除临时目录的文件
            if (!string.IsNullOrWhiteSpace(path))
            {
                ClearTempFile(path);
            }
            // 返回操作成功信息
            return ResultView.SuccessTask();
        }
        #endregion

        /// <summary>
        /// 清除临时的文件 
        /// </summary>
        /// <param name="path"></param>
        private void ClearTempFile(string path)
        {
            // todo: 整体功能有问题需完善
            if (!System.IO.Directory.Exists(path))
            {
                return;
            }
            try
            {
                Directory.Delete(path, true);
            }
            catch
            {
                Log.LogWarning($"清除临时目录失败。{path}");
            }
        }

        #region 根据上传数据 更新模块
        /// <summary>
        /// 获取上传更新的模块信息
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="docs"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<FunctionModuleInfo> GetModule(List<IFormFile> assemblies, List<IFormFile> docs, string code, string name, out string path)
        {
            var info = MatchModule(code, name);
            // 完善模块使用的程序集与注释信息
            path = System.IO.Path.Combine(AppContext.BaseDirectory, $"Upload\\temp\\Function\\{Guid.NewGuid()}\\");
            info.Assemblies = GetModuleAssembly(assemblies, path);
            info.Docs = GetModuleDoc(docs, path);
            List<FunctionModuleInfo> modules = new List<FunctionModuleInfo>() { info };
            return modules;
        }
        /// <summary>
        /// 获取上传的程序集
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<Assembly> GetModuleAssembly(List<IFormFile> assemblies, string path)
        {
            // 设置保存的路径
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            // 保存文件
            foreach (IFormFile file in assemblies)
            {
                // 创建一个临时个文件
                string diskFileName = $"{path}\\{file.FileName}";

                // 保存上传的文件
                using System.IO.FileStream fs = System.IO.File.Create(diskFileName);
                file.CopyTo(fs);
                fs.Flush();
                fs.Close();
            }

            List<Assembly> result = new List<Assembly>();
            try
            {
                // 设置程序集依赖路径
                AssemblyDependencyResolver resolver = new AssemblyDependencyResolver(path);
                // 创建加载程序集的对象
                AssemblyLoadContext assemblyLoadContext = new AssemblyLoadContext(Guid.NewGuid().ToString("N"), true);

                foreach (var file in Directory.GetFiles(path))
                {
                    result.Add(assemblyLoadContext.LoadFromAssemblyPath(file));
                }
                assemblyLoadContext.Unload();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取上传的注释文件
        /// </summary>
        /// <param name="docs"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<XDocument> GetModuleDoc(List<IFormFile> docs, string path)
        {
            // 设置保存的路径
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            foreach (IFormFile file in docs)
            {
                // 创建一个临时个文件
                string diskFileName = $"{path}\\{file.FileName}";

                // 保存上传的文件
                using System.IO.FileStream fs = System.IO.File.Create(diskFileName);
                file.CopyTo(fs);
                fs.Flush();
                fs.Close();

                // 读取注释文档
                yield return XDocument.Load(diskFileName);
            }
        }
        #endregion

        #region 根据参数或配置 更新模块信息
        /// <summary>
        /// 获取要更新的模块
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<FunctionModuleInfo> GetModule(string code = "", string name = "")
        {
            Dictionary<string, FunctionModuleInfo> moduleDict = new Dictionary<string, FunctionModuleInfo>();

            // 如果不指定模块编码与名称，从配置文件中读取
            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
            {
                // 设置编号
                int indexSetp = 1;
                // 从配置文件中读取全部的模块信息，并逐一匹配
                if (configService.Options.ModuleDict.Count == 0)
                {
                    ApiException.ThrowBadRequest("请配置appsettings.json中FunctionModule下的ModuleDict节点");
                }
                foreach (var item in configService.Options.ModuleDict)
                {
                    if (string.IsNullOrWhiteSpace(item.Code) || moduleDict.ContainsKey(item.Code))
                    {
                        continue;
                    }
                    var module = MatchModule(item.Code, item.Name, indexSetp++);
                    // 完善模块使用的程序集与注释信息
                    module.Assemblies = GetModuleAssembly(module);
                    module.Docs = GetModuleDoc(module);
                    moduleDict.Add(item.Code, module);
                }

                // 如果配置信息不准确，提示
                if (moduleDict.Count == 0)
                {
                    ApiException.ThrowBadRequest("请检查appsettings.json中FunctionModule下的ModuleDict节点，无有效的功能模块");
                }
            }
            else
            {
                moduleDict.Add(code, MatchModule(code, name));
            }
            return moduleDict.Values.ToList();
        }

        /// <summary>
        /// 匹配模块信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        private FunctionModuleInfo MatchModule(string code, string name = "", int step = 1)
        {
            // 验证传入数据
            if (string.IsNullOrWhiteSpace(code))
            {
                ApiException.ThrowBadRequest("生成权限信息错误，模块编码不能为空");
            }

            // 在数据库中匹配已有的模块信息
            var module = Dal.Queryable().Where(e => e.Code == code).FirstOrDefault();
            // 数据库中不存在，创建新的模块
            if (module == null)
            {
                int index = 1;
                if (Dal.Queryable().Count() > 0)
                {
                    index = Dal.Queryable().Max(e => e.Index) + step;
                }

                module = new FunctionModuleInfo()
                {
                    Index = index,
                    Code = code,
                    Name = string.IsNullOrWhiteSpace(name) ? Guid.NewGuid().ToString() : name,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                // 保存模块信息
                Dal.InsertObject(module);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    module.Name = name;
                }
            }
            return module;
        }

        #region 根据模块信息加载程序集合注释文件
        /// <summary>
        /// 根据模块信息匹配相应的程序集
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        private IEnumerable<Assembly> GetModuleAssembly(FunctionModuleInfo module)
        {
            // 在目录中获取程序集
            foreach (var library in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!library.FullName.Contains(module.Code))
                {
                    continue;
                }
                Assembly assembly = Assembly.Load(new AssemblyName(library.FullName));

                if (assembly != null)
                {
                    yield return assembly;
                }
            }
        }

        /// <summary>
        /// 按路径添加注释文档
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="path"></param>
        private void AppendDocumentPath(Dictionary<string, XDocument> dict, string path)
        {
            foreach (string file in System.IO.Directory.GetFiles(path, "*.xml"))
            {
                AppendDocumentDict(dict, file);
            }
        }
        /// <summary>
        /// 为注释文档字典中添加文档
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="file"></param>
        private void AppendDocumentDict(Dictionary<string, XDocument> dict, string file)
        {
            if (!System.IO.File.Exists(file))
            {
                return;
            }
            string fileName = System.IO.Path.GetFileName(file);
            if (!dict.ContainsKey(fileName))
            {
                dict.Add(fileName, XDocument.Load(file));
            }
        }
        /// <summary>
        /// 默认运行目录下的注释文件所在位置
        /// </summary>
        private const string DefaultExtPath = "LibraryComments";
        /// <summary>
        /// 根据模块信息匹配相应的注释文档
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        private IEnumerable<XDocument> GetModuleDoc(FunctionModuleInfo module)
        {
            // 在目录中获取程序集
            Dictionary<string, XDocument> dict = new Dictionary<string, XDocument>();

            // 获取运行目录
            string basePath = AppContext.BaseDirectory;

            // 获取当前目录的注释文件
            AppendDocumentPath(dict, basePath);
            AppendDocumentPath(dict, System.IO.Path.Combine(basePath, DefaultExtPath));

            // 获取依赖包中的注释文件
            foreach (Microsoft.Extensions.DependencyModel.CompilationLibrary library in Microsoft.Extensions.DependencyModel.DependencyContext.Default.CompileLibraries)
            {
                // 如果与当前模块无关，跳过。
                if (!library.Name.Contains(module.Code) || library.Serviceable || library.Type == "package")
                {
                    continue;
                }

                Assembly assembly = Assembly.Load(new AssemblyName(library.Name));

                if (assembly != null && !string.IsNullOrWhiteSpace(assembly.Location))
                {
                    string assemblyDoc = assembly.Location.Replace(".dll", ".xml");
                    // 如果程序集名称对应的注释文件存在，直添加对应的注释文件
                    if (System.IO.File.Exists(assemblyDoc))
                    {
                        AppendDocumentDict(dict, assemblyDoc);
                    }
                    else
                    {
                        // 获取程序集所在的目录
                        string assemblyPath = System.IO.Path.GetDirectoryName(assembly.Location);
                        // 在程序集所在目录添加注释文件
                        AppendDocumentPath(dict, assemblyPath);
                    }
                }
            }
            return dict.Values.ToList();
        }
        #endregion

        #endregion

        #region 填充分组信息
        /// <summary>
        /// 填充分组信息
        /// </summary>
        /// <param name="modules"></param>
        private List<FunctionModuleInfo> FillGroup(List<FunctionModuleInfo> modules)
        {
            foreach (FunctionModuleInfo module in modules)
            {
                // 遍历所有程序集，查询分组信息
                Dictionary<string, FunctionGroupInfo> groupDict = new Dictionary<string, FunctionGroupInfo>();
                foreach (var assembly in module.Assemblies)
                {
                    foreach (var type in assembly.DefinedTypes) //.GetTypes()
                    {
                        string key = GetTypeSummary(type, module.Docs);
                        if (groupDict.ContainsKey(key))
                        {
                            continue;
                        }
                        // 添加分组信息
                        groupDict.Add(key, new FunctionGroupInfo()
                        {
                            Code = type.Name,
                            Name = key,
                            ModeuleCode = module.Code
                        });

                    }
                }
                module.FunctionGroups = groupDict.Values.ToList();
            }
            return modules;
        }
        #endregion

        #region 填充功能点列表
        /// <summary>
        /// 填充功能点列表
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        private List<FunctionModuleInfo> FillFunctions(List<FunctionModuleInfo> modules)
        {
            foreach (FunctionModuleInfo module in modules)
            {
                // 收集模块下的所有功能点
                List<FunctionInfo> funs = new List<FunctionInfo>();
                foreach (var assembly in module.Assemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        foreach (var method in type.GetMethods())
                        {
                            string policy = GetAuthPolicy(method);
                            // 如果没有策略或策略已经添加到功能列表，跳出当前循环
                            if (string.IsNullOrWhiteSpace(policy) || funs.Where(f => f.Policy == policy).Count() > 0)
                            {
                                continue;
                            }

                            // 创建功能点信息
                            funs.Add(new FunctionInfo()
                            {
                                Policy = policy,
                                Name = GetMethodSummary(type, method, module.Docs),
                                GroupCode = type.Name,
                                ModeuleCode = module.Code
                            });
                        }
                    }
                }

                // 将功能点放置到对应的分组下
                foreach (FunctionInfo fun in funs)
                {
                    var group = module.FunctionGroups.Where(e => e.Code == fun.GroupCode).FirstOrDefault();
                    if (group != null)
                    {
                        fun.Index = group.FunctionList.Count + 1;
                        group.FunctionList.Add(fun);
                    }
                    else
                    {
                        Log.LogWarning("功能点无法确定对应分组：{@fun}", fun);
                    }
                }
            }
            return modules;
        }

        /// <summary>
        /// 获取权限的策略名称
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private string GetAuthPolicy(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<AuthActionAttribute>(false);
            if (attribute != null)
            {
                return attribute.ActionCode;
            }

            var author = method.GetCustomAttribute<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>(false);
            if (author != null)
            {
                return author.Policy;
            }
            return string.Empty;
        }
        #endregion

        #region 删除空的分组信息
        /// <summary>
        /// 删除空的分组信息
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        private List<FunctionModuleInfo> RemoveEmptyGroup(List<FunctionModuleInfo> modules)
        {
            foreach (FunctionModuleInfo module in modules)
            {
                for (int i = 0; i < module.FunctionGroups.Count; i++)
                {
                    if (module.FunctionGroups[i].FunctionList.Count == 0)
                    {
                        module.FunctionGroups.RemoveAt(i);
                        i--;
                        continue;
                    }
                    module.FunctionGroups[i].Index = i + 1;
                }
            }
            return modules;
        }
        #endregion

        #region 持久化模块功能点数据
        /// <summary>
        /// 持久化模块功能点数据
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="logonInfo"></param>
        private void SaveToDB(List<FunctionModuleInfo> modules, TokenBase logonInfo)
        {
            if (logonInfo == null)
            {
                // todo: 加载初始化系统用户信息
                logonInfo = new TokenBase() { };
            }
            foreach (FunctionModuleInfo module in modules)
            {
                if (module.FunctionGroups.Count > 0)
                {
                    Update(module, logonInfo);
                }
            }
        }
        #endregion

        #region 注释文档操作

        /// <summary>
        /// 获取类型说明
        /// </summary>
        /// <param name="type"></param>
        /// <param name="docs"></param>
        /// <returns></returns>
        private string GetTypeSummary(Type type, IEnumerable<XDocument> docs)
        {
            string result = GetSummary($"T:{type.FullName}", docs);
            if (string.IsNullOrWhiteSpace(result))
            {
                Log.LogDebug($"类型{type.FullName}对应的说明文本没有找到");
                return type.FullName;
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// 获取方法的注释
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="docs"></param>
        /// <returns></returns>
        private string GetMethodSummary(Type type, MethodInfo method, IEnumerable<XDocument> docs)
        {
            // 获取功能点名称
            string name = GetSummary($"M:{ type.FullName}.{ method.Name}(", docs);
            if (string.IsNullOrWhiteSpace(name))
            {
                name = GetSummary($"M:{ type.FullName}.{ method.Name}", docs);
            }
            // 如果方法的说明文本有很多时，只取空格前的
            if (name.Contains(" "))
            {
                name = name.Split(' ')[0];
            }
            return name;
        }

        /// <summary>
        /// 在注释文档中查找
        /// </summary>
        /// <param name="key"></param>
        /// <param name="docs"></param>
        /// <returns></returns>
        private string GetSummary(string key, IEnumerable<XDocument> docs)
        {
            foreach (var doc in docs)
                foreach (var ele in doc.Root.Element("members").Elements("member"))
                {
                    //获取control信息
                    var member = ele.Attribute("name").Value.ToString();
                    if (member.Contains(key))
                    {
                        // 如果设置了power 优先使用power信息
                        if (ele.Element("power") != null)
                        {
                            return ele.Element("power").Value?.ToString().Replace("\n", "").Replace("\r", "").Trim();
                        }
                        return ele.Element("summary")?.Value?.ToString().Replace("\n", "").Replace("\r", "").Trim();
                    }
                }
            return string.Empty;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询所有的功能点列表，按树形显示
        /// </summary>
        /// <returns></returns>
        public List<FunctionModuleView> LoadTree()
        {
            var list = Dal.Queryable().ToList();
            return ObjectTran.Tran<List<FunctionModuleView>>(list);
        }
        /// <summary>
        /// 获取所有的功能点列表
        /// </summary>
        /// <returns></returns>
        public List<FunctionView> SearchFunction()
        {
            List<FunctionView> list = new List<FunctionView>();
            var query = Dal.Queryable().ToList();
            foreach (var module in query)
            {
                foreach (var group in module.FunctionGroups)
                {
                    foreach (var fun in group.FunctionList)
                    {
                        list.Add(ObjectTran.Tran<FunctionView>(fun));
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取所有的功能点列表
        /// </summary>
        /// <returns></returns>
        static public List<FunctionView> FunctionList()
        {
            List<FunctionView> list = new List<FunctionView>();
            FunctionDAL _Dal = new FunctionDAL();
            var query = _Dal.Queryable().ToList();
            foreach (var module in query)
            {
                foreach (var group in module.FunctionGroups)
                {
                    foreach (var fun in group.FunctionList)
                    {
                        list.Add(ObjectTran.Tran<FunctionView>(fun));
                    }
                }
            }
            return list;
        }
        #endregion
    }
}
