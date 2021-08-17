using TianCheng.Common;
using TianCheng.Inventory.DAL;
using TianCheng.Inventory.DTO;
using TianCheng.Inventory.Model;
using TianCheng.Service.Core;

namespace TianCheng.Inventory.Services
{
    /// <summary>
    /// 商品库存信息    [ Service ]
    /// </summary>
    public class InventoryGoodsService : MongoBusinessService<InventoryGoodsDAL, InventoryGoodsInfo, InventoryQuery>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public InventoryGoodsService(InventoryGoodsDAL dal) : base(dal)
        {

        }

        #endregion
    }
}
