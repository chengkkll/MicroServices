using TianCheng.Common;
using TianCheng.Inventory.DAL;
using TianCheng.Inventory.DTO;
using TianCheng.Inventory.Model;
using TianCheng.Service.Core;

namespace TianCheng.Inventory.Services
{
    /// <summary>
    /// 商品规格的库存信息    [ Service ]
    /// </summary>
    public class InventorySpecificationService : MongoBusinessService<InventorySpecificationDAL, InventorySpecificationInfo, InventoryQuery>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public InventorySpecificationService(InventorySpecificationDAL dal) : base(dal)
        {

        }

        #endregion
    }
}
