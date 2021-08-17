using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 将对象转成指定的类型
    /// </summary>
    static public class ObjectTran
    {
        /// <summary>
        /// 将对象转成指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public T Tran<T>(this object obj)
        {
            return AutoMapperExtension.Mapper.Map<T>(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        static public T Map<T>(object src)
        {
            return AutoMapperExtension.Mapper.Map<T>(src);
        }
    }
}
