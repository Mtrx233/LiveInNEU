using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 导航存储接口
    /// </summary>
    public interface IScheduleStorage
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        bool Initialized();

        /// <summary>
        /// 初始化
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 获取一个导航
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<Schedule> GetScheduleAsync(Expression<Func<Schedule, bool>> where);

        /// <summary>
        /// 获取满足给定条件的导航集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Schedule>> GetSchedulesAsync(Expression<Func<Schedule, bool>> where);


        /// <summary>
        /// 更新导航
        /// </summary>
        Task UpdateScheduleAsync(Schedule schedule);
    }
}
