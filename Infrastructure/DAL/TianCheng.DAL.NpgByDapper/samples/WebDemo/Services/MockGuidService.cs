using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDemo.DAL;
using WebDemo.Model;

namespace WebDemo.Services
{
    public class MockGuidService
    {
        MockGuidDAL Dal;
        public MockGuidService(MockGuidDAL dal)
        {
            Dal = dal;
        }

        public MockGuidInfo First()
        {
            return Dal.First(new MockGuidQuery() { LikeName = "c" });
        }

        public void Create()
        {
            Dal.InsertObject(new MockGuidInfo() { Email = "email", FirstName = "first", LastName = "last" });
        }

        public int Count(MockGuidQuery query) => (Dal.Count(query));

        //public int Count(TestQuery query) => (Dal.Count(query));
    }
}
