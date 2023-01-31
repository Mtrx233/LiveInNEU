using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Scheduler.Internal;
using LiveInNEU.Models;
using LiveInNEU.Utils;
using Newtonsoft.Json;

namespace LiveInNEU.Services.implementations {
    /// <summary>
    /// 网络数据获取实现类
    /// </summary>
    /// <author>钱子昂、殷昭伉（网络部分由殷昭伉书写，数据处理部分由钱子昂书写）</author>
    public class NeuDataService : INeuDataService {
        /******** 构造函数 ********/
        public NeuDataService(ILessonStorage lessonStorage,
            ILoginStorage loginStorage, ISemesterStorage semesterStorage, IAlertService alertService,IDataStorage dataStorage)
        {
            _alertService = alertService;
            _lessonStorage = lessonStorage;
            _loginStorage = loginStorage;
            _semesterStorage = semesterStorage;
            _dataStorage = dataStorage;
            
        }

        /******** 公有变量 ********/

        /******** 私有变量 ********/
        /// <summary>
        /// ip网格地址
        /// </summary>
        private const string ipgwURL = "https://pass.neu.edu.cn/tpass/login";

        /// <summary>
        /// webvpn地址
        /// </summary>
        private const string webvpnURL =
            "https://pass.neu.edu.cn/tpass/login?service=https%3A%2F%2Fwebvpn.neu.edu.cn%2Flogin%3Fcas_login%3Dtrue";

        /// <summary>
        /// 课程信息获取地址
        /// </summary>
        private const string courseURL =
            "https://portal.neu.edu.cn/tp_up/up/widgets/getClassbyUserInfo";

        /// <summary>
        /// 学期开始时间获取地址
        /// </summary>
        private const string dateURL =
            "https://portal.neu.edu.cn/tp_up/up/widgets/getDatebyLearnweek";

        /// <summary>
        /// 课程信息存储
        /// </summary>
        private ILessonStorage _lessonStorage;

        /// <summary>
        /// 数据信息存储
        /// </summary>
        private IDataStorage _dataStorage;
        /// <summary>
        /// 登录信息存储
        /// </summary>
        private ILoginStorage _loginStorage;

        /// <summary>
        /// 弹窗功能
        /// </summary>
        private IAlertService _alertService;

        /// <summary>
        /// 学期信息存储
        /// </summary>
        private ISemesterStorage _semesterStorage;

        /// <summary>
        /// HTTP连接
        /// </summary>
        private HttpClient httpClient;

