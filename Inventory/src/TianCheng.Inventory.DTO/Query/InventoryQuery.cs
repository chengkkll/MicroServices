﻿using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;

namespace TianCheng.Inventory.DTO
{
    /// <summary>
    /// 库存查询条件
    /// </summary>
    public class InventoryQuery : QueryObject
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
