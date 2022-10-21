namespace VBarUtilities.Views
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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

            await LoadBatteries();
        }

        private async Task LoadBatteries()
        {
            var batteryVms = new List<BatteryVm>();

            var batteries = await App.Database.Batteries()
                .OrderByDescending(b => b.Favourite)
                .ThenBy(b => b.Name)
                .ToListAsync();

            foreach (var battery in batteries)
            {
                batteryVms.Add(new BatteryVm
                {
                    Id = battery.Id,
                    Name = battery.Name,
                    Size = battery.Cells + "S " + Fmt.No(battery.mAh) + " mAh",
                    TotalFlights = battery.Flights,
                    TotalDuration = battery.FlightTimeS,
                    LastFlight = battery.LastFlight,
                    LastDuration = battery.LastDurationS,
                    FirstFlight = battery.FirstFlight,
                    Favourite = battery.Favourite
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
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void View_OnClicked(object sender, EventArgs e)
        {
            var batteryVm = (BatteryVm)((Button)sender).BindingContext;

            await Shell.Current.GoToAsync(nameof(BatteryPage) + "?id=" + batteryVm.Name);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var batteryVm = (BatteryVm)((Button)sender).BindingContext;

            var vuBattery = await App.Database.GetBatteryById(batteryVm.Id);

            vuBattery.Favourite = !batteryVm.Favourite;

            await App.Database.UpdateBatteryAsync(vuBattery);

            await LoadBatteries();
        }
    }
}