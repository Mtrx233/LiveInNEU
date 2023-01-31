using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using LiveInNEU.Models;
using SQLite;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 学期数据库
    /// </summary>
    /// <author>赵全</author>
    public class SemesterStorage:ISemesterStorage
    {
        /******** 构造函数 ********/
        public SemesterStorage(IInfoStorage infoStorage,IPreferenceStorage preferenceStorage)
        {
            this._infoStorage = infoStorage;
            this._preferenceStorage = preferenceStorage;
        }

        /******** 公有变量 ********/
        /// <summary>
        /// 诗词数据库路径。
        /// </summary>
        public static readonly string SemesterDbPath =
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
            (_connection = new SQLiteAsyncConnection(SemesterDbPath));

        /******** 继承方法 ********/

        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool Initialized() =>
            _preferenceStorage.Get(LessonStorageConstants.VERSION_KEY, -1) ==
            LessonStorageConstants.VERSION;

        /// <summary>
        /// 数据库初始化
        /// </summary>
        public async Task InitializeAsync()
        {
            using (var dbFileStream =
                new FileStream(SemesterDbPath, FileMode.OpenOrCreate))
            {
                using (var dbAssetStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(DbName))
                {
                    await dbAssetStream.CopyToAsync(dbFileStream);
                }
                _preferenceStorage.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION);
                await _infoStorage.Set(LessonStorageConstants.ACCOUNT_KEY, LessonStorageConstants.ACCOUNT);
                await _infoStorage.Set(LessonStorageConstants.PASSWORD_KEY, LessonStorageConstants.PASSWORD);
            }
        }

        /// <summary>
        /// 获取一个学期
        /// </summary>
        /// <param name="id">学期id。</param>
        public async Task<Semester> GetSemesterAsync(int id) =>
            await Connection.Table<Semester>().Where(p => p.Id == id)
                .FirstOrDefaultAsync();

        /// <summary>
        ///  添加一个学期
        /// </summary>
        public async Task AddSemesterAsync(Semester semester) =>
            await Connection.InsertAsync(semester);

        /// <summary>
        /// 获取满足给定条件的学期集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        public async Task<IList<Semester>> GetSemestersAsync(
            Expression<Func<Semester, bool>> @where) =>
            await Connection.Table<Semester>().Where(@where)
                .ToListAsync();

        /// <summary>
        /// 得到当前学期开始时间
        /// </summary>
        /// <returns>开始时间</returns>
        public async Task<DateTime> GetInitBeginDate()
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy.MM.dd";
            return Convert.ToDateTime(await _infoStorage.Get(LessonStorageConstants.SEMESTER_START_KEY,
                "time"), dtFormat);
        }

        /// <summary>
        /// 得到当前学期
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetInitSemester() =>
            await _infoStorage.Get(LessonStorageConstants.SEMESTER_KEY, -1);

        /// <summary>
        /// 自动选择学期
        /// </summary>
        public async Task<int> HelpSelected()
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy.MM.dd";
            var now = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            int year = now.Year;
            Semester semester = await GetSemesterAsync(year * 10 + 1);
            if (now >= Convert.ToDateTime(semester.StartDate,dtFormat))
            {
                await _infoStorage.Set(LessonStorageConstants.SEMESTER_KEY, year * 10 + 1);
            }
            else
            {
                await _infoStorage.Set(LessonStorageConstants.SEMESTER_KEY, (year - 1) * 10 + 2);
                semester = await GetSemesterAsync((year - 1) * 10 + 2);
            }
            await _infoStorage.Set(LessonStorageConstants.SEMESTER_START_KEY, semester.StartDate);
            return year * 10 + 1;
        }

        /******** 扩展方法 ********/

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public async Task CloseAsync() => await Connection.CloseAsync();
    }
}

