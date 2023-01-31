using System.Threading.Tasks;
using LiveInNEU.Views;
using Xamarin.Forms;

namespace LiveInNEU.Services.implementations {
    /// <summary>
    /// 弹窗
    /// </summary>
    /// <author>殷昭伉</author>
    public class AlertService : IAlertService
    {
        /******** 公开变量 ********/

        /******** 私有变量 ********/

        private LoginPage MainPage => _mainPage ??
            (_mainPage = Shell.Current.CurrentPage as LoginPage);

        private LoginPage _mainPage;

        /******** 继承方法 ********/

        /// <summary>
        /// 显示警告。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="message">信息。</param>
        /// <param name="button">按钮文字。</param>
        public async Task ShowAlertAsync(string title, string message,
            string button) =>
            Device.BeginInvokeOnMainThread(async () =>
                await MainPage.DisplayAlert(title, message, button));

        private LessonPage LessonPage  => _lessonPage ??
                                      (_lessonPage = Shell.Current.CurrentPage as LessonPage);

        private LessonPage _lessonPage;

        public async Task ShowsAlertAsync(string title, string message,
            string button) =>
            Device.BeginInvokeOnMainThread(async () =>
                await LessonPage.DisplayAlert(title, message, button));
        /******** 公开方法 ********/

        /******** 私有方法 ********/
    }
}