using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.DAL
{
    /// <summary>
    /// Id为string类型的数据库操作接口
    /// </summary>
    /// <typeparam name="DO"></typeparam>
    public interface IStringIdDBOperation<DO> : IDBOperation<DO, string>
    {

    }
}
