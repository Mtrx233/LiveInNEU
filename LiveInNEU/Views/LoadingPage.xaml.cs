using LiveInNEU.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveInNEU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }
        

        // internal LoadingPageViewModel ViewModel { get; set; } = Locator.Current.GetService<LoadingPageViewModel>();

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // ViewModel.Init();
		}
    }
}