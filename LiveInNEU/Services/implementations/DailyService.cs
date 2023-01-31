using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Scheduler.Internal;
using LiveInNEU.Models;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 今日刷题服务实现
    /// </summary>
    /// <author></author>
    public class DailyService : IDailyService
    {
        /******** 构造函数 ********/
        public DailyService(IDailyStorage dailyStorage) {
            this._dailyStorage = dailyStorage;
        }

        /******** 共有变量 ********/

        /******** 私有变量 ********/
        /// <summary>
        /// 今日刷题信息存储
        /// </summary>
        private IDailyStorage _dailyStorage;

        /******** 继承方法 ********/


        /******** 扩展方法 ********/


        public async Task<int> GetAllNumberAsync() {
            int sum = 0;
            IList<Daily> list = await _dailyStorage.GetDailysAsync(p =>
                p.TestTime == DateTime.Now.ToString("d"));
            list.ForEach(p=>sum += p.Number);
            return sum;
        }

        public async Task<int> GetRightNumberAsync() {
            int sum = 0;
            IList<Daily> list = await _dailyStorage.GetDailysAsync(p =>
                p.TestTime == DateTime.Now.ToString("d"));
            list.ForEach(p => sum += p.RightNumber);
            return sum;
        }

        public async Task<double> GetRightAsync() {
            double sum = 0;
            double right_sum = 0;
            IList<Daily> list = await _dailyStorage.GetDailysAsync(p =>
                p.TestTime == DateTime.Now.ToString("d"));
            list.ForEach(p => sum += p.Number);
            list.ForEach(p => right_sum += p.RightNumber);
            return right_sum/sum;
        }

        public Task<IList<Daily>> GetFirstDailysAsync() {
            return _dailyStorage.GetDailysAsync(p => p.Character == "First");
        }

        public Task<IList<Daily>> GetSecondDailysAsync(string lessonName) {
            return _dailyStorage.GetDailysAsync(p => p.Character != "First" && p.LessonName == lessonName);
        }
    }
}
