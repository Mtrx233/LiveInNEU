using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 题目数据处理接口
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// 按照序号从数据库中获取三个题目
        /// </summary>
        Task<IList<Question>> GetQuestinsAsync(string lessonName, string character, int num);

        /// <summary>
        /// 练习题目
        /// </summary>
        Task FinishQuestionAsync(string lessonName, string character, int num);

        /// <summary>
        /// 按照序号从数据库中获取三个收藏题目
        /// </summary>
        Task<IList<Question>> GetStoreUpsAsync(string lessonName, string character, int num);

        /// <summary>
        /// 收藏/取消收藏
        /// </summary>
        Task SetStoreUpAsync(string lessonName, string character, int num);

        /// <summary>
        /// 按照序号从数据库中获取章节题目
        /// </summary>
        Task<IList<Question>> GetQuestinsAsync(string lessonName, string character);

        /// <summary>
        /// IOS数据同步
        /// </summary>
        /// <returns></returns>
        Task<bool> Synchronization();

        /// <summary>
        /// 安卓同步
        /// </summary>
        /// <returns></returns>
        Task<bool> SynchronizationByOneDrive();
    }
}