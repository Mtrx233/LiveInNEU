using System.Threading.Tasks;

namespace LiveInNEU.Services {
    /// <summary>
    /// 网络数据获取接口
    /// </summary>
    public interface INeuDataService {

        /// <summary>
        /// 判断账户名和密码是否正确
        /// </summary>
        /// <param name="kind">网络途径</param>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        Task<bool> IsRight(int kind, string username, string password);

        /// <summary>
        /// 网络数据处理并存储
        /// </summary>
        Task DataStorageAsync();

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <returns>消息队列</returns>
        Task<string> Update();
        Task<string> Update(int a);
    }
}