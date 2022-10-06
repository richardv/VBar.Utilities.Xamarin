using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VBarUtilities.Views
{
    using System.IO;
    using Data;
    using Microsoft.AppCenter.Analytics;
    using Xamarin.Essentials;

    [QueryProperty("id", "id")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModelPage : ContentPage
    {
        public string id { get; set; }
        public ModelPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var model = await App.Database.GetModel(id);

            Name.Text = model.Name;
            if (model.ImageData.Length > 100)
            {
                ModelImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(model.ImageData.Replace("data:image/png;base64,", ""))));
            }

            var controller = await App.Database.GetController(model.ControllerId);

            Controller.Text = "Controller: " + controller.Name;
            ControllerImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(controller.ImageData.Replace("data:image/png;base64,", ""))));

            var device = await App.Database.GetDevice(model.DeviceId);

            if (device != null)
            {
                Device.Text = "Device: " + device.Name;
            }

            var firstFlight = await App.Database.Flights()
                .Where(f => f.ModelName == model.Name)
                .OrderBy(f => f.DateAndTime)
                .FirstOrDefaultAsync();

            if (firstFlight != null)
            {
                FirstFlight.Text = "First flight: " + Fmt.Day(firstFlight.DateAndTime);
            }

            Flights.Text = "Flights: " + Fmt.No(await App.Database.Flights().CountAsync(f => f.ModelName == model.Name));

            var flights = await App.Database.Flights()
                .Where(f => f.ModelName == model.Name)
                .OrderByDescending(f => f.DateAndTime)
                .ToListAsync();

            FlightList.ItemsSource = flights;
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void ViewFlight_OnClicked(object sender, EventArgs e)
        {
            var flight = (VuFlight)((Button)sender).BindingContext;

            await Shell.Current.GoToAsync($"{nameof(FlightPage)}?DateAndTime={flight.DateAndTime}");
        }
    }
}