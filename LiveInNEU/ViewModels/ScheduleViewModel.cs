using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Views;
using Plugin.LocalNotification;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LiveInNEU.ViewModels
{
    public class ScheduleViewModel : ViewModelBase, IQueryAttributable, INotifyPropertyChanged
    {
        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<Feature> _featureRouteCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<Feature> FeatureRouteCommand =>
            _featureRouteCommand ?? (_featureRouteCommand =
                new RelayCommand<Feature>(async feature =>
                {
                    var featureArray = feature.title.Split('-');
                    await _routingService.NavigateToAsync(
                        $"{nameof(QuestionPage)}?LessonNameChoose={featureArray[0]}&CharacterChoose={featureArray[1]}&SubjectNow={featureArray[2]}");
                }));

        public ObservableCollection<NoteView> notes { get; set; } = new ObservableCollection<NoteView>();

        /// <summary>
        /// 登陆验证服务
        /// </summary>
        public NoteView _note { set; get; } = new NoteView();

        /// <summary>
        /// 登陆验证服务
        /// </summary>
        private INoteService _noteService;

        private ILessonService _lessonService;

        private IScheduleService _scheduleService;

        public ScheduleViewModel(INoteService noteService, ILessonService lessonService,
            IScheduleService scheduleService, IRoutingService routingService)
        {
            _routingService = routingService;
            _noteService = noteService;
            _lessonService = lessonService;
            _scheduleService = scheduleService;
            
        }


        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand _pageAppearingCommand;

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
            // time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            notes.Clear();
            ObservableCollection<Note> notess = new ObservableCollection<Note>();
            var list = _lessonName.Equals("all")
                ? await _noteService.GetNotesAsync()
                : await _noteService.GetNotesAsync(_lessonName);

            foreach (var note in list)
            {
                notess.Add(note);
            }

            foreach (var note in notess)
            {
                notes.Add(NoteView.NoteToNoteView(note));
            }

            await SetValue();
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> _deleteItemCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> DeleteItemCommand =>
            _deleteItemCommand ?? (_deleteItemCommand =
                new RelayCommand<NoteView>(async p => await DeleteItemCommandFunction(p)));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task DeleteItemCommandFunction(NoteView noteview)
        {
            await _noteService.DeleteNoteAsync(noteview.Id);
            await PageAppearingCommandFunction();
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> _addItemCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> AddItemCommand =>
            _addItemCommand ?? (_addItemCommand =
                new RelayCommand<NoteView>(async p => await AddItemCommandFunction(p)));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task AddItemCommandFunction(NoteView noteview)
        {
            await _noteService.DeleteNoteAsync(noteview.Id);
            await PageAppearingCommandFunction();
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> _doingItemCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> DoingItemCommand =>
            _doingItemCommand ?? (_doingItemCommand =
                new RelayCommand<NoteView>(async p => await DoingCommandFunction(p)));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task DoingCommandFunction(NoteView noteview)
        {
            notes.Clear();
            ObservableCollection<Note> notess = new ObservableCollection<Note>();
            var list = await _noteService.GetNotesAsync();

            foreach (var note in list)
            {
                notess.Add(note);
            }

            foreach (var note in notess)
            {
                notes.Add(NoteView.NoteToNoteView(note));
            }
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> _doneItemCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> DoneItemCommand =>
            _doneItemCommand ?? (_doneItemCommand =
                new RelayCommand<NoteView>(async p => await DoneCommandFunction(p)));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task DoneCommandFunction(NoteView noteview)
        {
            notes.Clear();
            ObservableCollection<Note> notess = new ObservableCollection<Note>();
            var list = await _noteService.GetFinishNotesAsync();

            foreach (var note in list)
            {
                notess.Add(note);
            }

            foreach (var note in notess)
            {
                notes.Add(NoteView.NoteToNoteView(note));
            }
        }


        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> _finishItemCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand<NoteView> FinishItemCommand =>
            _finishItemCommand ?? (_finishItemCommand =
                new RelayCommand<NoteView>(async p => await FinishItemCommandFunction(p)));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task FinishItemCommandFunction(NoteView noteview)
        {
            await _noteService.FinishNoteAsync(noteview.Id);
            await PageAppearingCommandFunction();
        }

        public string title { get; set; }

        public string page { get; set; }

        public string question { get; set; }

        public string lesson { get; set; }

        public string character { get; set; }

        public DateTime DDL
        {
            get { return _ddl; }

            set
            {
                if (_ddl != value)
                {
                    _ddl = value;
                    OnPropertyChanged("DDL");
                }
            }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        public string Date { get; set; } = DateTime.Now.ToString("D",
            CultureInfo.CreateSpecificCulture("en-US"));

        public string lessons { get; set; }

        public string Msg
        {
            get { return _msg; }
            set
            {
                if (_msg != value)
                {
                    _msg = value;
                    OnPropertyChanged("Msg");
                }
            }
        }

        private string _msg;

        public IList<string> lessonList { get; set; } = new List<string>();

        public IList<string> lessonNameList { get; set; } = new List<string>();

        public IList<string> characterList { get; set; } = new List<string>();

        public Question aquestion { get; set; } = new Question()
        {
            LessonName = ""
        };

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand _delCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand DelCommand =>
            _delCommand ?? (_delCommand =
                new RelayCommand(async () => await DelCommandFunction()));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task DelCommandFunction()
        {
            await SetValue();
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand _updateCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand UpdateCommand =>
            _updateCommand ?? (_updateCommand =
                new RelayCommand(async () => await UpdateCommandFunction()));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task UpdateCommandFunction()
        {
            var list = await _scheduleService.GetSecondSchedule(lessons);
            foreach (var schedule in list)
            {
                characterList.Add(schedule.Character);
            }
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand _addsItemCommand;

        private readonly IRoutingService _routingService;


        private string _lessonName { set; get; } = "all";

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand AddsItemCommand =>
            _addsItemCommand ?? (_addsItemCommand =
                new RelayCommand(async () => await AddsItemCommandFunction()));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task AddsItemCommandFunction()
        {
            if (Lessonstr == "")
            {
                question = "";
            }
            else
            {
                question = Lessonstr;
            }

            var note = new Note()
            {
                EndTime = (_ddl + _time).ToString(),
                Page = page,
                Questions = question,
                Title = title,
                SetupTime = DateTime.Now.ToString(),
                SubjectId = lesson
            };
            await _noteService.AddNoteAsync(note);
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = note.Title,
                Description = note.Page,
                ReturningData = "Dummy data", // Returning data when tapped on notification.
                Schedule =
                {
                    NotifyTime =
                        Convert.ToDateTime(note.EndTime)
                            .AddSeconds(
                                -20) // Used for Scheduling local notification, if not specified notification will show immediately.
                }
            };
            await NotificationCenter.Current.Show(notification);
            Msg = "success";
            Lessonstr = "";
            await SetValue();
            await PageAppearingCommandFunction();
        }

        private async Task SetValue()
        {
            title = "";
            page = "";
            question = "";
            lesson = "";
            character = "";
            DDL = DateTime.Today;
            Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            lessons = "";
            Msg = "wow";
            lessonList = await _lessonService.GetLessonNameAsync();
            var list = await _scheduleService.GetFirstSchedule();
            foreach (var schedule in list)
            {
                lessonNameList.Add(schedule.LessonName);
            }

            characterList.Clear();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            _lessonName = query.ContainsKey("LessonName") ? HttpUtility.UrlDecode(query["LessonName"]) : "all";
        }

        public string Lessonstr
        {
            get { return _lessonstr; }
            set
            {
                if (_lessonstr != value)
                {
                    _lessonstr = value;
                    OnPropertyChanged("Lessonstr");
                }
            }
        }

        private string _lessonstr;
        private DateTime _ddl;
        private TimeSpan _time;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class NoteView
    {
        public int Id { get; set; }


        public string Title { get; set; }


        public string Page { get; set; }

        public string SetupTime { get; set; }

        public List<Feature> features { get; set; }

        public static NoteView NoteToNoteView(Note note)
        {
            var noteview = new NoteView();
            noteview.Id = note.Id;
            noteview.Page = note.Page;
            noteview.Title = note.Title;
            noteview.SetupTime = DateTime.Parse(note.EndTime).ToString("MM-dd hh:mm");
            noteview.features = new List<Feature>();
            if (note.Questions != null)
            {
                if (note.SubjectId != "")
                {
                    noteview.features.Add(new Feature()
                    {
                        title = note.SubjectId,
                        color = "#f0fbf3",
                        colour = "#74d884"
                    });
                }

                if (note.Questions != "")
                {
                    noteview.features.Add(new Feature()
                    {
                        title = note.Questions,
                        color = "#fefbec",
                        colour = "#fcd058"
                    });
                }
            }

            return noteview;
        }
    }

    public class Feature
    {
        public string title { get; set; }
        public string color { get; set; }
        public string colour { get; set; }
    }
}