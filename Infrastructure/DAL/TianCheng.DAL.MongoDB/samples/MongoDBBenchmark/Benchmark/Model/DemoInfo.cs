using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace MongoDBBenchmark
{
    /// <summary>
    /// Demo对象实体
    /// </summary>
    public class DemoInfo : MongoIdModel
    {
        //[BsonElement("Name")]
        public string Name { get; set; }
        /// <summary>
        /// BsonIgnore 可以将忽略属性，不保存进数据库
        /// </summary>
        //[BsonIgnore]
        public string Code { get; set; }
        /// <summary>
        /// BsonElement可以为属性起个别名存放
        /// </summary>
        //[BsonElement("test_date")]
        public DateTime Date { get; set; }
    }
}
