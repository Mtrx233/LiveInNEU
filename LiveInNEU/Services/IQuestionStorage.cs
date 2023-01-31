using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 题目存储接口
    /// </summary>
    public interface IQuestionStorage
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
        /// 获取一个题目
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<Question> GetQuestionAsync(Expression<Func<Question, bool>> where);

        /// <summary>
        /// 获取满足给定条件的题目集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Question>> GetQuestionsAsync(Expression<Func<Question, bool>> where);


        /// <summary>
        /// 更新题目
        /// </summary>
        Task UpdateQuestionAsync(Question question);

        /// <summary>
        /// 题库同步跟新
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        Task UpdateQuestionStoreAsync(Question question);
    }
}