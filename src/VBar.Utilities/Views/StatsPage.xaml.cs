using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VBarUtilities.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsPage : ContentPage
    {
        public StatsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


        }
    }
}