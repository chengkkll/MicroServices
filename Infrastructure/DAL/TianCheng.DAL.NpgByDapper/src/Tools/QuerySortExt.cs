using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    public static class QuerySortExt
    {
        static public string OrderSQL(this QuerySort sort)
        {
            return string.IsNullOrWhiteSpace(sort.Property) ? "" : string.Format(" order by {0} {1}", sort.Property, sort.IsAsc ? "ASC" : "DESC");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        static public string LimitSQL(this QueryPagination page)
        {
            return (page != null && page.Size > 0) ? $" limit {page.Size}" : "";
        }

        static public string OffsetSQL(this QueryPagination page)
        {
            return (page != null && page.Index > -1 && page.Size > 0) ? $" offset {page.Index * page.Size}" : "";
        }
    }
}
