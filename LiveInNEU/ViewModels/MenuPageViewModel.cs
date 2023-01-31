using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNEU.Views;

namespace LiveInNEU.ViewModels
{
    public class MenuPageViewModel
    {
        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand =
                new RelayCommand(() => PageAppearingCommandFunction()));

        public RelayCommand<Menu> ButtonClickCommand =>
            _buttonlickCommand ?? (_buttonlickCommand =
                new RelayCommand<Menu>(async menu =>
                {
                    await _routingService.NavigateToAsync(menu.Route);
                }));
        public ObservableCollection<Menu> MenusCollection { get; set; } = new ObservableCollection<Menu>(); 
        private async void PageAppearingCommandFunction()
        {
            if (!_inited)
            {
                var menusList = await _menuService.GetMenusAsync();
                foreach (var menu in menusList)
                {
                    MenusCollection.Add(menu);
                }

                _inited = true;
            }
        }
        
        private IMenuService _menuService;
        private RelayCommand _pageAppearingCommand;
        private bool _inited;
        private RelayCommand<Menu> _buttonlickCommand;
        private IRoutingService _routingService;

        public MenuPageViewModel(IMenuService menuService,IRoutingService routingService)
        {
            _menuService = menuService;
            _routingService = routingService;
            _inited = false;
        }
        
        
    }
}