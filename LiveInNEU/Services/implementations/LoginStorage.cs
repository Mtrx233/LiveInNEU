using System.Threading.Tasks;


namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 登录信息存储实现
    /// </summary>
    /// <author>钱子昂</author>
    public class LoginStorage : ILoginStorage
    {
        /******** 构造函数 ********/
        public LoginStorage(IInfoStorage infoStorage)
        {
            this._infoStorage = infoStorage;
        }

        /******** 共有变量 ********/

        /******** 私有变量 ********/
        /// <summary>
        /// 键值对存储
        /// </summary>
        private IInfoStorage _infoStorage;

        /******** 继承方法 ********/
        /// <summary>
        /// 判断是否已经登录
        /// </summary>
        public async Task<bool> IsLogin() =>
            await _infoStorage.Get(LessonStorageConstants.ACCOUNT_KEY, "string") != LessonStorageConstants.ACCOUNT;

        /// <summary>
        /// 存储用户名和密码
        /// </summary>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        public async Task StorageOfInfo(int kind, string username, string password)
        {
            await _infoStorage.Set(LessonStorageConstants.KIND_KEY, kind);
            await _infoStorage.Set(LessonStorageConstants.ACCOUNT_KEY, username);
            await _infoStorage.Set(LessonStorageConstants.PASSWORD_KEY, password);
        }

        /******** 扩展方法 ********/

    }
}