using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using TianCheng.Common;

namespace TianCheng.Organization.Model
{
    /// <summary>
    /// 功能模块
    /// </summary>
    public class FunctionModuleInfo : MongoIdModel, ICreateStringModel, IUpdateStringModel
    {
        /// <summary>
        /// 模块序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 模块编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 功能模块涉及到的程序集
        /// </summary>
        [BsonIgnore]
        public IEnumerable<Assembly> Assemblies { get; set; }

        /// <summary>
        /// 功能模块涉及到的注释文档
        /// </summary>
        [BsonIgnore]
        public IEnumerable<XDocument> Docs { get; set; }

        /// <summary>
        /// 功能点分组（Control）
        /// </summary>
        public List<FunctionGroupInfo> FunctionGroups { get; set; } = new List<FunctionGroupInfo>();

        #region 新增信息
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreaterName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        #endregion

        #region 修改信息
        /// <summary>
        /// 修改人Id
        /// </summary>
        public string UpdaterId { get; set; }
        /// <summary>
        /// 更新人名称
        /// </summary>
        public string UpdaterName { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        #endregion 

    }
}
