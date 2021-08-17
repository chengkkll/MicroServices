using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// Id为MongoId类型的实体基类
    /// </summary>
    [BsonIgnoreExtraElements(Inherited = true)]
    public class MongoPrimaryKey : IPrimaryKey<ObjectId>
    {
        /// <summary>
        /// ID
        /// </summary>
        [BsonElement("_id")]
        [JsonConverter(typeof(MongoObjectIdConverter))]
        [JsonProperty("id")]
        public ObjectId id { get; set; }

        /// <summary>
        /// 获取ID的字符串格式
        /// </summary>
        [JsonIgnore]
        public string IdString
        {
            get
            {
                return Check(id) ? id.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// 判断对象是否为空对象
        /// </summary>
        [JsonIgnore]
        public bool IdEmpty
        {
            get
            {
                return !Check(id);
            }
        }

        /// <summary>
        /// 设置对象ID为空
        /// </summary>
        public void SetEmpty()
        {
            id = ObjectId.Empty;
        }

        /// <summary>
        /// 检查当前对象ID是否正确
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true正确ID   false不可用ID</returns>
        public bool CheckId(ObjectId id)
        {
            return Check(id);
        }

        /// <summary>
        /// 检查id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool Check(ObjectId id)
        {
            return !(id == null || id == ObjectId.Empty || string.IsNullOrEmpty(id.ToString()) || id.Timestamp == 0 || id.Machine == 0 || id.Increment == 0);
        }

        /// <summary>
        /// 设置对象ID，如果传入的ID无效，返回false
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public bool SetId(string strId)
        {
            // 检查ID是否有效
            var id = ConvertID(strId);
            if (!Check(id))
            {
                return false;
            }

            this.id = id;
            return true;
        }

        /// <summary>
        /// 转化ID
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public ObjectId ConvertID(string strId)
        {
            if (ObjectId.TryParse(strId, out ObjectId id))
            {
                return id;
            }
            return ObjectId.Empty;
        }
    }
}
