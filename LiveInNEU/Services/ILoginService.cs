using System.Threading.Tasks;


namespace LiveInNEU.Services {
    /// <summary>
    /// 登陆服务接口
    /// </summary>
    public interface ILoginService {
        /// <summary>
        /// 初始化
        /// </summary>
        Task BeforeLogin();

        /// <summary>
        /// 判断是否已经登录
        /// </summary>
        /// <returns></returns>
        Task<bool> IsLogin();


        /// <summary>
        /// 判断账户密码是否正确
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>返回是否登录成功</returns>
        Task<bool> Login(int kind, string username, string password);

        /// <summary>
        /// 获取数据
        /// </summary>
        Task GetDataAsync();
    }
}