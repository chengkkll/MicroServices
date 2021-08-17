using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.Inventory.DAL;
using TianCheng.Inventory.Model;
using TianCheng.Service.Core;

namespace TianCheng.Inventory.Services
{
    /// <summary>
    /// 品牌信息    [ Service ]
    /// </summary>
    public class BrandService : MongoBusinessService<BrandDAL, BrandInfo, QueryObject>,
        IDeleteMongoService<BrandInfo, BrandDAL>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public BrandService(BrandDAL dal) : base(dal)
        {

        }

        #endregion

        #region 新增 / 修改方法
        /// <summary>
        /// 保存的校验
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void SavingCheck(BrandInfo info, TokenBase logonInfo)
        {
            //数据验证
            if (string.IsNullOrWhiteSpace(info.Name))
            {
                throw ApiException.BadRequest("请指定品牌的名称");
            }
            if (Dal.HasRepeat(info.Id, "Name", info.Name))
            {
                throw ApiException.BadRequest("品牌名称已存在，不能重复保存");
            }
        }
        #endregion
    }
}
