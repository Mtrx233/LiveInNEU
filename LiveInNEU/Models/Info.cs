using SQLite;

namespace LiveInNEU.Models
{
    [Table("infos")]
    public class Info
    {
        /// <summary>
        /// 主键(课程编号)
        /// </summary>
        [PrimaryKey]
        [SQLite.Column("key")]
        public string KEY { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        [SQLite.Column("value")]
        public string Value { get; set; }
    }
}
