using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using SQLite;
using Xamarin.CommunityToolkit.Behaviors;

namespace LiveInNEU.Models
{
    /// <summary>
    /// 题库菜单类
    /// </summary>
    [Table("schedules")]
    public class Schedule
    {
        /// <summary>
        /// 对应课程编号
        /// </summary>
        [SQLite.Column("lesson_id")]
        public string LessonId { get; set; }

        /// <summary>
        /// 对应课程名
        /// </summary>
        [SQLite.Column("lesson_name")]
        public string LessonName { get; set; }

        /// <summary>
        /// 章节名称
        /// </summary>
        [SQLite.Column("character")]
        public string Character { get; set; }

        /// <summary>
        /// 总题数
        /// </summary>
        [SQLite.Column("sum")]
        public int Sum { get; set; }

        /// <summary>
        /// 已完成题目数
        /// </summary>
        [SQLite.Column("finished")]
        public int Finished { get; set; }

        /// <summary>
        /// 当前进程
        /// </summary>
        [SQLite.Column("now")]
        public int Now { get; set; }

        /// <summary>
        /// 收藏题目数量
        /// </summary>
        [SQLite.Column("store_num")]
        public int StoreNum { get; set; }

        [SQLite.Ignore] public ObservableCollection<Schedule> Schedules { get; set; }
    }
}