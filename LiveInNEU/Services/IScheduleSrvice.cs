using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services.implementations;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 导航数据处理接口
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// 从数据库中获取全部目录
        /// </summary>
        Task<IList<Schedule>> GetSchedule();

        /// <summary>
        /// 获取所有一级目录
        /// </summary>
        Task<IList<Schedule>> GetFirstSchedule();

        /// <summary>
        /// 获取指定二级目录
        /// </summary>
        /// <param name="character">章节名</param>
        Task<IList<Schedule>> GetSecondSchedule(string character);


        Task UpdateScheduleAsync(Schedule schedule);
    }
}