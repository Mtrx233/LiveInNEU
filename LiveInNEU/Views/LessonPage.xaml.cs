using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Scheduler;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveInNEU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonPage : ContentPage
    {
        public LessonPage()
        {
            InitializeComponent();
        }
        private void WorkWeekView_Tap(object sender, DevExpress.XamarinForms.Scheduler.SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo == null)
            {
                ShowNewAppointmentEditPage(e.IntervalInfo);
                return;
            }
            AppointmentItem appointment = e.AppointmentInfo.Appointment;
            ShowAppointmentEditPage(appointment);
        }

        private void ShowAppointmentEditPage(AppointmentItem appointment)
        {
            Navigation.PushPopupAsync(new LessonEditPage(appointment));
        }

        private void ShowNewAppointmentEditPage(IntervalInfo info)
        {
            if (info == null) {
                return;
            }
            Navigation.PushPopupAsync(new LessonEditPage(info.Start));
        }
    }
}