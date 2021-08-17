using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DAL;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 读取default库的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("default")]
        public IEnumerable<DemoView> GetDefault()
        {
            var data = new DemoDAL().Queryable();
            return TianCheng.Common.ObjectTran.Map<IEnumerable<DemoView>>(data);
        }

        /// <summary>
        /// 读取debug库的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("debug")]
        public IEnumerable<DemoView> GetDebug()
        {
            var data = new DemoDebugDAL().Queryable();
            return TianCheng.Common.ObjectTran.Map<IEnumerable<DemoView>>(data);
        }

        /// <summary>
        /// 写default库的数据
        /// </summary>
        [HttpGet("default/append")]
        public string InsertDefault()
        {
            var entity = new DemoInfo() { Name = Guid.NewGuid().ToString().Substring(0, 7), Code = "default", Date = DateTime.Now };
            new DemoDAL().InsertObject(entity);
            return entity.IdString;
        }

        /// <summary>
        /// 写debug库的数据
        /// </summary>
        [HttpGet("debug/append")]
        public void InsertDebug()
        {
            new DemoDebugDAL().InsertObject(new DemoInfo() { Name = Guid.NewGuid().ToString(), Code = "debug", Date = DateTime.Now });
        }

        /// <summary>
        /// 写debug库的数据
        /// </summary>
        [HttpGet("delete/{id}")]
        public void Delete(string id)
        {
            new DemoDAL().DeleteById(id);
        }

        /// <summary>
        /// 写debug库的数据
        /// </summary>
        [HttpGet("remove/{id}")]
        public void Remove(string id)
        {
            new DemoDAL().RemoveById(id);
        }
    }
}
