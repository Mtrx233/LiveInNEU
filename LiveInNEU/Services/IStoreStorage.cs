using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 收藏存储接口
    /// </summary>
    public interface IStoreStorage
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
        /// 获取一个收藏
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<Store> GetStoreAsync(Expression<Func<Store, bool>> where);

        /// <summary>
        /// 获取满足给定条件的收藏集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Store>> GetStoresAsync(Expression<Func<Store, bool>> where);


        /// <summary>
        /// 更新收藏
        /// </summary>
        Task UpdateStoreAsync(Store schedule);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        Task AddStoreAsync(Store store);

        /// <summary>
        /// 获取学号
        /// </summary>
        Task<string> GetUserName();
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="stores"></param>
        /// <returns></returns>
        Task UpStoresAsync(IList<Store> stores);
    }
}