using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// SelectView 对象比较
    /// </summary>
    public class SelectViewCompare : IEqualityComparer<SelectView>
    {
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(SelectView x, SelectView y)
        {
            return (x.Id == y.Id) && (x.Name == y.Name) && (x.Code == y.Code);
        }
        /// <summary>
        /// HashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(SelectView obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
