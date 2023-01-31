using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 今日题目数据处理接口
    /// </summary>
    public interface IDailyService
    {

        /// <summary>
        /// 获取今日总题数
        /// </summary>
        Task<int> GetAllNumberAsync();

        /// <summary>
        /// 获取今日正确题数
        /// </summary>
        Task<int> GetRightNumberAsync();

        /// <summary>
        /// 获取今日准确率
        /// </summary>
        Task<double> GetRightAsync();

        /// <summary>
        /// 获取今日一级标题
        /// </summary>
        Task<IList<Daily>> GetFirstDailysAsync();

        /// <summary>
        /// 获取今日二级标题
        /// </summary>
        Task<IList<Daily>> GetSecondDailysAsync(string lessonName);


    }
}
