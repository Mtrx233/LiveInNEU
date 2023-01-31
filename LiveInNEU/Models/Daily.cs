using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LiveInNEU.Models
{
    /// <summary>
    /// 收藏类
    /// </summary>
    [Table("dailys")]
    public class Daily
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
        /// 刷题数目
        /// </summary>
        [SQLite.Column("number")]
        public int Number { get; set; }

        /// <summary>
        /// 正确数目
        /// </summary>
        [SQLite.Column("right_number")]
        public int RightNumber { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [SQLite.Column("test_time")]
        public string TestTime { get; set; }
    }
}
