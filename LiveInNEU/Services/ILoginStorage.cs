using System.Threading.Tasks;

namespace LiveInNEU.Services {
    /// <summary>
    /// 登录信息存储接口
    /// </summary>
    public interface ILoginStorage {
        /// <summary>
        /// 判断是否已经登录
        /// </summary>
        Task<bool> IsLogin();


        /// <summary>
        /// 存储用户名和密码
        /// </summary>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        Task StorageOfInfo(int kind, string username,string password);
    }


}