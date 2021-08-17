using TianCheng.Common;
using TianCheng.Inventory.DAL;
using TianCheng.Inventory.DTO;
using TianCheng.Inventory.Model;
using TianCheng.Service.Core;

namespace TianCheng.Inventory.Services
{
    /// <summary>
    /// 商品信息    [ Service ]
    /// </summary>
    public class GoodsService : MongoBusinessService<GoodsDAL, GoodsInfo, GoodsQuery>,
        IDeleteMongoService<GoodsInfo, GoodsDAL>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public GoodsService(GoodsDAL dal) : base(dal)
        {

        }

        #endregion

        #region 新增 / 修改方法
        /// <summary>
        /// 保存的校验
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void SavingCheck(GoodsInfo info, TokenBase logonInfo)
        {
            //数据验证
            if (string.IsNullOrWhiteSpace(info.Name))
            {
                throw ApiException.BadRequest("请指定商品的名称");
            }
            if (Dal.HasRepeat(info.Id, "Name", info.Name))
            {
                throw ApiException.BadRequest("商品名称已存在，不能重复保存");
            }
        }
        #endregion
    }
}
