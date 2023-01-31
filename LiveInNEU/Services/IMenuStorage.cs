using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    public interface IMenuStorage
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        bool Initialized();

        /// <summary>
        /// 初始化
        /// </summary>
        Task InitializeAsync();
        
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        Task<IList<Menu>> GetMenusAsync();
        
    }
}