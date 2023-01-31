using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNEU.Services.Validation;
using LiveInNEU.Utils;
using LiveInNEU.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LiveInNEU.ViewModels
{
    /// <author>钱子昂，殷昭伉（互相写不调用自己的那部分）</author>
    public class LessonPageViewModel : ViewModelBase
    {
        /******** 构造函数 ********/

        /// <summary>
        /// 课程服务
        /// </summary>
        private ILessonService _lessonService;

        /// <summary>
        /// 验证服务
        /// </summary>
        private readonly LessonValidator _validator;


        public LessonPageViewModel(ILessonService lessonService,
            LessonValidator lessonValidator,
            INeuDataService neuDataService,
            IRoutingService routingService)
        {
            _neuDataService = neuDataService;
            _lessonService = lessonService;
            _validator = lessonValidator;
            _routingService = routingService;
            CreateLabels();
        }


        /******** 绑定属性 ********/


        /// <summary>
        /// 课程开始时间
        /// </summary>
        public static DateTime BaseTime { get; } = DateTime.Today;

        /// <summary>
        /// 课程信息容器
        /// </summary>
        public ObservableCollection<LessonShowData> LessonsCollection { get; set; } =
            new ObservableCollection<LessonShowData>();

        /// <summary>
        /// 课程标签颜色
        /// </summary>
        public ObservableCollection<MedicalAppointmentType> Labels { get; private set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public string LessonID { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string LessonName { get; set; }

        /// <summary>
        /// 老师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 开始节数
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 上课持续节次
        /// </summary>
        public int ContinueTime { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime LessonDate { get; set; }

        /// <summary>
        /// 上课时间 如 2021/12/12 10：20
        /// </summary>
        public string DetailTime { get; set; }

        /// <summary>
        /// 数据验证信息
        /// </summary>
        public string ValidateMsg
        {
            get => _validateMsg;
            set => Set(nameof(ValidateMsg), ref _validateMsg, value);
        }

        private string _validateMsg;


        /******** 绑定命令 ********/

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand _lessonScheduleCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand LessonScheduleCommand =>
            _lessonScheduleCommand ?? (_lessonScheduleCommand =
                new RelayCommand(async () =>
                {
                    ValidateMsg = ErrorMessages.LESSON_EDIT_SUCESS;
                    ValidateMsg = ErrorMessages.LESSON_EDIT_INIT;
                    await _routingService.NavigateToAsync(
                        $"{nameof(SchedulePage)}?LessonName={LessonName}");
                }));

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

        private AlertService _alertService = new AlertService();

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task PageAppearingCommandFunction()
        {
            IsLoading = true;
            var a = await _neuDataService.Update();
            if (Preferences.ContainsKey("Update"))
            {
                var m = await _neuDataService.Update(1);
                await _alertService.ShowsAlertAsync("提示", m, "确定");
                Preferences.Remove("Update");
            }

            await _alertService.ShowsAlertAsync("提示", a, "确定");
            await LessonsCollectionLoad();
            IsLoading = false;
        }

        /// <summary>
        /// 课程列表加载指令
        /// </summary>
        public async Task LessonsCollectionLoad()
        {
            LessonsCollection.Clear();
            var lessonList = await _lessonService.GetLesson();
            foreach (var lesson in lessonList)
            {
                LessonsCollection.Add(lesson);
            }
        }

        /// <summary>
        /// 课程修改指令
        /// </summary>
        public RelayCommand _lessonEditCommand;

        /// <summary>
        /// 课程修改指令
        /// </summary>
        public RelayCommand LessonEditCommand =>
            _lessonEditCommand ?? (_lessonEditCommand =
                new RelayCommand(async () => await LessonEdit()));

        /// <summary>
        /// 课程修改指令函数
        /// </summary>
        public async Task LessonEdit()
        {
            var validationResult = ValidateInput();
            if (validationResult.IsValid)
            {
                var result = await _lessonService.UpdateLesson(LessonID, LessonName,
                    TeacherName, Location, LessonDate, StartTime, ContinueTime,
                    Convert.ToDateTime(DetailTime));
                if (result)
                {
                    await LessonsCollectionLoad();
                    ValidateMsg = ErrorMessages.LESSON_EDIT_SUCESS;
                    ValidateMsg = ErrorMessages.LESSON_EDIT_INIT;
                }
                else
                {
                    ValidateMsg = ErrorMessages.LESSON_EDIT_CONFLICT;
                }
            }
            else
            {
                if (validationResult.Errors.Count > 0)
                {
                    ValidateMsg = validationResult.Errors[0].ErrorMessage;
                }
                else
                {
                    ValidateMsg = "erro!";
                }
            }
        }

        /// <summary>
        /// 课程添加指令
        /// </summary>
        public RelayCommand _lessonAddCommand;

        /// <summary>
        /// 课程添加指令
        /// </summary>
        public RelayCommand LessonAddCommand =>
            _lessonAddCommand ?? (_lessonAddCommand =
                new RelayCommand(async () => await LessonAdd()));

        /// <summary>
        /// 课程添加指令函数
        /// </summary>
        public async Task LessonAdd()
        {
            var validationResult = ValidateInput();
            if (validationResult.IsValid)
            {
                var result = await _lessonService.AddLesson(LessonID,
                    LessonName, TeacherName, Location, LessonDate, StartTime,
                    ContinueTime);
                if (result)
                {
                    await LessonsCollectionLoad();
                    ValidateMsg = ErrorMessages.LESSON_EDIT_SUCESS;
                    ValidateMsg = ErrorMessages.LESSON_EDIT_INIT;
                }
                else
                {
                    ValidateMsg = ErrorMessages.LESSON_EDIT_CONFLICT;
                }
            }
            else
            {
                if (validationResult.Errors.Count > 0)
                {
                    ValidateMsg = validationResult.Errors[0].ErrorMessage;
                }
                else
                {
                    ValidateMsg = ErrorMessages.ERRO;
                }
            }
        }

        /// <summary>
        /// 课程删除指令
        /// </summary>
        public RelayCommand _lessonDelCommand;

        /// <summary>
        /// 课程删除指令
        /// </summary>
        public RelayCommand LessonDelCommand =>
            _lessonDelCommand ?? (_lessonDelCommand =
                new RelayCommand(async () => await LessonDel()));

        /// <summary>
        /// 课程删除指令函数
        /// </summary>
        public async Task LessonDel()
        {
            var x = LessonID;
            var y = Convert.ToDateTime(DetailTime);
            await _lessonService.DelLesson(LessonID,
                Convert.ToDateTime(DetailTime));
            await LessonsCollectionLoad();
            ValidateMsg = ErrorMessages.LESSON_EDIT_SUCESS;
            ValidateMsg = ErrorMessages.LESSON_EDIT_INIT;
        }


        /******** 额外方法 ********/
        /// <summary>
        /// 颜色标签创建指令
        /// </summary>
        public void CreateLabels()
        {
            ObservableCollection<MedicalAppointmentType> result =
                new ObservableCollection<MedicalAppointmentType>();
            for (int i = 0; i < 18; i++)
            {
                MedicalAppointmentType appointmentType =
                    new MedicalAppointmentType();
                appointmentType.Id = i;
                appointmentType.Caption = "label";
                appointmentType.Color = AppointmentTypeColors[i];
                result.Add(appointmentType);
            }

            Labels = result;
        }

        /// <summary>
        /// 验证输入
        /// </summary>
        /// <returns></returns>
        public ValidationResult ValidateInput()
        {
            Lesson lesson = new Lesson();
            int start = StartTime.Length == 4
                ? int.Parse(StartTime.Substring(1, 1))
                : int.Parse(StartTime.Substring(1, 2));
            lesson.StartTime = start;
            lesson.Name = LessonName;
            lesson.Id = LessonID;
            lesson.ContinueTime = ContinueTime;
            return _validator.Validate(lesson);
        }


        /******** 私有变量 ********/

        /// <summary>
        /// 是否完成初始化
        /// </summary>
        private bool _isInited = false;

        /// <summary>
        /// 课程标签颜色
        /// </summary>
        public static Color[] AppointmentTypeColors =
        {
            Color.FromHex("#949494"),
            Color.FromHex("#F15558"),
            Color.FromHex("#FF7C11"),
            Color.FromHex("#FFBF22"),
            Color.FromHex("#FF6E86"),
            Color.FromHex("#9865b0"),
            Color.FromHex("#756CFD"),
            Color.FromHex("#0055D8"),
            Color.FromHex("#01B0EE"),
            Color.FromHex("#0097AD"),
            Color.FromHex("#00C831"),
            Color.FromHex("#CD853F"),
            Color.FromHex("#008B8B"),
            Color.FromHex("#FFE7BA"),
            Color.FromHex("#D2691E"),
            Color.FromHex("#FF0000"),
            Color.FromHex("#66CDAA"),
            Color.FromHex("#FFFF00"),
            Color.FromHex("#CD853F"),
            Color.FromHex("#F0FFF0"),
            Color.FromHex("#FFE7BA")
        };

        private readonly INeuDataService _neuDataService;
        private readonly IRoutingService _routingService;
    }

    /// <summary>
    /// 课程样式标签
    /// </summary>
    public class MedicalAppointmentType
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color { get; set; }
    }
}