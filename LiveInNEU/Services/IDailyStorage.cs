using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 今日刷题存储接口
    /// </summary>
    public interface IDailyStorage
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
        /// 获取一个今日刷题
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<Daily> GetDailyAsync(Expression<Func<Daily, bool>> where);

        /// <summary>
        /// 获取满足给定条件的今日刷题集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Daily>> GetDailysAsync(Expression<Func<Daily, bool>> where);

        /// <summary>
        /// 更新今日刷题
        /// </summary>
        Task UpdateDailyAsync(Daily daily);

        /// <summary>
        /// 添加一个今日刷题
        /// </summary>
        Task AddDailyAsync(Daily daily);
    }
}
