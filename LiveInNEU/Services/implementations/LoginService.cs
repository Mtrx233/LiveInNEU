using System.Threading.Tasks;

namespace LiveInNEU.Services.implementations {
    /// <summary>
    /// 登陆服务实现
    /// </summary>
    /// <author>殷昭伉</author>
    public class LoginService : ILoginService {
        /******** 构造函数 ********/
        public LoginService(ILessonStorage lessonStorage, ILoginStorage loginStorage, INeuDataService neuDataService)
        {
            this._lessonStorage = lessonStorage;
            this._loginStorage = loginStorage;
            this._neuDataService = neuDataService;
        }

        /******** 共有变量 ********/

        /******** 私有变量 ********/
        /// <summary>
        /// 课程信息存储
        /// </summary>
        private ILessonStorage _lessonStorage;

        /// <summary>
        /// 登录信息存储
        /// </summary>
        private ILoginStorage _loginStorage;

        /// <summary>
        /// 网络数据获取实现
        /// </summary>
        private INeuDataService _neuDataService;

        /******** 继承方法 ********/
        /// <summary>
        /// 初始化
        /// </summary>
        public async Task BeforeLogin()
        {
            if (!_lessonStorage.Initialized())
            {
                await _lessonStorage.InitializeAsync();
            }
        }

        /// <summary>
        /// 判断是否已经登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsLogin()
        {
            return await _loginStorage.IsLogin();
        }

        /// <summary>
        /// 判断账户密码是否正确
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>返回是否登录成功</returns>
        public Task<bool> Login(int kind, string username, string password)
        {
            return _neuDataService.IsRight(kind, username, password);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public async Task GetDataAsync()
        {
            await _neuDataService.DataStorageAsync();
        }

        /******** 扩展方法 ********/

    }
}