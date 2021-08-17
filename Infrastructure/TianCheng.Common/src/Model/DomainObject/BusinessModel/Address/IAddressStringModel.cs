using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 地址信息接口
    /// </summary>
    public interface IAddressStringModel
    {
        #region 地址信息
        /// <summary>
        /// 省份Id
        /// </summary>
        /// <remarks></remarks>
        public string ProvinceId { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>
        /// <remarks></remarks>
        public string CityId { get; set; }
        /// <summary>
        /// 区Id
        /// </summary>
        public string AreaId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        /// <remarks></remarks>
        public string Province { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        /// <remarks></remarks>
        public string City { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        #endregion
    }
}
