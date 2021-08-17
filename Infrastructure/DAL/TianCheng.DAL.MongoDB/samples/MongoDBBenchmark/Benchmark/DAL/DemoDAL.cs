using TianCheng.DAL.MongoDB;

namespace MongoDBBenchmark
{
    /// <summary>
    /// 按默认数据库连接操作表test_demo
    /// </summary>
    [DBMapping("test_demo")]
    public class DemoDAL : MongoOperation<DemoInfo>
    {

    }

    /// <summary>
    /// 按默认数据库连接操作表test_demo
    /// </summary>
    [DBMapping("test_demo")]
    public class DemoDAL2 : MongoO<DemoInfo>
    {

    }
}
