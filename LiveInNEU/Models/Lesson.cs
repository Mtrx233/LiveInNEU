using System.Security.Cryptography;
using System.Text;
using SQLite;

namespace LiveInNEU.Models {
    /// <summary>
    /// 课程类
    /// </summary>
    [Table("lessons")]
    public class Lesson
    {
        /// <summary>
        /// 主键(课程编号)
        /// </summary>
        [SQLite.Column("id")]
        public string Id { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        [SQLite.Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        [SQLite.Column("teacher_name")]
        public string TeacherName { get; set; }

        /// <summary>
        /// 教室
        /// </summary>
        [SQLite.Column("classroom")]
        public string Classroom { get; set; }

        /// <summary>
        /// 上课学期
        /// </summary>
        [SQLite.Column("year")]
        public int Year { get; set; }

        /// <summary>
        /// 上课周次
        /// </summary>
        [SQLite.Column("week")]
        public int Week { get; set; }

        /// <summary>
        /// 上课星期
        /// </summary>
        [SQLite.Column("weekday")]
        public int Weekday { get; set; }

        /// <summary>
        /// 上课开始节次
        /// </summary>
        [SQLite.Column("start_time")]
        public int StartTime { get; set; }

        /// <summary>
        /// 上课持续时间
        /// </summary>
        [SQLite.Column("continue_time")]
        public int ContinueTime { get; set; }

        /// <summary>
        /// 数据种类
        /// </summary>
        [SQLite.Column("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// 得到哈希
        /// </summary>
        public string ToHash()
        {
            string data = Id + Week.ToString() + StartTime.ToString() +
                Weekday.ToString() + ContinueTime.ToString() + Classroom +
                TeacherName + Name;
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}