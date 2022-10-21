namespace VBarUtilities.Views
{
    using System;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassicPage
    {
        ClassicViewModel _viewModel;

        public ClassicPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ClassicViewModel();
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.Status = string.Empty;
            _viewModel.Progress = 0;
        }

        private async void Import_Clicked(object sender, EventArgs e)
        {
            var logs = await App.ClassicImport.Logs();

            if (logs == null)
            {
                return;
            }

            _viewModel.Status = "Importing Logs";

            var pos = 0;
            var totalFlights = logs.Count;

            foreach (var vbcFlight in logs)
            {
                pos++;
                _viewModel.Progress = (double)pos / totalFlights;

                await App.Database.InsertFlightAsync(new Data.VuFlight
                {
                    ControllerId = "VBC",
                    DateAndTime = vbcFlight.Date,
                    ModelName = vbcFlight.Model,
                    DeviceId = "Unknown",
                    FlightNo = 1,
                    DurationS = vbcFlight.Duration
                });
            }

            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}