namespace VBarUtilities.Views
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AppCenter.Analytics;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [QueryProperty("FromDate", "FromDate")]
    [QueryProperty("ToDate", "ToDate")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightsByDatePage : ContentPage
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public FlightsByDatePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var fromDate = DateTime.Parse(FromDate);
            var toDate = DateTime.Parse(ToDate);

            Title.Text = "Flights from " + fromDate.ToLongDateString() + " to " + toDate.ToLongDateString();

            var flights = await App.Database.Flights()
                .Where(f => f.DateAndTime >= fromDate && f.DateAndTime <= toDate)
                .OrderByDescending(f => f.DateAndTime)
                .ToListAsync();

            FlightCount.Text = flights.Count + " flights found";

            Flights.ItemsSource = flights;
        }

        private async void ViewFlight_OnClicked(object sender, EventArgs e)
        {
            var flight = (VuFlight)((Button)sender).BindingContext;

            await Shell.Current.GoToAsync($"{nameof(FlightPage)}?DateAndTime={flight.DateAndTime}");
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }
    }
}