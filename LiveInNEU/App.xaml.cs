using LiveInNEU.Services;
using LiveInNEU.Views;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveInNEU
{
    public partial class App : Application
    {

        // UIParent used by Android version of the app
        public static object AuthUIParent;

        // Keychain security group used by iOS version of the app
        public static string iOSKeychainSecurityGroup;

        public App()
        {
            DevExpress.XamarinForms.Scheduler.Initializer.Init();
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
