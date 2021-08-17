using System.Threading.Tasks;

namespace TianCheng.Common
{
    /// <summary>
    /// 通用的返回信息
    /// </summary>
    public class ResultView
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 需要提示的警告信息（有些时候，操作成功，也需要提示一个警告）
        /// </summary>
        public string Warning { get; set; }

        #region 操作成功
        /// <summary>
        /// 操作成功
        /// </summary>
        static public ResultView Success(string msg = "操作成功")
        {
            return new ResultView() { Code = 0, Message = msg };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        static public Task<ResultView> SuccessTask(string msg = "操作成功")
        {
            return Task.Run(() =>
            {
                return new ResultView() { Code = 0, Message = msg };
            });
        }
        #endregion
    }
}
