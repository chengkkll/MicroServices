using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// NameViewModel 对象比较
    /// </summary>
    public class NameViewModelCompare : IEqualityComparer<NameViewModel>
    {
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(NameViewModel x, NameViewModel y)
        {
            return (x.Id == y.Id) && (x.Name == y.Name);
        }
        /// <summary>
        /// HashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(NameViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
