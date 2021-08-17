﻿using Newtonsoft.Json;

namespace TianCheng.DAL
{
    /// <summary>
    /// int主键
    /// </summary>
    public class IntPrimaryKey : IPrimaryKey<int>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取ID的字符串格式
        /// </summary>
        [JsonIgnore]
        public string IdString
        {
            get
            {
                return Id.ToString();
            }
        }
        /// <summary>
        /// 判断Id是否为空
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public bool IdEmpty
        {
            get
            {
                return !CheckId(this.Id);
            }
        }

        /// <summary>
        /// 检查id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckId(int id)
        {
            return id != 0;
        }
        /// <summary>
        /// 设置对象ID，如果传入的ID无效，返回false
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public bool SetId(string strId)
        {
            // 检查ID是否有效
            if (!int.TryParse(strId, out int id))
            {
                return false;
            }
            if (!CheckId(id))
            {
                return false;
            }

            this.Id = id;
            return true;
        }

        /// <summary>
        /// 转化ID
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public int ConvertID(string strId)
        {
            if (int.TryParse(strId, out int id))
            {
                return id;
            }
            throw new System.ArgumentException("转化id参数无效");
        }
    }
}
