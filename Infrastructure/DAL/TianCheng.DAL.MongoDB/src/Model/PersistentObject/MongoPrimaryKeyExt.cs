using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL.MongoDB
{
    /// <summary>
    /// MongoId 的扩展方法
    /// </summary>
    static public class MongoPrimaryKeyExt
    {
        /// <summary>
        /// 获取对象的ID列表
        /// </summary>
        /// <param name="objectList"></param>
        /// <returns></returns>
        static public IEnumerable<string> ToIdList<T>(this List<T> objectList) where T : MongoPrimaryKey
        {
            foreach (MongoPrimaryKey model in objectList)
            {
                yield return model.id.ToString();
            }
        }
        /// <summary>
        /// string -> mongo的ObjectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public ObjectId ToObjectId(this string id)
        {
            if (ObjectId.TryParse(id, out ObjectId _id))
            {
                return _id;
            }
            return ObjectId.Empty;
        }
        /// <summary>
        /// string -> MongoIdModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public MongoPrimaryKey ToMongoId(this string id)
        {
            return new MongoPrimaryKey() { id = id.ToObjectId() };
        }
        /// <summary>
        /// 检查id是否为一个有效的MongoId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public bool CheckMongoId(string id)
        {
            return ObjectId.TryParse(id, out _);
        }
    }
}
