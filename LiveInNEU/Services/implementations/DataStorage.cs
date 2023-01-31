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
    /// 数据存储实现
    /// </summary>
    /// <author>殷昭伉</author>
    public class DataStorage : IDataStorage
    {
        /******** 构造函数 ********/
        public DataStorage(IInfoStorage infoStorage, IPreferenceStorage preferenceStorage)
        {
            this._infoStorage = infoStorage;
            this._preferenceStorage = preferenceStorage;
        }

        /******** 公开变量 ********/
        /// <summary>
        /// 诗词数据库路径。
        /// </summary>
        public static readonly string DataDbPath =
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
            (_connection = new SQLiteAsyncConnection(DataDbPath));


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
                new FileStream(DataDbPath, FileMode.OpenOrCreate))
            {
                using (var dbAssetStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(DbName))
                {
                    await dbAssetStream.CopyToAsync(dbFileStream);
                }

                _preferenceStorage.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION);
            }
        }

        /// <summary>
        /// 获取一个数据
        /// </summary>
        /// <param name="id">课程id。</param>
        public async Task<Data> GetDataAsync(string id) =>
            await Connection.Table<Data>().Where(p => p.ID == id)
                .FirstOrDefaultAsync();

        /// <summary>
        /// 添加一个数据
        /// </summary>
        public async Task AddDataAsync(Data data) =>
            await Connection.InsertAsync(data);

        /// <summary>
        /// 获取满足给定条件的数据集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        public async Task<IList<Data>> GetDatasAsync(Expression<Func<Data, bool>> @where) =>
            await Connection.Table<Data>().Where(@where)
                .ToListAsync();

        /// <summary>
        /// 删除数据
        /// </summary>
        public async Task DeleteDatasAsync() =>
            await Connection.DeleteAllAsync<Data>();

        /// <summary>
        /// 设置数据更新时间
        /// </summary>
        public async Task<string> GetUpdateTime() =>
            await _infoStorage.Get(LessonStorageConstants.UPDATEDATE_KEY,
                "time");

        /// <summary>
        /// 获取数据更新时间
        /// </summary>
        public async Task SetUpdateTime(string time) =>
            await _infoStorage.Set(LessonStorageConstants.UPDATEDATE_KEY,
                time);

        /******** 扩展方法 ********/
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public async Task CloseAsync() => await Connection.CloseAsync();
    }
    }
