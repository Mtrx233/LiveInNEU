using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Scheduler;
using LiveInNEU.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveInNEU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteAddPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public NoteAddPage(string name)
        {
            InitializeComponent();
            ClassChoose.IsVisible = false;
            infos.Text = name;
        }

        public NoteAddPage()
        {
            InitializeComponent();
            ClassChoose.IsVisible = true;
        }

        private void BindablesObject_OnPropertyChanged(object sender,
            PropertyChangedEventArgs e)
        {
            if (Nows.Text == "success")
            {
                Navigation.PopPopupAsync();
            }
        }
    }
}