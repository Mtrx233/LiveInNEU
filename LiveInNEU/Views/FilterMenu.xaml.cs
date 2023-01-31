using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Utils.Commands;
using LiveInNEU.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace LiveInNEU.Views
{
    public partial class FilterMenu : ContentView
    {
        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create("SelectedCommand", typeof(ICommand), typeof(FilterMenu), null);

        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        private bool _isAnimating = false;
        private uint _animationDelay = 150;

        public FilterMenu()
        {
            InitializeComponent();

            InnerButtonClose.IsVisible = false;
            InnerButtonMenu.IsVisible = true;
            HandleMenuCenterClicked();
            HandleCloseClicked();
            HandleOptionsClicked();
        }

        private void HandleOptionsClicked()
        {
            //HandleOptionClicked(N, "add");
            //HandleOptionClicked(SW, "doing");
            //HandleOptionClicked(SWs, "done");
        }


        private void HandleOptionClicked(Image image, string value)
        {
            image.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await CloseMenu();
                    if (value == "add") {
                        await Navigation.PushPopupAsync(new NoteAddPage());
                    }
                }),
                NumberOfTapsRequired = 1
            });
        }

        private void HandleCloseClicked()
        {
            InnerButtonClose.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    await CloseMenu();
                }),
                NumberOfTapsRequired = 1
            });

        }

        private async Task CloseMenu()
        {
            if (!_isAnimating)
            {

                _isAnimating = true;

                InnerButtonMenu.IsVisible = true;
                InnerButtonClose.IsVisible = true;
                await HideButtons();

                await InnerButtonClose.RotateTo(0, _animationDelay);
                await InnerButtonClose.FadeTo(0, _animationDelay);
                await InnerButtonMenu.RotateTo(0, _animationDelay);
                await InnerButtonMenu.FadeTo(1, _animationDelay);
                await OuterCircle.ScaleTo(1, 100, Easing.Linear);
                InnerButtonClose.IsVisible = false;

                _isAnimating = false;
            }
        }

        private void HandleMenuCenterClicked()
        {
            InnerButtonMenu.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    if (!_isAnimating)
                    {
                        _isAnimating = true;

                        InnerButtonClose.IsVisible = true;
                        InnerButtonMenu.IsVisible = true;

                        await InnerButtonMenu.RotateTo(360, _animationDelay);
                        await InnerButtonMenu.FadeTo(0, _animationDelay);
                        await InnerButtonClose.RotateTo(360, _animationDelay);
                        await InnerButtonClose.FadeTo(1, _animationDelay);
                        await OuterCircle.ScaleTo(3.5, 50, Easing.Linear);
                        await ShowButtons();
                        InnerButtonMenu.IsVisible = false;

                        _isAnimating = false;

                    }
                }),
                NumberOfTapsRequired = 1
            });
        }

        private async Task HideButtons()
        {
            var speed = 25U;
            await N.FadeTo(0, speed);
            await SW.FadeTo(0, speed);
            await SWs.FadeTo(0, speed);
        }

        private async Task ShowButtons()
        {
            var speed = 25U;
            await N.FadeTo(1, speed);
            await SW.FadeTo(1, speed);
            await SWs.FadeTo(1, speed);
        }

        private async void OnImageButtonClicked(object sender, EventArgs e) {
            await Navigation.PushPopupAsync(new NoteAddPage());
        }
    }
}