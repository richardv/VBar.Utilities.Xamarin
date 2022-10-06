namespace VBarUtilities.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.AppCenter.Analytics;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControllersPage
    {
        public ControllersPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var controllerVms = new List<ControllerVm>();

            var flights = await App.Database.Flights().ToListAsync();

            var controllers = await App.Database.Controllers().OrderByDescending(b => b.Name).ToListAsync();

            foreach (var controller in controllers)
            {
                controllerVms.Add(new ControllerVm
                {
                    Name = controller.Name,
                    ImageData = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(controller.ImageData.Replace("data:image/png;base64,", "")))),
                    Flights = flights.Count(),
                    FirstDate = flights.Min(f => f.DateAndTime),
                    LastDate = flights.Max(f => f.DateAndTime),
                });
            }

            Controllers.ItemsSource = controllerVms;

            ControllerCount.Text = $"{Fmt.No(controllers.Count)} controller{Fmt.Plural(controllers.Count)} found";
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void VStabiControllers_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/cloud", BrowserLaunchMode.External);
        }
    }
}