using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Services;
using LiveInNEU.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Menu = LiveInNEU.Models.Menu;

namespace LiveInNEU.ViewModels
{
    public class SelfPageViewModel : ViewModelBase
    {
        public RelayCommand UpdateCommand =>
            _updateCommand ?? (_updateCommand = new RelayCommand(async () =>
                await UpdateCommandFunction()));

        private async Task UpdateCommandFunction()
        {
            Preferences.Set("Update",1);
            await _routingService.NavigateToAsync(nameof(LessonPage));
        }

        public RelayCommand DataUpdateCommand =>
            _dataUpdateCommand ?? (_dataUpdateCommand = new RelayCommand(async () =>
                await DataUpdateCommandFunction()));

        private async Task DataUpdateCommandFunction() {
            var l= await _questionService.SynchronizationByOneDrive();
            if (l) {
                await _routingService.NavigateToAsync(nameof(QuestionCollectedPage));
            }
        }

        private RelayCommand _logoutCommand;
        private readonly IRoutingService _routingService;
        private RelayCommand _dataUpdateCommand;
        private readonly IQuestionService _questionService;
        private RelayCommand _updateCommand;

        public RelayCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new RelayCommand(() =>
                LogoutCommandFunction()));

        private void LogoutCommandFunction()
        {
            Preferences.Clear();
            _routingService.NavigateToAsync("//login");
        }

        public SelfPageViewModel(IRoutingService routingService, IQuestionService questionService)
        {
            _routingService = routingService;
            _questionService = questionService;
        }
    }
}