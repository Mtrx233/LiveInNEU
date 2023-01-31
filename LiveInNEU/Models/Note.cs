using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LiveInNEU.Models
{
    /// <summary>
    /// 备忘录类
    /// </summary>
    [Table("notes")]
    public class Note
    {
        /// <summary>
        /// 对应备忘录编号
        /// </summary>
        [PrimaryKey]
        [SQLite.Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 对应科目ID
        /// </summary>
        [SQLite.Column("subject_id")]
        public string SubjectId { get; set; }


        /// <summary>
        /// 对应标题
        /// </summary>
        [SQLite.Column("title")]
        public string Title { get; set; }

        /// <summary>
        /// 对应内容
        /// </summary>
        [SQLite.Column("page")]
        public string Page { get; set; }

        /// <summary>
        /// 对应创建时间
        /// </summary>
        [SQLite.Column("setup_time")]
        public string SetupTime { get; set; }

        /// <summary>
        /// 对应截止时间
        /// </summary>
        [SQLite.Column("end_time")]
        public string EndTime { get; set; }

        /// <summary>
        /// 对应完成时间
        /// </summary>
        [SQLite.Column("finish_time")]
        public string FinishTime { get; set; }

        /// <summary>
        /// 对应是否删除
        /// </summary>
        [SQLite.Column("is_delete")]
        public int IsDelete { get; set; }

        /// <summary>
        /// 对应是否完工
        /// </summary>
        [SQLite.Column("is_finish")]
        public int IsFinish { get; set; }

        /// <summary>
        /// 对应题库
        /// </summary>
        [SQLite.Column("questions")]
        public string Questions { get; set; }
    }
}
