using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using SQLite;
using Xamarin.Essentials;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 数据库版的偏好存储
    /// </summary>
    /// <author>钱子昂</author>
    public class InfoStorage :IInfoStorage 
    {
        /******** 构造函数 ********/
        public InfoStorage(IPreferenceStorage preferenceStorage)
        {
            this._preferenceStorage = preferenceStorage;
        }

        /******** 公开变量 ********/
        /// <summary>
        /// 诗词数据库路径。
        /// </summary>
        public static readonly string InfoDbPath =
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
        /// 数据库连接影子变量。
        /// </summary>
        private SQLiteAsyncConnection _connection;

        /// <summary>
        /// 数据库连接。
        /// </summary>
        private SQLiteAsyncConnection Connection =>
            _connection ??
            (_connection = new SQLiteAsyncConnection(InfoDbPath));

        /******** 继承方法 ********/

        /// <summary>
        /// 数据库是否初始化
        /// </summary>
        public bool Initialized() =>
            _preferenceStorage.Get(LessonStorageConstants.VERSION_KEY, -1) ==
            LessonStorageConstants.VERSION;

        /// <summary>
        /// 初始化
        /// </summary>
        public async Task InitializeAsync()
        {
            using (var dbFileStream =
                new FileStream(InfoDbPath, FileMode.OpenOrCreate))
            {
                using (var dbAssetStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream( DbName))
                {
                    await dbAssetStream.CopyToAsync(dbFileStream);
                }

                _preferenceStorage.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION);
            }
        }

        /// <param name="key">Preference key.</param>
        /// <param name="value">Preference value.</param>
        /// <summary>Sets a value for a given key.</summary>
        /// <remarks>
        ///     <para />
        /// </remarks>
        public async Task Set(string key, int value)
        {
            Info info = await Connection.Table<Info>().Where(p => p.KEY == key)
                .FirstOrDefaultAsync();
            if (info == null)
            {
                info = new Info { KEY = key, Value = value.ToString() };
                await AddInfoAsync(info);
            }
            else
            {
                info.Value = value.ToString();
                await UpdateInfoAsync(info);
            }
        }

        /// <param name="key">Preference key.</param>
        /// <param name="defaultValue">Default value to return if the key does not exist.</param>
        /// <summary>Gets the value for a given key, or the default specified if the key does not exist.</summary>
        /// <returns>Value for the given key, or the default if it does not exist.</returns>
        /// <remarks />
        public async Task<int> Get(string key,int defaults) {
            Info info = await Connection.Table<Info>().Where(p => p.KEY == key).FirstOrDefaultAsync();
            return Convert.ToInt32(info.Value);
        }

        /// <param name="key">Preference key.</param>
        /// <param name="value">Preference value.</param>
        /// <summary>Sets a value for a given key.</summary>
        /// <remarks>
        ///     <para />
        /// </remarks>
        public async Task Set(string key, string value)
        {
            Info info = await Connection.Table<Info>().Where(p => p.KEY == key)
                .FirstOrDefaultAsync();
            if (info == null)
            {
                info = new Info
                {
                    KEY = key,
                    Value = value
                };
                await AddInfoAsync(info);
            }
            else
            {
                info.Value = value;
                await UpdateInfoAsync(info);
            }
        }

        /// <param name="key">Preference key.</param>
        /// <param name="defaultValue">Default value to return if the key does not exist.</param>
        /// <summary>Gets the value for a given key, or the default specified if the key does not exist.</summary>
        /// <returns>Value for the given key, or the default if it does not exist.</returns>
        /// <remarks />
        public async Task<string> Get(string key, string defaults)
        {
            Info info = await Connection.Table<Info>().Where(p => p.KEY == key).FirstOrDefaultAsync();
           return info.Value;
        }


        /******** 扩展方法 ********/
        /// <summary>
        /// 向数据库写入数据
        /// </summary>
        public async Task AddInfoAsync(Info info) =>
            await Connection.InsertAsync(info);

        /// <summary>
        /// 向数据库更新数据
        /// </summary>
        public async Task UpdateInfoAsync(Info info) =>
            await Connection.UpdateAsync(info);

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public async Task CloseAsync() => await Connection.CloseAsync();

    }
}

