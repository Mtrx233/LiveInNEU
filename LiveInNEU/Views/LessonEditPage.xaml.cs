using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Scheduler;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveInNEU.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonEditPage : Rg.Plugins.Popup.Pages.PopupPage {
        public LessonEditPage(AppointmentItem appointment) {
            InitializeComponent();

            string[] sArray = Regex.Split(appointment.Subject, "\n",
                RegexOptions.IgnoreCase);
            account.Text = sArray[0];
            name.Text = sArray[1];
            teacher.Text = sArray[2];
            location.Text = sArray[3];
            LessonDatePicker.Date = appointment.Start;
            var times = appointment.Start.Hour - 7 > 5
                ? appointment.Start.Hour - 9
                : appointment.Start.Hour - 7;
            ClassBegin.SelectedItem = "第" + times.ToString() + "节课";
            ContinueTime.Value = appointment.Start.Hour == 14
                ? appointment.End.Hour - appointment.Start.Hour + 1
                : appointment.End.Hour - appointment.Start.Hour;
            account.IsEnabled = false;
            Have.IsVisible = true;
            DetailTime.Text = appointment.Start.ToString();
            No.IsVisible = false;
        }

        public LessonEditPage(DateTime date) {
            InitializeComponent();
            LessonDatePicker.Date = date;
            account.Text = "";
            name.Text = "";
            teacher.Text = "";
            location.Text = "";
            var times = date.Hour - 7 > 5 ? date.Hour - 9 : date.Hour - 7;
            ClassBegin.SelectedItem = "第" + times.ToString() + "节课";
            account.IsEnabled = true;
            Have.IsVisible = false;
            No.IsVisible = true;
            ContinueTime.Value = 0;
        }

        public LessonEditPage() {
            InitializeComponent();
        }

        private void BindableObject_OnPropertyChanged(object sender,
            PropertyChangedEventArgs e) {
            if (State.Text == "success!") {
                Navigation.PopPopupAsync();
            }
        }
    }
}