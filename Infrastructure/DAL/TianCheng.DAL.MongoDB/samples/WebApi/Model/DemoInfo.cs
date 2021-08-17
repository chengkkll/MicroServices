using MongoDB.Bson.Serialization.Attributes;
using System;
using TianCheng.Common;
using TianCheng.DAL.MongoDB;

namespace WebApi.Model
{
    /// <summary>
    /// Demo对象实体
    /// </summary>
    public class DemoInfo : MongoIdModel
    {
        [BsonElement("Name")]
        public string Name { get; set; }
        /// <summary>
        /// BsonIgnore 可以将忽略属性，不保存进数据库
        /// </summary>
        [BsonIgnore]
        public string Code { get; set; }
        /// <summary>
        /// BsonElement可以为属性起个别名存放
        /// </summary>
        [BsonElement("test_date")]
        public DateTime Date { get; set; }
    }

    public class DemoView
    {
        public string Id { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// BsonElement可以为属性起个别名存放
        /// </summary>
        public DateTime Date { get; set; }
    }
}
