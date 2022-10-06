namespace VBarUtilities.Views
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AppCenter.Analytics;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BatteriesPage
    {
        public BatteriesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var batteryVms = new List<BatteryVm>();

            var batteries = await App.Database.Batteries().OrderBy(b => b.Name).ToListAsync();

            foreach (var battery in batteries)
            {
                batteryVms.Add(new BatteryVm
                {
                    Name = battery.Name,
                    Size = battery.Cells + "S " + Fmt.No(battery.mAh) + " mAh",
                    TotalFlights = battery.Flights,
                    TotalDuration = battery.FlightTimeS,
                    LastFlight = battery.LastFlight,
                    LastDuration = battery.LastDurationS,
                    FirstFlight = battery.FirstFlight
                });
            }

            BatteryCount.Text = $"{Fmt.No(batteryVms.Count)} batteries found";

            Batteries.ItemsSource = batteryVms;
        }

        private async void OpenVStabiBatteries_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/cloud?action=&sort=New+first&start=0&Aid=All+VBC-t&vbar=All+VBar&action=battlist", BrowserLaunchMode.External);
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void View_OnClicked(object sender, EventArgs e)
        {
            var batteryVm = (BatteryVm)((Button)sender).BindingContext;

            await Shell.Current.GoToAsync(nameof(BatteryPage) + "?id=" + batteryVm.Name);
        }
    }
}