        /******** 继承方法 ********/
        /// <summary>
        /// 判断账户名和密码是否正确
        /// </summary>
        /// <param name="kind">网络途径</param>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
         public async Task<bool> IsRight(int kind, string username,
            string password) {
            var httpclientHandler =
                new HttpClientHandler() { UseCookies = true };
            httpClient = new HttpClient(httpclientHandler);
            var headers = httpClient.DefaultRequestHeaders;
            headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            var loginURL = kind == 1 ? ipgwURL : webvpnURL;
            HttpResponseMessage response;
            try {
                response = await httpClient.GetAsync(loginURL);
                response.EnsureSuccessStatusCode();
            } catch (Exception e) {
                await _alertService.ShowAlertAsync(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage("NEU", "200"),
                    ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                return false;
            }

            response.EnsureSuccessStatusCode();
            var resultLoginPage = await response.Content.ReadAsStringAsync();
            Regex reg = new Regex("name=\"lt\" value=\"(.+?)\"");
            string lt = reg.Match(resultLoginPage).Groups[1].Value;
            string tsa = username + password + lt;
            var ul = username.Length;
            var pl = password.Length;
            var fromContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("rsa", tsa),
                new KeyValuePair<string, string>("ul", ul.ToString()),
                new KeyValuePair<string, string>("pl", pl.ToString()),
                new KeyValuePair<string, string>("lt", lt),
                new KeyValuePair<string, string>("execution", "e1s1"),
                new KeyValuePair<string, string>("_eventId", "submit"),
            });
            try {
                var getCookie =
                    httpClient.PostAsync(loginURL, fromContent).Result;
                response.EnsureSuccessStatusCode();
                if (getCookie.StatusCode != HttpStatusCode.Redirect)
                {
                    await _alertService.ShowAlertAsync(
                        ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                        "账号或密码错误",
                        ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                    return false;
                }
                response.EnsureSuccessStatusCode();
            } catch (Exception e) {
                //密码错误
                await _alertService.ShowAlertAsync(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage("NEU", "200"),
                    ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                return false;
            }
            await _loginStorage.StorageOfInfo(kind, username, password);
            return true;
        }

        /// <summary>
        /// 网络数据处理并存储
        /// </summary>
        public async Task DataStorageAsync()
        {
            List<Lesson> lessons = new List<Lesson>();
            List<LessonData> lesonGets = await GetLessonDataAsync();
            var groups =
                lesonGets.GroupBy(x => x, new LessonEqualityComparer());
            foreach (var group in groups)
            {
                string key = group.Key.KKXND.Substring(0, 4);
                Semester semester = new Semester() {
                    Id = int.Parse(key) * 10 + int.Parse(group.Key.KKXQM),
                    StartDate = await GetBeginning(int.Parse(key) * 10 + int.Parse(group.Key.KKXQM))
                };

                string fullWeek = "0000000000000000000000000000000000000000000000000000";
                group.ForEach(p=> fullWeek = or(fullWeek, p.SKZC));

                int start = get_num(fullWeek) / 100;
                int end = get_num(fullWeek) % 100;
                semester.StartWeek = start;
                semester.WeekNum = end - start + 1;

                LessonData lastOne = new LessonData() {
                    KCH = "null"
                };

                foreach (var lessonData in group)
                {
                    if (lastOne.KCH != "null" && lessonData.KCMC == lastOne.KCMC &&
                        lessonData.JXDD == lastOne.JXDD &&
                        lessonData.SKXQ == lastOne.SKXQ &&
                        lessonData.SKJC == lastOne.SKJC)
                    {
                        lessonData.JSXM = lastOne.JSXM + "," + lessonData.JSXM;
                    }
                    else if (lastOne.KCH != "null")
                    {
                        await AddIntoStorageAsync(lastOne, start, end, semester.Id);
                    }
                    lastOne = lessonData;
                }
                await AddIntoStorageAsync(lastOne, start, end, semester.Id);
                await _semesterStorage.AddSemesterAsync(semester);
            }
            await _semesterStorage.HelpSelected();
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <returns>消息队列</returns>
        public async Task<string> Update()
        {
            //检测更新时间
            DateTime lasTime = DateTime.Parse(await _dataStorage.GetUpdateTime());
            TimeSpan last = DateTime.Now - lasTime;
            if (last.Days < 7)
            {
                return "暂时无需更新";
            }

            //开始更新
            await _dataStorage.SetUpdateTime(DateTime.Now.ToString());
            string result = "";

            //登录校园网
            await IsRight(1,
                await _lessonStorage.GetAccount(),
                await _lessonStorage.GetPassword());
            var list = await GetLessonDataAsync();

            //检测更新
            foreach (var data in list)
            {
                var record = await _dataStorage.GetDatasAsync(p => p.ID == data.KCH);

                List<string> hashs = new List<string>();
                record.ForEach(p=>hashs.Add(p.HashCode));
                
                if (!hashs.Contains(data.ToHash()))
                {
                    result += data.KCMC + "更新了" + ";";
                }
            }

            if (result == "")
            {
                result = "无课程更新";
                await _alertService.ShowsAlertAsync(ErrorMessages.UPDATE_MESSAGE, result,
                    ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                return result;
            }

            await _dataStorage.DeleteDatasAsync();
            await _lessonStorage.DeleteLessonAsync(p => p.Kind == "0");
            await DataStorageAsync();

            //检测自己添加课程的冲突性
            var lists = await _lessonStorage.GetLessonsAsync(p => p.Kind == "1");
            if (lists != null)
            {
                foreach (var lesson in lists)
                {
                    if (!await IsRepeat(lesson.Id, lesson.StartTime, lesson.ContinueTime, lesson.Week, lesson.Weekday))
                    {
                        /*
                        await _lessonStorage.DeleteLessonAsync(p =>
                            p.Kind == "1" && p.Id == lesson.Id &&
                            p.Week == lesson.Week && p.Weekday == lesson.Weekday &&
                            p.StartTime == lesson.StartTime);
                        */
                        result += lesson.Name + "发生冲突（新建课程）" + ";";
                    }
                }
            }
            

            //将从前的修改重新写入
            var listss = await _lessonStorage.GetLessonsAsync(p => p.Kind != "1" && p.Kind != "0");
            if (lists != null)
            {
                foreach (var lesson in listss)
                {
                    if (!await IsRepeat(lesson.Id, lesson.StartTime, lesson.ContinueTime, lesson.Week, lesson.Weekday))
                    {
                        /*
                        await _lessonStorage.DeleteLessonAsync(p =>
                            p.Kind == "1" && p.Id == lesson.Id &&
                            p.Week == lesson.Week && p.Weekday == lesson.Weekday &&
                            p.StartTime == lesson.StartTime);
                        */
                        result += lesson.Name + "发生冲突（改动课程）" + ";";
                    }
                    else
                    {
                        var sum = await _lessonStorage.GetLessonsAsync(p => p.Id == lesson.Id);
                        List<string> sum_hash = new List<string>();
                        sum.ForEach(p=> sum_hash.Add(p.ToHash()));
                    
                        if (!sum_hash.Contains(lesson.Kind))
                        {
                            lesson.Kind = "1";
                            result += lesson.Name + "的原修改已更替为新建课程" + ";";
                        }

                        else
                        {
                            foreach (var lessonss in sum)
                            {
                                if (lessonss.ToHash() == lesson.Kind)
                                {
                                    await _lessonStorage.DeleteLessonAsync(p =>
                                        p.Kind == "0" && p.Id == lessonss.Id &&
                                        p.Week == lessonss.Week && p.Weekday == lessonss.Weekday &&
                                        p.StartTime == lessonss.StartTime);
                                    result += lesson.Name + "的原修改已更新到课程" + ";";
                                }
                            }
                        }
                    }
                }
            }
            
            await _alertService.ShowsAlertAsync(ErrorMessages.UPDATE_MESSAGE, result,
                ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
            return result;
        }

        public async Task<string> Update(int a)
        {
            //检测更新时间
            DateTime lasTime = DateTime.Parse(await _dataStorage.GetUpdateTime());
            TimeSpan last = DateTime.Now - lasTime;

            //开始更新
            await _dataStorage.SetUpdateTime(DateTime.Now.ToString());
            string result = "";

            //登录校园网
            await IsRight(1,
                await _lessonStorage.GetAccount(),
                await _lessonStorage.GetPassword());
            var list = await GetLessonDataAsync();

            //检测更新
            foreach (var data in list)
            {
                var record = await _dataStorage.GetDatasAsync(p => p.ID == data.KCH);

                List<string> hashs = new List<string>();
                record.ForEach(p=>hashs.Add(p.HashCode));
                
                if (!hashs.Contains(data.ToHash()))
                {
                    result += data.KCMC + "更新了" + ";";
                }
            }

            if (result == "")
            {
                result = "无课程更新";
                await _alertService.ShowsAlertAsync(ErrorMessages.UPDATE_MESSAGE, result,
                    ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                return result;
            }

            await _dataStorage.DeleteDatasAsync();
            await _lessonStorage.DeleteLessonAsync(p => p.Kind == "0");
            await DataStorageAsync();

            //检测自己添加课程的冲突性
            var lists = await _lessonStorage.GetLessonsAsync(p => p.Kind == "1");
            if (lists != null)
            {
                foreach (var lesson in lists)
                {
                    if (!await IsRepeat(lesson.Id, lesson.StartTime, lesson.ContinueTime, lesson.Week, lesson.Weekday))
                    {
                        /*
                        await _lessonStorage.DeleteLessonAsync(p =>
                            p.Kind == "1" && p.Id == lesson.Id &&
                            p.Week == lesson.Week && p.Weekday == lesson.Weekday &&
                            p.StartTime == lesson.StartTime);
                        */
                        result += lesson.Name + "发生冲突（新建课程）" + ";";
                    }
                }
            }
            

            //将从前的修改重新写入
            var listss = await _lessonStorage.GetLessonsAsync(p => p.Kind != "1" && p.Kind != "0");
            if (lists != null)
            {
                foreach (var lesson in listss)
                {
                    if (!await IsRepeat(lesson.Id, lesson.StartTime, lesson.ContinueTime, lesson.Week, lesson.Weekday))
                    {
                        /*
                        await _lessonStorage.DeleteLessonAsync(p =>
                            p.Kind == "1" && p.Id == lesson.Id &&
                            p.Week == lesson.Week && p.Weekday == lesson.Weekday &&
                            p.StartTime == lesson.StartTime);
                        */
                        result += lesson.Name + "发生冲突（改动课程）" + ";";
                    }
                    else
                    {
                        var sum = await _lessonStorage.GetLessonsAsync(p => p.Id == lesson.Id);
                        List<string> sum_hash = new List<string>();
                        sum.ForEach(p=> sum_hash.Add(p.ToHash()));
                    
                        if (!sum_hash.Contains(lesson.Kind))
                        {
                            lesson.Kind = "1";
                            result += lesson.Name + "的原修改已更替为新建课程" + ";";
                        }

                        else
                        {
                            foreach (var lessonss in sum)
                            {
                                if (lessonss.ToHash() == lesson.Kind)
                                {
                                    await _lessonStorage.DeleteLessonAsync(p =>
                                        p.Kind == "0" && p.Id == lessonss.Id &&
                                        p.Week == lessonss.Week && p.Weekday == lessonss.Weekday &&
                                        p.StartTime == lessonss.StartTime);
                                    result += lesson.Name + "的原修改已更新到课程" + ";";
                                }
                            }
                        }
                    }
                }
            }
            
            await _alertService.ShowsAlertAsync(ErrorMessages.UPDATE_MESSAGE, result,
                ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
            return result;
        }
        /******** 扩展方法 ********/

        public async Task AddIntoStorageAsync(LessonData lastOne, int start,
            int end, int date) {
            Data datas = new Data() {
                HashCode = lastOne.ToHash(),
                ID = lastOne.KCH,
                UpdateTime = DateTime.Now.ToString()
            };
            await _dataStorage.AddDataAsync(datas);
            lastOne.SKZC = lastOne.SKZC.Substring(start, end - start + 1);
            for (int i = 0; i < end - start + 1; i++) {
                if (lastOne.SKZC.Substring(i, 1) == "1") {
                    Lesson lesson = new Lesson()
                    {
                        Id = lastOne.KCH,
                        Classroom = lastOne.JXDD,
                        ContinueTime = int.Parse(lastOne.CXJC),
                        Name = lastOne.KCMC,
                        StartTime = int.Parse(lastOne.SKJC),
                        TeacherName = lastOne.JSXM,
                        Week = i + 1,
                        Year = date,
                        Weekday = int.Parse(lastOne.SKXQ),
                        Kind = "0"
                    };
                    await _lessonStorage.AddLessonAsync(lesson);
                }
            }
        }

        /// <summary>
            /// 得到课程原始数据
            /// </summary>
            /// <returns></returns>
            public async Task<List<LessonData>> GetLessonDataAsync() {
            HttpContent content = new StringContent(
                "{ \"schoolYear\":\"0000-0000\", \"semester\":\"0\", \"learnWeek\":\"00\" }",
                Encoding.UTF8, "application/json");

            HttpResponseMessage result;
            try {
                await httpClient.GetAsync(courseURL);
                result = await httpClient.PostAsync(courseURL, content);
                result.EnsureSuccessStatusCode();
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }

            var courseJson = await result.Content.ReadAsStringAsync();
            var courses =
                JsonConvert.DeserializeObject<List<LessonData>>(courseJson);
            return courses;
        }

        /// <summary>
        /// 得到学期开始
        /// </summary>
        /// <param name="yearAndTerm"></param>
        /// <returns></returns>
        public async Task<string> GetBeginning(int yearAndTerm) {
            int years = yearAndTerm / 10;
            int semester = yearAndTerm % 10;
            HttpContent content = new StringContent(
                "{\"SCHOOL_YEAR\":\"" + years.ToString() + "-" +
                (years + 1).ToString() + "\",\"SEMESTER\":\"" +
                semester.ToString() + "\",\"learnWeek\":\"01\"}", Encoding.UTF8,
                "application/json");
            HttpResponseMessage result;
            try {
                await httpClient.GetAsync(dateURL);
                result = await httpClient.PostAsync(dateURL, content);
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }

            var dateJson = await result.Content.ReadAsStringAsync();
            Regex regs = new Regex("date1\":\"(.+?)\"");
            var firstDate = regs.Match(dateJson).Groups[1].Value;
            return firstDate;
        }

        /// <summary>
        /// 字符串取或运算
        /// </summary>
        public static string or(string str1, string str2) {
            for (int i = 0; i < str1.Length && i < str2.Length; i++) {
                if (str1.Substring(i, 1) == "1" || str2.Substring(i, 1) == "1") {
                    str1 = str1.Remove(i, 1).Insert(i, "1");
                }
            }

            return str1;
        }

        /// <summary>
        /// 去掉字符串中的0（返回开始与末尾）
        /// </summary>
        public static int get_num(string week) {
            int start = 54;
            int end = 0;
            for (int i = 0; i < week.Length; i++) {
                if (week.Substring(i, 1) == "1") {
                    start = i < start ? i : start;
                    end = i;
                }
            }
            return (start * 100) + end;
        }

        /// <summary>
        /// 查看当前课程是否冲突
        /// </summary>
        public async Task<bool> IsRepeat(string account, int stratTime, int continueTime,
            int week, int weekday)
        {
            var year = await _semesterStorage.GetInitSemester();
            var list = await _lessonStorage.GetLessonsAsync(p => p.Year == year && p.Week == week && p.Weekday == weekday);
            int record = 0;
            foreach (var lesson in list)
            {
                for (var i = lesson.StartTime; i < lesson.StartTime + lesson.ContinueTime; i++)
                {
                    if (lesson.Id == account && lesson.StartTime == stratTime && lesson.ContinueTime == continueTime)
                    {
                        continue;
                    }
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
    }

    /// <summary>
    /// 教务处JSON对应类
    /// </summary>
    public class LessonData {
        /// <summary>
        /// 教师姓名
        /// </summary>
        public string JSXM { get; set; }

        /// <summary>
        /// 开课学年度
        /// </summary>
        public string KKXND { get; set; }

        /// <summary>
        /// 开课学期末
        /// </summary>
        public string KKXQM { get; set; }

        /// <summary>
        /// 终止周
        /// </summary>
        public int ZZZ { get; set; }

        /// <summary>
        /// 上课星期
        /// </summary>
        public string SKXQ { get; set; }

        /// <summary>
        /// 起始周
        /// </summary>
        public int QSZ { get; set; }

        /// <summary>
        /// 持续节次
        /// </summary>
        public string CXJC { get; set; }

        /// <summary>
        /// 上课周程
        /// </summary>
        public string SKZC { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string KCMC { get; set; }

        /// <summary>
        /// 教学地点
        /// </summary>
        public string JXDD { get; set; }

        /// <summary>
        /// 上课节次
        /// </summary>
        public string SKJC { get; set; }

        /// <summary>
        /// 课程号
        /// </summary>
        public string KCH { get; set; }

        /// <summary>
        /// 获取哈希
        /// </summary>
        public string ToHash()
        {
            string data = CXJC + JXDD + KCH + KCMC + KKXND + KKXQM + QSZ.ToString() + SKJC + SKXQ + SKZC + ZZZ.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}