using LiveInNEU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using LiveInNEU.ViewModels;
using LiveInNEU.Views;
using Xamarin.Forms;

namespace LiveInNEU
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(QuestionPage), typeof(QuestionPage));
            Routing.RegisterRoute(nameof(LessonPage), typeof(LessonPage));
            Routing.RegisterRoute(nameof(QuestionDirectoryPage), typeof(QuestionDirectoryPage));
            Routing.RegisterRoute(nameof(SchoolCalendarPage), typeof(SchoolCalendarPage));
            Routing.RegisterRoute(nameof(QuestionCollectedPage), typeof(QuestionCollectedPage));
        }
    }
}