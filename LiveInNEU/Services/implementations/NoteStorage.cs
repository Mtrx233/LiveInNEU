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
    /// 备忘录存储实现
    /// </summary>
    /// <author></author>
    public class NoteStorage : INoteStorage
    {
        /******** 构造函数 ********/
        public NoteStorage(IInfoStorage infoStorage, IPreferenceStorage preferenceStorage)
        {
            this._preferenceStorage = preferenceStorage;
            this._infoStorage = infoStorage;
        }

        /******** 公开变量 ********/
        /// <summary>
        /// 数据库路径。
        /// </summary>
        public static readonly string NoteDbPath =
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
            (_connection = new SQLiteAsyncConnection(NoteDbPath));


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
                new FileStream(NoteDbPath, FileMode.OpenOrCreate))
            {

                //TODO 注意，如果是苹果电脑，需要将下边的using修改为-->using (var dbAssetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DbName))
                using (var dbAssetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DbName))
                {
                    await dbAssetStream.CopyToAsync(dbFileStream);
                }
            }

            //_preferenceStorage.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION);
            await _infoStorage.Set(LessonStorageConstants.ACCOUNT_KEY, LessonStorageConstants.ACCOUNT);
            await _infoStorage.Set(LessonStorageConstants.PASSWORD_KEY, LessonStorageConstants.PASSWORD);
            await _infoStorage.Set(LessonStorageConstants.UPDATEDATE_KEY, DateTime.Now.ToString());
        }

        public async Task<Note> GetNoteAsync(Expression<Func<Note, bool>> @where) =>
            await Connection.Table<Note>().Where(@where).FirstOrDefaultAsync();


        public async Task<IList<Note>> GetNotesAsync(Expression<Func<Note, bool>> @where) =>
            await Connection.Table<Note>().Where(@where).ToListAsync();


        public async Task UpdateNoteAsync(Note note)
        {
            await Connection.Table<Note>().Where(p => p.Id == note.Id).DeleteAsync();
            await Connection.InsertAsync(note);
        }


        /******** 扩展方法 ********/
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public async Task CloseAsync() => await Connection.CloseAsync();

        public async Task AddNoteAsync(Note note) =>
            await Connection.InsertAsync(note);

    }
}
