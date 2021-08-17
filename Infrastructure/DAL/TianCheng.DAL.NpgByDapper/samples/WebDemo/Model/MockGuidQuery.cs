using TianCheng.Common;

namespace WebDemo.Model
{

    public class MockGuidQuery : QueryObject
    {
        public string FirstName { get; set; }

        public string LikeName { get; set; }

        public bool? IsDelete { get; set; } = null;
    }


    public class TestQuery : QueryObject
    {
        public string FirstName { get; set; }
    }
}
