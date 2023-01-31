using GalaSoft.MvvmLight.Ioc;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNEU.Services.Validation;
using Xamarin.Forms;

namespace LiveInNEU.ViewModels
{
    public class ViewModelLocator
    {
        public LessonPageViewModel LessonPageViewModel =>
            SimpleIoc.Default.GetInstance<LessonPageViewModel>();

        public LoginPageViewModel LoginPageViewModel =>
            SimpleIoc.Default.GetInstance<LoginPageViewModel>();

        public LoadingPageViewModel LoadingPageViewModel =>
            SimpleIoc.Default.GetInstance<LoadingPageViewModel>();

        public QuestionPageViewModel QuestionPageViewModel =>
            SimpleIoc.Default.GetInstance<QuestionPageViewModel>();

        public QuestionDirectoryPageViewModel QuestionDirectoryPageViewModel =>
            SimpleIoc.Default.GetInstance<QuestionDirectoryPageViewModel>();

        public MenuPageViewModel MenuPageViewModel =>
            SimpleIoc.Default.GetInstance<MenuPageViewModel>();

        public SelfPageViewModel SelfPageViewModel =>
            SimpleIoc.Default.GetInstance<SelfPageViewModel>();


        public MainPageViewModel MainPageViewModel =>
            SimpleIoc.Default.GetInstance<MainPageViewModel>();

        public ScheduleViewModel ScheduleViewModel =>
            SimpleIoc.Default.GetInstance<ScheduleViewModel>();


        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<LoadingPageViewModel>();
            SimpleIoc.Default.Register<LoginPageViewModel>();
            SimpleIoc.Default.Register<LessonPageViewModel>();
            SimpleIoc.Default.Register<QuestionPageViewModel>();
            SimpleIoc.Default.Register<QuestionDirectoryPageViewModel>();
            SimpleIoc.Default.Register<MenuPageViewModel>();
            SimpleIoc.Default.Register<SelfPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<ScheduleViewModel>();

            SimpleIoc.Default.Register<LessonValidator, LessonValidator>();
            SimpleIoc.Default.Register<IAlertService, AlertService>();
            SimpleIoc.Default.Register<IInfoStorage, InfoStorage>();
            SimpleIoc.Default.Register<IRoutingService, RoutingService>();
            SimpleIoc.Default.Register<ILessonService, LessonService>();
            SimpleIoc.Default.Register<ILessonStorage, LessonStorage>();
            SimpleIoc.Default.Register<ILoginService, LoginService>();
            SimpleIoc.Default.Register<ILoginStorage, LoginStorage>();
            SimpleIoc.Default.Register<INeuDataService, NeuDataService>();
            SimpleIoc.Default.Register<IPreferenceStorage, PreferenceStorage>();
            SimpleIoc.Default.Register<ISemesterStorage, SemesterStorage>();
            SimpleIoc.Default.Register<IDataStorage, DataStorage>();
            SimpleIoc.Default.Register<IQuestionService, QuestionService>();
            SimpleIoc.Default.Register<IQuestionStorage, QuestionStorage>();
            SimpleIoc.Default.Register<IScheduleService, ScheduleService>();
            SimpleIoc.Default.Register<IScheduleStorage, ScheduleStorage>();
            SimpleIoc.Default.Register<IMenuStorage, MenuStorage>();
            SimpleIoc.Default.Register<IMenuService, MenuService>();
            SimpleIoc.Default.Register<INoteStorage, NoteStorage>();
            SimpleIoc.Default.Register<INoteService, NoteService>();
            SimpleIoc.Default.Register<IStoreStorage,StoreStorage>();
            SimpleIoc.Default.Register<IRemoteFavoriteStorage, RemoteFavoriteStorage>();
            SimpleIoc.Default.Register<ServiceResult, ServiceResult>();
        }
    }
}