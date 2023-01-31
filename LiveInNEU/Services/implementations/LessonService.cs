using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Scheduler.Internal;
using LiveInNEU.Models;


namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 课程数据处理实现
    /// </summary>
    /// <author>赵全</author>
    public class LessonService : ILessonService
    {
        /******** 构造函数 ********/
        public LessonService(ILessonStorage lessonStorage, ISemesterStorage semesterStorage)
        {
            _lessonStorage = lessonStorage;
            _semesterStorage = semesterStorage;
        }

        /******** 公有变量 ********/

        /******** 私有变量 ********/
        /// <summary>
        /// 课程信息存储
        /// </summary>
        private ILessonStorage _lessonStorage;

        /// <summary>
        /// 学期信息存储
        /// </summary>
        private ISemesterStorage _semesterStorage;

        /// <summary>
        /// 记录学期开始时间
        /// </summary>
        private DateTime beginDate;

        /// <summary>
        /// 记录当前学期
        /// </summary>
        private int Semester;

        /******** 继承方法 ********/
        /// <summary>
        /// 从数据库中获取全部课程
        /// </summary>
        public async Task<IList<LessonShowData>> GetLesson()
        {
            beginDate = await _semesterStorage.GetInitBeginDate();
            Semester = await _semesterStorage.GetInitSemester();
            IList<LessonShowData> lessonShowDatas = new List<LessonShowData>();

            var lessons = await _lessonStorage.GetLessonsAsync(p => p.Year == Semester);

            Dictionary<string, int> colour = new Dictionary<string, int>();

            foreach (var lesson in lessons)
            {
                if (!colour.ContainsKey(lesson.Id))
                {
                    colour.Add(lesson.Id, colour.Count);
                }

                lessonShowDatas.Add(ChangeData(lesson, colour[lesson.Id]));
            }

            return lessonShowDatas;
        }

        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="account">编号</param>
        /// <param name="lessonName">课程名</param>
        /// <param name="teacherName">老师名</param>
        /// <param name="location">上课地点</param>
        /// <param name="lessonDate">上课日期</param>
        /// <param name="startTime">上课节数</param>
        /// <param name="continueTime">持续节数</param>
        /// <returns>返回能否添加</returns>
        public async Task<bool> AddLesson(string account, string lessonName, string teacherName,
            string location, DateTime lessonDate, string startTime, int continueTime)
        {
            int start = startTime.Length == 4
                ? int.Parse(startTime.Substring(1, 1))
                : int.Parse(startTime.Substring(1, 2));
            TimeSpan d = lessonDate - await _semesterStorage.GetInitBeginDate();
            Lesson lesson = new Lesson()
            {
                Classroom = location,
                ContinueTime = continueTime,
                Id = account,
                Name = lessonName,
                StartTime = start,
                TeacherName = teacherName,
                Week = d.Days / 7 + 1,
                Weekday = d.Days % 7,
                Year = await _semesterStorage.GetInitSemester(),
                Kind = "1"
            };
            if (!await IsRepeat(account, lesson.StartTime, lesson.ContinueTime,
                lesson.Week, lesson.Weekday))
            {
                return false;
            }

            var list = await _lessonStorage.GetLessonsAsync(p => p.Year == lesson.Year);
            foreach (var ilesson in list)
            {
                if (ilesson.Id == account && ilesson.Name != lessonName)
                {
                    return false;
                }
            }

            await _lessonStorage.AddLessonAsync(lesson);
            return true;
        }

        public int ComparesTo(LessonShowData x, LessonShowData y)
        {
            if (x.StartTime < y.StartTime)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="account">编号</param>
        /// <param name="lessonDate">具体的上课时间</param>
        public async Task DelLesson(string account, DateTime lessonDate)
        {
            var d = lessonDate - await _semesterStorage.GetInitBeginDate();
            var week = d.Days / 7 + 1;
            var weekday = d.Days % 7;
            var strattime = lessonDate.Hour - 7 > 5
                ? lessonDate.Hour - 9
                : lessonDate.Hour - 7;
            await _lessonStorage.DeleteLessonAsync(p =>
                p.Id == account && p.StartTime == strattime && p.Weekday == weekday && p.Week == week);
        }

        /// <summary>
        /// 更新课程
        /// </summary>
        /// <param name="account">编号</param>
        /// <param name="lessonName">课程名</param>
        /// <param name="teacherName">老师名</param>
        /// <param name="location">上课地点</param>
        /// <param name="lessonDate">上课日期</param>
        /// <param name="startTime">上课节数</param>
        /// <param name="continueTime">持续节数</param>
        /// <param name="lastDate">更改之前的上课时间</param>
        /// <returns>返回能否更新</returns>
        public async Task<bool> UpdateLesson(string account, string lessonName, string teacherName,
            string location, DateTime lessonDate, string startTime, int continueTime, DateTime lastDate)
        {
            int start = startTime.Length == 4
                ? int.Parse(startTime.Substring(1, 1))
                : int.Parse(startTime.Substring(1, 2));
            TimeSpan d = lessonDate - await _semesterStorage.GetInitBeginDate();

            var dd = lessonDate - await _semesterStorage.GetInitBeginDate();
            var week = d.Days / 7 + 1;
            var weekday = d.Days % 7;
            var strattime = lessonDate.Hour - 7 > 5
                ? lessonDate.Hour - 9
                : lessonDate.Hour - 7;

            var strattimes = lastDate.Hour - 7 > 5
                ? lastDate.Hour - 9
                : lastDate.Hour - 7;

            var listss = await _lessonStorage.GetLessonsAsync(p =>
                p.Id == account && p.StartTime == strattimes && p.Weekday == weekday && p.Week == week);

            Lesson lesson = new Lesson()
            {
                Classroom = location,
                ContinueTime = continueTime,
                Id = account,
                Name = lessonName,
                StartTime = start,
                TeacherName = teacherName,
                Week = d.Days / 7 + 1,
                Weekday = d.Days % 7,
                Year = await _semesterStorage.GetInitSemester(),
                Kind = listss[0].ToHash()
            };

            if (!await IsRepeat(account, lesson.StartTime, lesson.ContinueTime,
                lesson.Week, lesson.Weekday))
            {
                return false;
            }

            var list = await _lessonStorage.GetLessonsAsync(p => p.Year == lesson.Year);
            foreach (var ilesson in list)
            {
                if (ilesson.Id == account && ilesson.Name != lessonName)
                {
                    return false;
                }
            }

            await DelLesson(account, lastDate);
            await _lessonStorage.AddLessonAsync(lesson);
            return true;
        }

        /******** 扩展方法 ********/
        /// <summary>
        /// 查看当前课程是否冲突
        /// </summary>
        public async Task<bool> IsRepeat(string account, int stratTime, int continueTime,
            int week, int weekday)
        {
            var year = await _semesterStorage.GetInitSemester();
            var list = await _lessonStorage.GetLessonsAsync(p =>
                p.Year == year && p.Week == week && p.Weekday == weekday);
            int record = 0;
            foreach (var lesson in list)
            {
                if (lesson.Id == account && lesson.StartTime == stratTime)
                {
                    continue;
                }
                for (var i = lesson.StartTime; i < lesson.StartTime + lesson.ContinueTime; i++)
                {

                    record = (record >> i) % 2 != 1 ? record += (1 << i) : record;
                }
            }

            for (var i = stratTime; i < continueTime + stratTime; i++)
            {
                if ((record >> i) % 2 == 1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 从课节转至时间
        /// </summary>
        public LessonShowData ChangeData(Lesson lesson, int colour)
        {
            List<TimeSpan> list = new List<TimeSpan>();
            list.Add(new TimeSpan(8, 30, 0));
            list.Add(new TimeSpan(9, 30, 0));
            list.Add(new TimeSpan(10, 40, 0));
            list.Add(new TimeSpan(11, 40, 0));
            list.Add(new TimeSpan(14, 00, 0));
            list.Add(new TimeSpan(15, 00, 0));
            list.Add(new TimeSpan(16, 10, 0));
            list.Add(new TimeSpan(17, 10, 0));
            list.Add(new TimeSpan(18, 30, 0));
            list.Add(new TimeSpan(19, 30, 0));
            list.Add(new TimeSpan(20, 30, 0));
            list.Add(new TimeSpan(21, 30, 0));
            LessonShowData lessonShowData = new LessonShowData();
            lessonShowData.Classroom = lesson.Classroom;
            lessonShowData.Id = lesson.Id;
            lessonShowData.TeacherName = lesson.TeacherName;
            lessonShowData.Name = lesson.Name;
            lessonShowData.colour = colour;
            lessonShowData.detail = lesson.Id + "\n" + lesson.Name + "\n" +
                                    lesson.TeacherName + "\n" + lesson.Classroom;
            var interval = (lesson.Week - 1) * 7 + lesson.Weekday % 7;
            lessonShowData.StartTime = beginDate.AddDays(interval)
                .Add(list[lesson.StartTime - 1]);
            lessonShowData.EndTime = beginDate.AddDays(interval)
                .Add(list[lesson.StartTime + lesson.ContinueTime - 2])
                .AddMinutes(50);
            return lessonShowData;
        }

        public async Task<IList<LessonShowData>> GetTodayLessonAsync()
        {
            beginDate = await _semesterStorage.GetInitBeginDate();
            Semester = await _semesterStorage.GetInitSemester();
            List<LessonShowData> lessonShowDatas = new List<LessonShowData>();

            var temp = DateTime.Today - beginDate;
            var week = (temp.Days / 7) + 1;
            var day = (temp.Days % 7);
            var lessons = await _lessonStorage.GetLessonsAsync(p =>
                p.Year == Semester && p.Week == week && p.Weekday == day);
            if (lessons == null)
            {
                return lessonShowDatas;
            }

            foreach (var lesson in lessons)
            {
                lessonShowDatas.Add(ChangeData(lesson, 1));
            }

            foreach (var lessonShowData in lessonShowDatas)
            {
                if (DateTime.Now > lessonShowData.EndTime)
                {
                    lessonShowData.colour = 0;
                }
            }

            lessonShowDatas.Sort((x, y) => ComparesTo(x, y));
            return lessonShowDatas;
        }

        public async Task<IList<LessonShowData>> GetTomorrowLessonAsync()
        {
            beginDate = await _semesterStorage.GetInitBeginDate();
            Semester = await _semesterStorage.GetInitSemester();
            List<LessonShowData> lessonShowDatas = new List<LessonShowData>();

            var temp = DateTime.Today - beginDate;
            var week = (temp.Days / 7) + 1;
            var day = (temp.Days % 7) + 1;
            var lessons = await _lessonStorage.GetLessonsAsync(p =>
                p.Year == Semester && p.Week == week && p.Weekday == day);
            if (lessons == null)
            {
                return lessonShowDatas;
            }

            foreach (var lesson in lessons)
            {
                lessonShowDatas.Add(ChangeData(lesson, 1));
            }

            foreach (var lessonShowData in lessonShowDatas)
            {
                if (DateTime.Now > lessonShowData.EndTime)
                {
                    lessonShowData.colour = 0;
                }
            }

            lessonShowDatas.Sort((x, y) => ComparesTo(x, y));
            return lessonShowDatas;
        }

        public async Task<IList<string>> GetLessonNameAsync()
        {
            var list = new List<string>();
            var lessonlist = await GetLesson();
            foreach (var lesson in lessonlist)
            {
                if (!list.Contains(lesson.Name))
                {
                    list.Add(lesson.Name);
                }
            }

            return list;
        }
    }

    /// <summary>
    /// view展示课程类
    /// </summary>
    public class LessonShowData
    {
        /// <summary>
        /// 主键(课程编号)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 教室
        /// </summary>
        public string Classroom { get; set; }

        public string detail { get; set; }

        /// <summary>
        /// 上课开始节次
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 上课持续时间
        /// </summary>
        public DateTime EndTime { get; set; }


        /// <summary>
        /// 颜色id
        /// </summary>
        public int colour { get; set; }
    }
}