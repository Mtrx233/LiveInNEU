using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using SQLite;

namespace LiveInNEU.Models
{
        /// <summary>
        /// 题目类
        /// </summary>
        [Table("questions")]
        public class Question
        {
            /// <summary>
            /// 对应课程编号
            /// </summary>
            [SQLite.Column("id")]
            public int Id { get; set; }

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
            /// 序号
            /// </summary>
            [SQLite.Column("number")]
            public int Number { get; set; }

            /// <summary>
            /// 题目
            /// </summary>
            [SQLite.Column("subject")]
            public string Subject { get; set; }

            /// <summary>
            /// 选项A
            /// </summary>
            [SQLite.Column("option_a")]
            public string OptionA { get; set; }

            /// <summary>
            /// 选项B
            /// </summary>
            [SQLite.Column("option_b")]
            public string OptionB { get; set; }

            /// <summary>
            /// 选项C
            /// </summary>
            [SQLite.Column("option_c")]
            public string OptionC { get; set; }

            /// <summary>
            /// 选项D
            /// </summary>
            [SQLite.Column("option_d")]
            public string OptionD { get; set; }

            /// <summary>
            /// 题目解析
            /// </summary>
            [SQLite.Column("analysis")]
            public string Analysis { get; set; }

            /// <summary>
            /// 正确答案
            /// </summary>
            [SQLite.Column("answer")]
            public int Answer { get; set; }

            /// <summary>
            /// 练习历史
            /// </summary>
            [SQLite.Column("is_tested")]
            public int IsTested { get; set; }

            /// <summary>
            /// 收藏
            /// </summary>
            [SQLite.Column("store_up")]
            public int StoreUp { get; set; }

            /// <summary>
            /// 收藏时间
            /// </summary>
            [SQLite.Column("store_up_time")]
            public string StoreUpTime { get; set; }
        }
}
