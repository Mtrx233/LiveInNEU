using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Views;
using Xamarin.Forms;
using Menu = LiveInNEU.Models.Menu;

namespace LiveInNEU.ViewModels
{
    public class QuestionDirectoryPageViewModel : ViewModelBase
    {
        // private RelayCommand _backCommand;
        //
        // public RelayCommand BackCommand =>
        //     _backCommand ?? (_backCommand =
        //         new RelayCommand(async () => await BackCommandFunction()));
        //
        // private async Task BackCommandFunction()
        // {
        //     if (_inited)
        //     {
        //         _inited = false;
        //         await PageAppearingCommandFunction();
        //     }
        //
        //     if (_storeInited)
        //     {
        //         _storeInited = false;
        //         await StorePageAppearingCommandFunction();
        //     }
        //
        //     await _routingService.GoBackAsync();
        // }

        public RelayCommand<Schedule> ScheduleTappedCommand =>
            _scheduleTappedCommand ?? (_scheduleTappedCommand =
                new RelayCommand<Schedule>(async schedule =>
                {
                    string url =
                        $"{nameof(QuestionPage)}?LessonNameChoose={schedule.LessonName}&CharacterChoose={schedule.Character}&SubjectNow={schedule.Now.ToString()}";
                    if (_storeInited)
                    {
                        url += "&IsStored=1";
                    }
                    await _routingService.NavigateToAsync(url);
                }));

        public ObservableCollection<Schedule> SchedulesCollection { get; set; } = new ObservableCollection<Schedule>();
        private RelayCommand _pageAppearingCommand;
        private readonly IScheduleService _scheduleService;
        private RelayCommand<Schedule> _scheduleTappedCommand;
        private readonly IRoutingService _routingService;
        private RelayCommand _scheduleExpandTappedCommand;
        private bool _inited;

        public IList<Schedule> ScheduleList { get; set; }

        private RelayCommand _storePageAppearingCommand;
        private bool _storeInited;
        private readonly IScheduleStorage _scheduleStorage;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand StorePageAppearingCommand =>
            _storePageAppearingCommand ?? (_storePageAppearingCommand =
                new RelayCommand(async () => await StorePageAppearingCommandFunction()));

        private async Task StorePageAppearingCommandFunction()
        {
            SchedulesCollection.Clear();
            ScheduleList = await _scheduleService.GetFirstSchedule();
            foreach (var schedule in ScheduleList)
            {
                if (schedule.StoreNum != 0)
                {
                    var schedulesList = await _scheduleService.GetSecondSchedule(schedule.LessonId);
                    schedule.Schedules = new ObservableCollection<Schedule>();
                    foreach (var s in schedulesList)
                    {
                        if (s.StoreNum != 0)
                        {
                            schedule.Schedules.Add(s);
                        }
                    }

                    SchedulesCollection.Add(schedule);
                }
            }
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand =
                new RelayCommand(async () => await PageAppearingCommandFunction()));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task PageAppearingCommandFunction()
        {
            SchedulesCollection.Clear();
            ScheduleList = await _scheduleService.GetFirstSchedule();
            foreach (var schedule in ScheduleList)
            {
                var schedulesList = await _scheduleService.GetSecondSchedule(schedule.LessonId);
                schedule.Schedules = new ObservableCollection<Schedule>();
                foreach (var s in schedulesList)
                {
                    schedule.Schedules.Add(s);
                }

                SchedulesCollection.Add(schedule);
            }
        }

        public QuestionDirectoryPageViewModel(IScheduleService scheduleService,
            IRoutingService routingService,
            IScheduleStorage scheduleStorage)
        {
            _scheduleStorage = scheduleStorage;
            _routingService = routingService;
            _scheduleService = scheduleService;
            _inited = false;
            _storeInited = false;
        }
    }
}