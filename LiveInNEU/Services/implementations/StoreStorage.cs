using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using SQLite;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 收藏存储实现
    /// </summary>
    /// <author></author>
    public class StoreStorage : IStoreStorage
    {
        /******** 构造函数 ********/
        public StoreStorage(IInfoStorage infoStorage, IPreferenceStorage preferenceStorage)
        {
            this._preferenceStorage = preferenceStorage;
            this._infoStorage = infoStorage;
        }

        /******** 公开变量 ********/
        /// <summary>
        /// 数据库路径。
        /// </summary>
        public static readonly string StoreDbPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData), DbName);


        /******** 私有变量 ********/

        /// <summary>
        /// 数据库名。
        /// </summary>
        private const string DbName = "neudb.sqlite3";

        /// <summary>
        /// 偏好存储
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        /// <summary>
        /// 键值对存储
        /// </summary>
        private IInfoStorage _infoStorage;

        /// <summary>
        /// 数据库连接影子变量。
        /// </summary> 
        private SQLiteAsyncConnection _connection;

        /// <summary>
        /// 数据库连接。
        /// </summary>
        private SQLiteAsyncConnection Connection =>
            _connection ??
            (_connection = new SQLiteAsyncConnection(StoreDbPath));


        /******** 继承方法 ********/
        /// <summary>
        /// 数据库是否初始化
        /// </summary>
        public bool Initialized() =>
            _preferenceStorage.Get(LessonStorageConstants.VERSION_KEY, -1) ==
            LessonStorageConstants.VERSION;

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public async Task InitializeAsync()
        {
            using (var dbFileStream =
                new FileStream(StoreDbPath, FileMode.OpenOrCreate))
            {
                using (var dbAssetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DbName))
                {
                    await dbAssetStream.CopyToAsync(dbFileStream);
                }
            }

            _preferenceStorage.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION);
            await _infoStorage.Set(LessonStorageConstants.ACCOUNT_KEY, LessonStorageConstants.ACCOUNT);
            await _infoStorage.Set(LessonStorageConstants.PASSWORD_KEY, LessonStorageConstants.PASSWORD);
            await _infoStorage.Set(LessonStorageConstants.UPDATEDATE_KEY, DateTime.Now.ToString());
        }

        public async Task<Store> GetStoreAsync(Expression<Func<Store, bool>> @where) =>
            await Connection.Table<Store>().Where(@where).FirstOrDefaultAsync();


        public async Task<IList<Store>> GetStoresAsync(Expression<Func<Store, bool>> @where) =>
            await Connection.Table<Store>().Where(@where).ToListAsync();

        public async Task UpdateStoreAsync(Store store)
        {
            await Connection.Table<Store>().Where(p => p.QuestionId == store.QuestionId).DeleteAsync();
            await Connection.InsertAsync(store);
        }


        /******** 扩展方法 ********/
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public async Task CloseAsync() => await Connection.CloseAsync();

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public async Task AddStoreAsync(Store store) =>
            await Connection.InsertAsync(store);

        /// <summary>
        /// 获得用户名
        /// </summary>
        /// <returns></returns>
        public Task<string> GetUserName()
        {
            return _infoStorage.Get(LessonStorageConstants.ACCOUNT_KEY,
                "username");
        }

        public async Task UpStoresAsync(IList<Store> stores)
        {
            await Connection.Table<Store>().Where(p => p.QuestionId != -1).DeleteAsync();
            foreach (var store in stores)
            {
                await AddStoreAsync(store);
            }
        }
    }
}