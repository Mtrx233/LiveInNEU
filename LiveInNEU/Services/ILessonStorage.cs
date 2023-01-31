using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services {
    /// <summary>
    /// 课程存储接口
    /// </summary>
    public interface ILessonStorage
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
        /// 获取一个课程
        /// </summary>
        /// <param name="id">课程id。</param>
        Task<Lesson> GetLessonAsync(string id);

        /// <summary>
        /// 添加一个课程
        /// </summary>
        Task AddLessonAsync(Lesson lesson);

        /// <summary>
        /// 获取满足给定条件的课程集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Lesson>> GetLessonsAsync(
            Expression<Func<Lesson, bool>> where);


        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task DeleteLessonAsync(Expression<Func<Lesson, bool>> @where);

        /// <summary>
        /// 获取用户名
        /// </summary>
        Task<string> GetAccount();

        /// <summary>
        /// 获取密码
        /// </summary>
        Task<string> GetPassword();
    }

    /// <summary>
    /// 获取存储的键值对
    /// </summary>
    public static class LessonStorageConstants
    {
        /// <summary>
        /// 学期键值对的索引
        /// </summary>
        public const string SEMESTER_KEY =
            nameof(LessonStorageConstants) + "." + nameof(SEMESTER);

        /// <summary>
        /// 账户键值对的索引
        /// </summary>
        public const string ACCOUNT_KEY =
            nameof(LessonStorageConstants) + "." + nameof(ACCOUNT);


        /// <summary>
        /// 密码键值对的索引
        /// </summary>
        public const string PASSWORD_KEY =
            nameof(LessonStorageConstants) + "." + nameof(PASSWORD);


        /// <summary>
        /// 更新时间键值对的索引
        /// </summary>
        public const string UPDATEDATE_KEY =
            nameof(LessonStorageConstants) + "." + nameof(UPDATETIME);


        /// <summary>
        /// 版本号键值对的索引
        /// </summary>
        public const string VERSION_KEY =
            nameof(LessonStorageConstants) + "." + nameof(VERSION);


        /// <summary>
        /// 学期开始时间键值对的索引
        /// </summary>
        public const string SEMESTER_START_KEY =
            nameof(LessonStorageConstants) + "." + nameof(SEMESTER_START);


        /// <summary>
        /// 校园网登录方式键值对的索引
        /// </summary>
        public const string KIND_KEY =
            nameof(LessonStorageConstants) + "." + nameof(KIND);



        /// 存储键值对的初始值
        public const string SEMESTER_START = "2021.9.5";
        public const string ACCOUNT = "00000000";
        public const string PASSWORD = "00000000";
        public const string UPDATETIME = "2021-11-23 22:13";
        public const int SEMESTER = 2021;
        public const int KIND = 1;
        public const int VERSION = 1;
    }


}