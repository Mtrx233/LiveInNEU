using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LiveInNEU.Models
{
    [Table("datas")]
    public class Data
    {
        /// <summary>
        /// 主键(课程编号)
        /// </summary>
        [PrimaryKey]
        [SQLite.Column("id")]
        public string ID { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        [SQLite.Column("hash_code")]
        public string HashCode { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        [SQLite.Column("update_time")]
        public string UpdateTime { get; set; }
    }
}