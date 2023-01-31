using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveInNEU.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListEventTypesView : ContentView
    {
        public ListEventTypesView()
        {
            InitializeComponent();
        }

        private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e) {
            
        }
    }
}