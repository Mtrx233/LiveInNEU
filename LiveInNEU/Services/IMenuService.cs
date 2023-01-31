using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    public interface IMenuService
    {
        Task<IList<Menu>> GetMenusAsync();
    }
}