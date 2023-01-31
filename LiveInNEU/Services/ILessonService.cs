using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveInNEU.Services.implementations;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 课程数据处理接口
    /// </summary>
    public interface ILessonService
    {
        /// <summary>
        /// 从数据库中获取全部课程
        /// </summary>
        Task<IList<LessonShowData>> GetLesson();

        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="account">编号</param>
        /// <param name="lessonName">课程名</param>
        /// <param name="teacherName">老师名</param>
        /// <param name="location">上课地点</param>
        /// <param name="lessonDate">上课日期</param>
        /// <param name="startTime">上课节数</param>
        /// <param name="continueTime">持续节数</param>
        /// <returns>返回能否添加</returns>
        Task<bool> AddLesson(string account, string lessonName,
            string teacherName, string location, DateTime lessonDate,
            string startTime, int continueTime);

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="account">编号</param>
        /// <param name="lessonDate">具体的上课时间</param>
        Task DelLesson(string account, DateTime lessonDate);

        /// <summary>
        /// 更新课程
        /// </summary>
        /// <param name="account">编号</param>
        /// <param name="lessonName">课程名</param>
        /// <param name="teacherName">老师名</param>
        /// <param name="location">上课地点</param>
        /// <param name="lessonDate">上课日期</param>
        /// <param name="startTime">上课节数</param>
        /// <param name="continueTime">持续节数</param>
        /// <param name="lastDate">更改之前的上课时间</param>
        /// <returns>返回能否更新</returns>
        Task<bool> UpdateLesson(string account, string lessonName, string teacherName,
            string location, DateTime lessonDate, string startTime, int continueTime, DateTime lastDate);

        /// <summary>
        /// 得到当天课程信息
        /// </summary>
        /// <returns></returns>
        Task<IList<LessonShowData>> GetTodayLessonAsync();

        /// <summary>
        /// 得到明天课程
        /// </summary>
        /// <returns></returns>
        Task<IList<LessonShowData>> GetTomorrowLessonAsync();
        Task<IList<string>> GetLessonNameAsync();
        
    }
}