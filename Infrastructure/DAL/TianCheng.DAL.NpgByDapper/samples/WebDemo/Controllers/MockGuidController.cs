using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.Common;
using TianCheng.DAL.NpgByDapper;
using WebDemo.Services;

namespace WebDemo.Controllers
{
    [ApiController]
    [Route("MockGuid")]
    public class MockGuidController : ControllerBase
    {
        private readonly MockGuidService Service;

        public MockGuidController(MockGuidService service)
        {
            Service = service;

        }

        [HttpGet("First")]
        public string First()
        {
            return Service.First().ToJson();
        }

        [HttpGet("count1")]
        public int Count()
        {
            return Service.Count(new Model.MockGuidQuery() { LikeName = "c" });
        }

        [HttpGet("count2")]
        public int Count2()
        {
            return Service.Count(new Model.MockGuidQuery() { FirstName = "Marybelle" });
        }

        [HttpPost()]
        public void Create()
        {
            Service.Create();
        }
    }
}
