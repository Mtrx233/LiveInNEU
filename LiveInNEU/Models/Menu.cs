using System.Collections.ObjectModel;
using LiveInNEU.ViewModels;
using SQLite;
using Xamarin.Forms;

namespace LiveInNEU.Models
{
    [Table("menus")]
    public class Menu
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SQLite.Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        [SQLite.Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [SQLite.Column("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 菜单路由
        /// </summary>
        [SQLite.Column("route")]
        public string Route { get; set; }

        public Menu(string name, string icon)
        {
            Name = name;
            Icon = icon;
        }

        public Menu()
        {
        }

        public static ObservableCollection<Menu> GetMenuCollection()
        {
            return new ObservableCollection<Menu>()
            {
                new Menu("个人设置","star_black.png"),
                new Menu("我的消息","star_black.png"),
                new Menu("我的成绩","star_black.png"),
                new Menu("刷新数据","star_black.png"),
                new Menu("关于我们","star_black.png")
            };
        }
    }
}