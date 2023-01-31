using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services {
    /// <summary>
    /// 学期存储接口
    /// </summary>
    public interface ISemesterStorage
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
        /// 获取一个学期
        /// </summary>
        /// <param name="id">学期id。</param>
        Task<Semester> GetSemesterAsync(int id);

        /// <summary>
        ///  添加一个学期
        /// </summary>
        Task AddSemesterAsync(Semester semester);

        /// <summary>
        /// 获取满足给定条件的学期集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Semester>> GetSemestersAsync(
            Expression<Func<Semester, bool>> where);


        /// <summary>
        /// 得到当前学期开始时间
        /// </summary>
        /// <returns>开始时间</returns>
        Task<DateTime> GetInitBeginDate();

        /// <summary>
        /// 得到当前学期
        /// </summary>
        /// <returns></returns>
        Task<int> GetInitSemester();


        /// <summary>
        /// 自动选择学期
        /// </summary>
        Task<int> HelpSelected();

    }
}