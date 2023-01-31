namespace LiveInNEU.Models {
    /// <summary>
    /// 学年度
    /// </summary>
    [SQLite.Table("semesters")]
    public class Semester
    {
        /// <summary>
        /// 主键（学年度）
        /// </summary>
        [SQLite.Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        [SQLite.Column("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// 课程周数
        /// </summary>
        [SQLite.Column("week_num")]
        public int WeekNum { get; set; }

        /// <summary>
        /// 课程开始周数
        /// </summary>
        [SQLite.Column("start_week")]
        public int StartWeek { get; set; }

    }
}