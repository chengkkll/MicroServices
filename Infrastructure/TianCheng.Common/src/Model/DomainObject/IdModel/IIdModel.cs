namespace TianCheng.Common
{
    /// <summary>
    /// Id实体基类接口
    /// </summary>
    public interface IIdModel
    {
        /// <summary>
        /// 获取ID的字符串格式
        /// </summary>
        string IdString { get; }
    }
    /// <summary>
    /// Id实体扩展方法
    /// </summary>
    static public class IIdModelExt
    {
        /// <summary>
        /// 获取类型的唯一名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        static public string TypeKey(this IIdModel model)
        {
            string key = model.GetType().ToString();
            return key.Replace(".Model.",".").Replace(".DTO.", ".").Replace("DTO", "").Replace("Info", "").Replace("View", "");
        }
    }
    /// <summary>
    /// ID实体基类接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdModel<T> : IIdModel
    {
        /// <summary>
        /// Id
        /// </summary>
        T Id { get; set; }
        /// <summary>
        /// 判断ID是否为空
        /// </summary>
        /// <returns></returns>
        bool IdEmpty { get; }

        /// <summary>
        /// 检查指定ID是否正确
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckId(T id);
        /// <summary>
        /// 设置对象ID，如果传入的ID无效，返回false
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        bool SetId(string strId);
        /// <summary>
        /// 转化id
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        T ConvertID(string strId);
    }
}
