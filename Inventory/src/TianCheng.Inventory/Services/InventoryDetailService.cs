using TianCheng.Common;
using TianCheng.Inventory.DAL;
using TianCheng.Inventory.DTO;
using TianCheng.Inventory.Model;
using TianCheng.Service.Core;

namespace TianCheng.Inventory.Services
{
    /// <summary>
    /// 库存明细信息    [ Service ]
    /// </summary>
    public class InventoryDetailService : MongoBusinessService<InventoryDetailDAL, InventoryDetailInfo, InventoryQuery>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public InventoryDetailService(InventoryDetailDAL dal) : base(dal)
        {

        }

        #endregion
    }
}
