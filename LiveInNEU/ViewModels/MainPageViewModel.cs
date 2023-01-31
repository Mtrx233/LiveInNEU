using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Plugin.LocalNotification;
using Xamarin.Essentials;

namespace LiveInNEU.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private List<LessonShow> todayLessonShows;

        private List<LessonShow> tomorrowLessonShows;

        private int isToday = 1;

        /// <summary>
        /// 后台加载服务
        /// </summary>
        private readonly ILoginService _loginService;

        /// <summary>
        /// 是否正在加载
        /// </summary>
        private bool _isLoading = true;

        /// <summary>
        /// 是否后台加载
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(nameof(IsLoading), ref _isLoading, value);
        }

        /// <summary>
        /// 当天课程信息
        /// </summary>
        public ObservableCollection<LessonShow> LessonCollection { get; set; } = new ObservableCollection<LessonShow>();

        /// <summary>
        /// 当天备忘录信息
        /// </summary>
        public ObservableCollection<Note> NoteCollection { get; set; } = new ObservableCollection<Note>();

        /// <summary>
        /// 课程服务
        /// </summary>
        private ILessonService _lessonService;


        public MainPageViewModel(ILessonService lessonService,
            ILoginService loginService,
            INoteService noteService)
        {
            _noteService = noteService;
            _lessonService = lessonService;
            _loginService = loginService;
        }


        public string TodayLesson
        {
            get => _todayLesson;
            set => Set(nameof(TodayLesson), ref _todayLesson, value);
        }

        private string _todayLesson = "今日课程";

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand _pageAppearingCommand;

        private bool _isInited;

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
            if (!Preferences.ContainsKey("InitNeed"))
            {
                await _loginService.GetDataAsync();
                Preferences.Set("InitNeed", 1);
            }

            todayLessonShows = new List<LessonShow>();
            tomorrowLessonShows = new List<LessonShow>();

            var lessonShowDatas = await _lessonService.GetTodayLessonAsync();
            TodayLesson = "今日课程:" + lessonShowDatas.Count + "节 || 单击模块切换明日课程";
            LessonCollection.Clear();
            NoteCollection.Clear();
            if (lessonShowDatas.Count == 0)
            {
                todayLessonShows.Add(new LessonShow()
                {
                    Name = "暂无", Length = 365, Color = "#F1F2F4",
                });
                LessonCollection.Add(new LessonShow()
                {
                    Name = "暂无", Length = 365, Color = "#F1F2F4",
                });
            }
            else
            {
                foreach (var lessonShowData in lessonShowDatas)
                {
                    todayLessonShows.Add(new LessonShow(lessonShowData));
                    LessonCollection.Add(new LessonShow(lessonShowData));
                }
            }

            var lessonShowDatasTemp =
                await _lessonService.GetTomorrowLessonAsync();
            if (lessonShowDatasTemp.Count == 0)
            {
                tomorrowLessonShows.Add(new LessonShow()
                {
                    Name = "暂无", Length = 365, Color = "#F1F2F4",
                });
            }
            else
            {
                foreach (var lessonShowData in lessonShowDatasTemp)
                {
                    tomorrowLessonShows.Add(new LessonShow(lessonShowData));
                }
            }

            var noteList = await _noteService.GetNotesAsync();
            foreach (var note in noteList)
            {
                NoteCollection.Add(note);
            }

            IsLoading = false;
        }

        private RelayCommand _changeLessonShow;
        private readonly INoteService _noteService;

        public RelayCommand ChangeLessonShow =>
            _changeLessonShow ?? (_changeLessonShow = new RelayCommand(() =>
                ChangeLessonShowFuc()));


        public async void ChangeLessonShowFuc()
        {
            LessonCollection.Clear();
            if (this.isToday == 1)
            {
                this.isToday = 0;
                foreach (var one in tomorrowLessonShows)
                {
                    LessonCollection.Add(one);
                }

                if (tomorrowLessonShows.Count == 1 && tomorrowLessonShows[0].Length > 100)
                {
                    TodayLesson = "明日课程:" + "0" + "节 || 单击模块切换今日课程";
                }
                else
                {
                    TodayLesson = "明日课程:" + tomorrowLessonShows.Count + "节 || 单击模块切换今日课程";
                }
            }
            else
            {
                this.isToday = 1;
                foreach (var one in todayLessonShows)
                {
                    LessonCollection.Add(one);
                }

                if (tomorrowLessonShows.Count == 1 && todayLessonShows[0].Length > 100)
                {
                    TodayLesson = "今日课程:" + "0" + "节 || 单击模块切换明日课程";
                }
                else
                {
                    TodayLesson = "今日课程:" + LessonCollection.Count + "节 || 单击模块切换明日课程";
                }
            }
        }
    }


    public class LessonShow
    {
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课程地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 课程开始时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 显示颜色
        /// </summary>
        public string Color { get; set; }


        /// <summary>
        /// 框的长度
        /// </summary>
        public int Length { get; set; }

        public LessonShow()
        {
        }

        public LessonShow(LessonShowData lessonShowData)
        {
            this.Name = lessonShowData.Name;
            this.Location = lessonShowData.Classroom;
            if (lessonShowData.colour == 1)
            {
                this.Color = "#35B9F5";
            }
            else
            {
                this.Color = "#F1F2F4";
            }

            this.BeginTime = lessonShowData.StartTime.GetDateTimeFormats('t')[0] + "-" +
                             lessonShowData.EndTime.GetDateTimeFormats('t')[0];
            this.Length = 95;
        }
    }
}