using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services.implementations
{
    public class MenuService:IMenuService
    {
        private readonly IMenuStorage _menuStorage;

        public MenuService(IMenuStorage menuStorage)
        {
            _menuStorage = menuStorage;
        }
        public async Task<IList<Menu>> GetMenusAsync()
        {
            return await _menuStorage.GetMenusAsync();
        }
    }
}