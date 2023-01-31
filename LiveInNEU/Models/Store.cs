using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LiveInNEU.Models
{
    /// <summary>
    /// 收藏类
    /// </summary>
    [Table("stores")]
    public class Store
    {
        /// <summary>
        /// 对应题目编号
        /// </summary>
        [PrimaryKey]
        [SQLite.Column("question_id")]
        public int QuestionId { get; set; }

        /// <summary>
        /// 对应题目状态
        /// </summary>
        [SQLite.Column("is_store")]
        public int IsStore { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SQLite.Column("update_time")]
        public string UpdateTime { get; set; }

    }
}