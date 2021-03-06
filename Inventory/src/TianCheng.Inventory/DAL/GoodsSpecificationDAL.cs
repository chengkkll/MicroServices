using System.Linq;
using TianCheng.Common;
using TianCheng.DAL.MongoDB;
using TianCheng.Inventory.DTO;
using TianCheng.Inventory.Model;

namespace TianCheng.Inventory.DAL
{
    /// <summary>
    /// 商品规格 [数据持久化]
    /// </summary>
    [DBMapping("stock_goods_speci")]
    public class GoodsSpecificationDAL : MongoOperation<GoodsSpecificationInfo, GoodsQuery>
    {
        #region 查询方法
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected override IQueryable<GoodsSpecificationInfo> BaseFilter(GoodsQuery filter)
        {
            var query = Queryable();

            #region 查询条件
            //不显示删除的数据
            query = query.Where(e => e.IsDelete == false);

            #endregion

            #region 设置排序规则
            //设置排序方式
            switch (filter.Sort.Property)
            {
                case "name": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name); break; }
                default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }
        #endregion
    }
}
