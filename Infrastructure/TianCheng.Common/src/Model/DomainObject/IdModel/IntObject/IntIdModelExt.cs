using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// int 类型Id扩展方法
    /// </summary>
    static public class IntIdModelExt
    {
        /// <summary>
        /// 将逗号分隔的id字符串转成int类型的数组
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IEnumerable<int> ToIntIdList(this string ids)
        {
            foreach (string id in ids.Split(","))
            {
                if (int.TryParse(id, out int i))
                {
                    yield return i;
                }
            }
        }
    }
}
