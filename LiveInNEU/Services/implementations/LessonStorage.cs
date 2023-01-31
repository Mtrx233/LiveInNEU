using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using LiveInNEU.Models;
using SQLite;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 课程存储实现
    /// </summary>
    /// <author>殷昭伉</author>
    public class LessonStorage : ILessonStorage
    {
        /******** 构造函数 ********/
        public LessonStorage(IInfoStorage infoStorage,IPreferenceStorage preferenceStorage)
        {
            this._preferenceStorage = preferenceStorage;
            this._infoStorage = infoStorage;
        }

        /******** 公开变量 ********/
        /// <summary>
        /// 诗词数据库路径。
        /// </summary>
        public static readonly string LessonDbPath =
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
            (_connection = new SQLiteAsyncConnection(LessonDbPath));


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
                new FileStream(LessonDbPath, FileMode.OpenOrCreate))
            {

                //TODO 注意，如果是苹果电脑，需要将下边的using修改为-->using (var dbAssetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DbName))
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

        /// <summary>
        /// 获取一个课程
        /// </summary>
        /// <param name="id">课程id。</param>
        public async Task<Lesson> GetLessonAsync(string id) =>
            await Connection.Table<Lesson>().Where(p => p.Id == id)
                .FirstOrDefaultAsync();

        /// <summary>
        /// 添加一个课程
        /// </summary>
        public async Task AddLessonAsync(Lesson lesson) =>
            await Connection.InsertAsync(lesson);

        /// <summary>
        /// 获取满足给定条件的课程集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        public async Task<IList<Lesson>> GetLessonsAsync(
            Expression<Func<Lesson, bool>> @where) =>
            await Connection.Table<Lesson>().Where(@where)
                .ToListAsync();

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="where">Where条件。</param>
        public async Task DeleteLessonAsync(
            Expression<Func<Lesson, bool>> @where) =>
            await Connection.Table<Lesson>().Where(@where)
                .DeleteAsync();

        /// <summary>
        /// 获取账户
        /// </summary>
        public async Task<string> GetAccount() =>
            await _infoStorage.Get(LessonStorageConstants.ACCOUNT_KEY,
                "account");

        /// <summary>
        /// 获取密码
        /// </summary>
        public async Task<string> GetPassword() =>
            await _infoStorage.Get(LessonStorageConstants.PASSWORD_KEY,
            "password");

        /******** 扩展方法 ********/
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public async Task CloseAsync() => await Connection.CloseAsync();

    }
}

