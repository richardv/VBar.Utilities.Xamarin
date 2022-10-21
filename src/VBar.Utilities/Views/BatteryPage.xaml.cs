namespace VBarUtilities.Views
{
    using System;
    using System.Linq;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [QueryProperty("id", "id")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BatteryPage : ContentPage
    {
        public string id { get; set; }

        public BatteryPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Name.Text = id;

            var vuBattery = await App.Database.GetBatteryById(id);

            if (vuBattery == null)
            {
                vuBattery = await App.Database.Batteries().Where(b => b.Name == id).FirstOrDefaultAsync();
            }

            if (vuBattery != null)
            {
                Spec.Text = Fmt.Cells(vuBattery.Cells) + " " + Fmt.MAh(vuBattery.mAh);
                FlightCount.Text = Fmt.No(vuBattery.Flights) + " flights";
                StoreAging.Text = "Store aging: " + Fmt.MAh(vuBattery.StoreAging) + "/100d";
                FlightAging.Text = "Flight aging: " + Fmt.MAh(vuBattery.FlightAging) + "/Flight";

                var flightDetails = from fd in (await App.Database.FlightDetails()
                    .Where(f => f.BatteryName == vuBattery.Name)
                    .ToArrayAsync())
                                    orderby fd.Date
                                    select new
                                    {
                                        Date = fd.Date.Date.ToString("d MMM yyyy"),
                                        fd.Capacity,
                                        fd.CapacityUsed,
                                        fd.VoltEnd,
                                        fd.VoltMin,
                                        fd.VoltStart,
                                        fd.AmpsMax
                                    };

                FlightsFound.Text = flightDetails.Count() + " flights found";

                Series.ItemsSource = flightDetails;
                Series2.ItemsSource = flightDetails;
                Series3.ItemsSource = flightDetails;
                Series4.ItemsSource = flightDetails;
                Series6.ItemsSource = flightDetails;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Series.ItemsSource = null;
            Series2.ItemsSource = null;
            Series3.ItemsSource = null;
            Series4.ItemsSource = null;
            Series6.ItemsSource = null;

            Chart = null;
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }
    }
}