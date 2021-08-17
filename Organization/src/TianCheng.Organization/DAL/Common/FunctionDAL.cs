using TianCheng.DAL.MongoDB;
using TianCheng.Organization.Model;

namespace TianCheng.Organization.DAL
{
    /// <summary>
    /// 功能点信息 [数据持久化]
    /// </summary>
    [DBMapping("system_function")]
    public class FunctionDAL : MongoOperation<FunctionModuleInfo, FunctionQuery>
    {
    }
}
