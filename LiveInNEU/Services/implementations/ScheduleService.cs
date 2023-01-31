using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 导航服务实现
    /// </summary>
    /// <author>殷昭伉</author>
    public class ScheduleService : IScheduleService
    {
        /******** 构造函数 ********/
        public ScheduleService(IScheduleStorage scheduleStorage)
        {
            this._scheduleStorage = scheduleStorage;
        }

        /******** 共有变量 ********/

        /******** 私有变量 ********/
        /// <summary>
        /// 导航信息存储
        /// </summary>
        private IScheduleStorage _scheduleStorage;


        /******** 继承方法 ********/


        /******** 扩展方法 ********/

        public async Task<IList<Schedule>> GetSchedule()
        {
            return await _scheduleStorage.GetSchedulesAsync(p => p.LessonId != "");
        }

        public async Task<IList<Schedule>> GetFirstSchedule()
        {
            return await _scheduleStorage.GetSchedulesAsync(p => p.Character == "First");
        }

        public int ComparesTo(Schedule x, Schedule y)
        {
            var xint = Convert.ToInt32(x.Character.Split(' ')[0]);
            var yint = Convert.ToInt32(y.Character.Split(' ')[0]);
            if (xint < yint)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public async Task<IList<Schedule>> GetSecondSchedule(string character) {
            List<Schedule> list =  (List<Schedule>)await _scheduleStorage.GetSchedulesAsync(p => p.Character != "First" && p.LessonId == character);
            list.Sort((x, y) => ComparesTo(x, y));
            return list;
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            await _scheduleStorage.UpdateScheduleAsync(schedule);
        }
    }
}