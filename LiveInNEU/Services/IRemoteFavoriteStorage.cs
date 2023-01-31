using System.Collections.Generic;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services {
    public interface IRemoteFavoriteStorage : INotifyStatusChanged
    {   /// <summary>
        /// 获得所有收藏项，包括收藏与非收藏。
        /// </summary>
        Task<IList<Store>> GetFavoriteItemsAsync();

        /// <summary>
        /// 保存所有收藏项，包括收藏与非收藏。
        /// </summary>
        /// <param name="favoriteList">所有收藏项，包括收藏与非收藏。</param>
        Task<ServiceResult>
            SaveFavoriteItemsAsync(IList<Store> favoriteList);

        /// <summary>
        /// 是否已登录。
        /// </summary>
        Task<bool> IsSignedInAsync();

        /// <summary>
        /// 登录。
        /// </summary>
        Task<bool> SignInAsync();

        /// <summary>
        /// 注销。
        /// </summary>
        Task SignOutAsync();



    }
}